using System.ComponentModel.DataAnnotations;

namespace HomeLab.CookBook.API.Models
{
    public class IngredientCreateModel
    {
        [Required]
        public Guid StepId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        
        [Required]
        [MinLength(5)]
        [MaxLength(128)]
        public string Details { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [MaxLength(10)]
        public string AmountType { get; set; }
    }
}
