namespace HomeLab.CookBook.API.Models
{
    public class RecipeOverviewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        public int NoOfSteps { get; set; }
        public string Duration { get; set; }
    }
}
