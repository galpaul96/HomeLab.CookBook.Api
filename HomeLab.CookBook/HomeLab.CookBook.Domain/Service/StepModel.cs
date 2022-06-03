namespace HomeLab.CookBook.Domain.Service
{
    public class StepModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid RecipeId { get; set; }
        public RecipeModel Recipe { get; set; }

        public ICollection<SubStepModel> SubSteps { get; set; }
    }
}
