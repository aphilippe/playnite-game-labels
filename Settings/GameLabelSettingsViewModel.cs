using GameLabels.Conditions;
using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameLabels.Settings
{
    public class GameLabelSettingsViewModel : ObservableObject
    {
        private readonly IPlayniteAPI playniteAPI;
        public string Text { get => label.Text; set { label.Text = value; } }
        public Brush BackgroundColor
        {
            get => label.BackgroundColor; 
            set
            {
                label.BackgroundColor = value;
                OnPropertyChanged();
            }
        }
        public Brush TextColor
        {
            get => label.TextColor; 
            set
            {
                label.TextColor = value;
                OnPropertyChanged();
            }
        }

        private DatabaseObject databaseObject;
        public DatabaseObject DatabaseObject { 
            get
            {
                if (databaseObject != null)
                {
                    return databaseObject;
                }

                if (label.Condition is GuidGameLabelCondition)
                {
                    DatabaseObject = playniteAPI.Database.Tags.FirstOrDefault(x => x.Id == ((GuidGameLabelCondition)label.Condition).Guid);
                }
                return databaseObject;
            }
            set => databaseObject = value;
        }

        private readonly GameLabel label;
        public IItemCollection Items { get; set; }

        public GameLabelSettingsViewModel(GameLabel label, IPlayniteAPI api)
        {
            playniteAPI = api;
            
            this.label = label;
            
            Items = playniteAPI.Database.Tags;

            
        }

        public GameLabel GetLabel() 
        {
            if (DatabaseObject == null) return label;

            label.Condition = new GuidGameLabelCondition(DatabaseObject.Id, typeof(Game).GetProperty(nameof(Game.TagIds)));
            return label;
        }
    }
}
