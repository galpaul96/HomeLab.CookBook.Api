using System.ComponentModel.DataAnnotations;

namespace HomeLab.CookBook.API.Models
{
    public class InstructionCreateModel
    {
        [Required]
        [MinLength(10)]
        [MaxLength(128)]
        public string Description { get; set; }

        [Required]
        [RegularExpression("^\\d{2}:[0-5][0-9]:[0-5][0-9]$")]
        public string Duration { get; set; }

        [Required]
        public Guid RecipeId { get; set; }
    }
}
