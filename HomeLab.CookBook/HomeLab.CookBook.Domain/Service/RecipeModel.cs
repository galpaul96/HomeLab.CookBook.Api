namespace HomeLab.CookBook.Domain.Service
{
    public class RecipeModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public IEnumerable<StepModel> Steps { get; set; }
    }
}
