using GameLabels.Conditions;
using GameLabels.Settings;
using Playnite.SDK;
using Playnite.SDK.Data;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameLabels
{
    public class GameLabelsSettings : ObservableObject
    {
        private ObservableCollection<GameLabel> labels = new ObservableCollection<GameLabel>();

        [DontSerialize]
        public ObservableCollection<GameLabel> Labels { get => labels; set => SetValue(ref labels, value); } 
    }

    internal class SeriazableGameLabel
    {
        public string Text { get; set; }
        public Brush BackgroundColor { get; set; }
        public Brush TextColor { get; set; }
        public string ConditionSerializerName { get; set; }
        public Dictionary<string, object> ConditionArguments { get; set; }
    }

    public class GameLabelsSettingsViewModel : ObservableObject, ISettings
    {
        private readonly GameLabels plugin;
        private GameLabelsSettings editingClone { get; set; }

        private GameLabelsSettings settings;
        public GameLabelsSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<GameLabelSettingsViewModel> labels;
        public ObservableCollection<GameLabelSettingsViewModel> Labels
        {
            get => labels;
            set
            {
                labels = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddLabelCommand { get; }
        public RelayCommand<GameLabelSettingsViewModel> DeleteLabelCommand { get; }

        public GameLabelsSettingsViewModel(GameLabels plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;
            
            Settings = new GameLabelsSettings();

            try
            {
                // Load saved settings.
                var savedSettings = plugin.LoadPluginSettings<IEnumerable<SeriazableGameLabel>>();

                // LoadPluginSettings returns null if no saved data is available.
                if (savedSettings != null)
                {
                    Settings.Labels = savedSettings.Select(x =>
                    {
                        var objectHandle = Activator.CreateInstance(null, x.ConditionSerializerName);
                        var conditionSerializer = (IGameLabelConditionSerializer)objectHandle.Unwrap();

                        var condition = conditionSerializer.Deserialize(x.ConditionArguments);
                        return new GameLabel(x.Text, x.BackgroundColor, x.TextColor, condition);
                    }).ToObservable();
                }
            }
            catch (Exception) { }

            Labels = settings.Labels.Select(label => new GameLabelSettingsViewModel(label, plugin.PlayniteApi)).ToObservable();

            AddLabelCommand = new RelayCommand( () => 
            {
                var label = new GameLabel {
                    Text = "New",
                    BackgroundColor = new SolidColorBrush(Color.FromRgb(0x0, 0x0, 0x0)),
                    TextColor = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Condition = new AlwaysTrueGameLabelCondition()
                }; 
                Labels.Add(new GameLabelSettingsViewModel(label, plugin.PlayniteApi));
            } );

            DeleteLabelCommand = new RelayCommand<GameLabelSettingsViewModel>(label =>
            {
                Labels.Remove(label);
            });
        }

        public void BeginEdit()
        {
            // Code executed when settings view is opened and user starts editing values.
            editingClone = Serialization.GetClone(Settings);
        }

        public void CancelEdit()
        {
            // Code executed when user decides to cancel any changes made since BeginEdit was called.
            // This method should revert any changes made to Option1 and Option2.
            Settings = editingClone;
        }

        public void EndEdit()
        {
            // Code executed when user decides to confirm changes made since BeginEdit was called.
            // This method should save settings made to Option1 and Option2.

            Settings.Labels = Labels.Select(x => x.GetLabel()).ToObservable();

            var plop = Settings.Labels.Select(x => 
            {
                var conditionSerializer = x.Condition.Serializer;
                
                var serializeLabel = new SeriazableGameLabel
                {
                    BackgroundColor = x.BackgroundColor,
                    Text = x.Text,
                    TextColor = x.TextColor,
                    ConditionSerializerName = conditionSerializer.GetType().FullName,
                    ConditionArguments = conditionSerializer.Serialize(x.Condition)
                };
                return serializeLabel;
            }).ToList();

            plugin.SavePluginSettings(plop);
        }

        public bool VerifySettings(out List<string> errors)
        {
            // Code execute when user decides to confirm changes made since BeginEdit was called.
            // Executed before EndEdit is called and EndEdit is not called if false is returned.
            // List of errors is presented to user if verification fails.
            errors = new List<string>();
            return true;
        }
    }
}