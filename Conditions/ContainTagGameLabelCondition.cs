using Playnite.SDK.Models;

namespace GameLabels.Conditions
{
    public class ContainTagGameLabelCondition : IGameLabelCondition
    {
        public ContainTagGameLabelCondition(Tag tag)
        {
            Tag = tag;
        }

        Tag Tag { get; set; }

        public bool Test(Game game)
        {
            return game.Tags?.Contains(Tag) ?? false;
        }
    }

    public class ContainCategoryGameLabelCondition : IGameLabelCondition
    { 
        Category Category { get; set; }
        public bool Test(Game game)
        {
            return game.Categories?.Contains(Category) ?? false;
        }

        public ContainCategoryGameLabelCondition(Category category)
        {
            Category = category;
        }
    }
}
