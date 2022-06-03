namespace HomeLab.CookBook.Domain.Entities
{
    public class Ingredient : Audit
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string Amount { get; set; }
        public string AmountType { get; set; }

        public Guid SubStepId { get; set; }
        public SubStep SubStep { get; set; }
    }
}
