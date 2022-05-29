namespace HomeLab.CookBook.Domain.Entities
{
    public class Ingredient : Audit
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string Amount { get; set; }
        public string AmountType { get; set; }

        public Guid StepId { get; set; }
        public Step Step { get; set; }
    }
}
