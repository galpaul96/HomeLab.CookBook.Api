namespace HomeLab.CookBook.API.Models
{
    public class StepDetailsModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        public RecipeOverviewModel Recipe { get; set; }
        public ICollection<IngredientOverviewModel> Ingredients { get; set; }
    }
}
