namespace HomeLab.CookBook.API.Models
{
    public class SubStepDetailsModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        public StepOverviewModel Step { get; set; }
        public ICollection<IngredientOverviewModel> Ingredients { get; set; }
    }
}
