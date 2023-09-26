using Playnite.SDK.Controls;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameLabels
{
    /// <summary>
    /// Interaction logic for GameLabelsContainer.xaml
    /// </summary>
    public partial class GameLabelsContainer : PluginUserControl, INotifyPropertyChanged
    {
        private GameLabelsSettingsViewModel _settings;
        private IEnumerable<GameLabel> labels;

        public IEnumerable<GameLabel> Labels 
        {
            get => labels; 
            private set 
            { 
                labels = value;
                NotifyPropertyChanged();
            } 
        }

        public GameLabelsContainer(GameLabelsSettingsViewModel settings)
        {
            DataContext = this;

            InitializeComponent();

            _settings = settings;
        }

        private void UpdateLabels()
        {
            Labels = _settings.Settings.Labels.Where(x => x.Condition.Test(GameContext));
        }

        #region Events

        public override void GameContextChanged(Game oldContext, Game newContext)
        {
            if (oldContext != null)
            {
                oldContext.PropertyChanged -= Context_PropertyChanged;
            }

            newContext.PropertyChanged += Context_PropertyChanged;
            UpdateLabels();
        }

        private void Context_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.Tags))
            {
                UpdateLabels();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
