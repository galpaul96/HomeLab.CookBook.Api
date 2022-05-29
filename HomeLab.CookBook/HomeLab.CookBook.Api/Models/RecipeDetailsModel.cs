namespace HomeLab.CookBook.API.Models
{
    public class RecipeDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public IEnumerable<StepOverviewModel> Steps { get; set; }
    }
}
