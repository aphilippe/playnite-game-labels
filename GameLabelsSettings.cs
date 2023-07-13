using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameLabels
{
    public class GameLabelsSettings : ObservableObject
    {
        private ObservableCollection<GameLabel> labels = new ObservableCollection<GameLabel> {
            new GameLabel { Text = "DEMO", BackgroundColor = new SolidColorBrush(Color.FromRgb(0xae, 0xd7, 0x7c)), TextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0)) },
            new GameLabel { Text = "DLC", BackgroundColor = new SolidColorBrush(Color.FromRgb(0x9d, 0x51, 0xaa)), TextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0)) },
            new GameLabel { Text = "EARLYBAYE", BackgroundColor = new SolidColorBrush(Color.FromRgb(0x3c, 0x6d, 0x9d)), TextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0)) },
        };

        [DontSerialize]
        public ObservableCollection<GameLabel> Labels { get => labels; set => SetValue(ref labels, value); } 

        private string option1 = string.Empty;
        private bool option2 = false;
        private bool optionThatWontBeSaved = false;

        public string Option1 { get => option1; set => SetValue(ref option1, value); }
        public bool Option2 { get => option2; set => SetValue(ref option2, value); }
        // Playnite serializes settings object to a JSON object and saves it as text file.
        // If you want to exclude some property from being saved then use `JsonDontSerialize` ignore attribute.
        [DontSerialize]
        public bool OptionThatWontBeSaved { get => optionThatWontBeSaved; set => SetValue(ref optionThatWontBeSaved, value); }
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

        public RelayCommand AddLabelCommand { get; }
        public RelayCommand<GameLabel> DeleteLabelCommand { get; }

        public GameLabelsSettingsViewModel(GameLabels plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;

            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<GameLabelsSettings>();

            // LoadPluginSettings returns null if no saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new GameLabelsSettings();
            }

            AddLabelCommand = new RelayCommand( () => 
            { 
                Settings.Labels.Add(new GameLabel { Text = "New", BackgroundColor = new SolidColorBrush(Color.FromRgb(0x0, 0x0, 0x0)), TextColor = new SolidColorBrush(Color.FromRgb(255, 255, 255)) }); 
            } );

            DeleteLabelCommand = new RelayCommand<GameLabel>(label =>
            {
                Settings.Labels.Remove(label);
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
            plugin.SavePluginSettings(Settings);
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