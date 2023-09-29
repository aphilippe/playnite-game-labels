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
        public DatabaseObject DatabaseObject { get; }

        public PropertyInfo PropertyInfo { get; }

        public GuidGameLabelCondition(DatabaseObject databaseObject, PropertyInfo propertyInfo)
        {
            DatabaseObject = databaseObject;
            PropertyInfo = propertyInfo;
        }

        public bool Test(Game game)
        {
            game.TagIds
            var objects = PropertyInfo.GetValue(game, null) as IEnumerable<DatabaseObject>;
            return objects?.Contains(DatabaseObject) ?? false;
        }
    }
}
