namespace HomeLab.CookBook.Domain.Service
{
    public class IngredientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Amount { get; set; }
        public string AmountType { get; set; }

        public Guid SubStepId { get; set; }
        public SubStepModel SubStep { get; set; }
    }
}
