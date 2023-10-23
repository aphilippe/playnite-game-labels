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
        IGameLabelConditionSerializer Serializer { get; }
        bool Test(Game game);
    }

    public interface IGameLabelConditionSerializer
    {
        Dictionary<string,  object> Serialize(IGameLabelCondition condition);
        IGameLabelCondition Deserialize(Dictionary<string, object> serializedValue);
    }

    public class AlwaysTrueGameLabelCondition : IGameLabelCondition
    {
        public IGameLabelConditionSerializer Serializer => throw new NotImplementedException();

        public bool Test(Game game)
        {
            return true;
        }
    }
    public class AlwaysFalseGameLabelCondition : IGameLabelCondition
    {
        public IGameLabelConditionSerializer Serializer => throw new NotImplementedException();

        public bool Test(Game game)
        {
            return false;
        }
    }
}
