namespace HomeLab.CookBook.API.Models
{
    public class IngredientDetailsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Amount { get; set; }
        public string AmountType { get; set; }

        public StepOverviewModel Step { get; set; }
    }
}
