namespace HomeLab.CookBook.Domain.Service
{
    public class SubStepModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid StepId { get; set; }
        public StepModel Step { get; set; }
        public ICollection<IngredientModel> Ingredients { get; set; }
    }
}
