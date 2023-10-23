using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameLabels.Conditions
{
    internal class GuidGameLabelCondition : IGameLabelCondition
    {
        public Guid Guid { get; }

        public PropertyInfo PropertyInfo { get; }

        public IGameLabelConditionSerializer Serializer => new GuidGameLabelConditionSerializer();

        public GuidGameLabelCondition(Guid guid, PropertyInfo propertyInfo)
        {
            Guid = guid;
            PropertyInfo = propertyInfo;
        }

        public bool Test(Game game)
        {
            var objects = PropertyInfo.GetValue(game, null) as IEnumerable<Guid>;
            return objects?.Contains(Guid) ?? false;
        }
    }

    internal class GuidGameLabelConditionSerializer : IGameLabelConditionSerializer
    {
        public IGameLabelCondition Deserialize(Dictionary<string, object> serializedValue)
        {
            var property = typeof(Game).GetProperty(serializedValue["PropertyName"].ToString());
            var guid = new Guid(serializedValue["Guid"].ToString());
            return new GuidGameLabelCondition(guid, property);
        }

        public Dictionary<string, object> Serialize(IGameLabelCondition condition)
        {
            return new Dictionary<string, object>
            {
                    { "Guid", ((GuidGameLabelCondition)condition).Guid },
                    { "PropertyName", ((GuidGameLabelCondition)condition).PropertyInfo.Name }
            };
        }
    }
}
