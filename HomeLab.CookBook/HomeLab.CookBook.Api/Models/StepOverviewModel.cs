using System.ComponentModel.DataAnnotations;

namespace HomeLab.CookBook.API.Models
{
    public class StepOverviewModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        [RegularExpression("^\\d{2}:[0-5][0-9]:[0-5][0-9]$")]
        public string Duration { get; set; }
        
        public int NoOfIngredients { get; set; }
    }
}
