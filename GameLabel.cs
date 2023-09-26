using GameLabels.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameLabels
{
    public class GameLabel
    {
        public string Text { get; set; }
        public Brush BackgroundColor { get; set; }
        public Brush TextColor { get; set; }
        public IGameLabelCondition Condition { get; set; }

        public GameLabel(string text, Brush backgroundColor, Brush textColor, IGameLabelCondition condition) 
        {
            Text = text;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            Condition = condition;
        }

        public GameLabel() { }
    }
}
