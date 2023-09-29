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
}
