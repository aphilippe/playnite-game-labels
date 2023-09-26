using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLabels.Conditions
{
    public interface IGameLabelCondition
    {
        bool Test(Game game);
    }

    public class AlwaysTrueGameLabelCondition : IGameLabelCondition
    {
        public bool Test(Game game)
        {
            return true;
        }
    }
    public class AlwaysFalseGameLabelCondition : IGameLabelCondition
    {
        public bool Test(Game game)
        {
            return false;
        }
    }
}
