using System.ComponentModel.DataAnnotations;

namespace HomeLab.CookBook.API.Models
{
    public class RecipeCreateModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(128)]
        public string Description { get; set; }

        [Required]
        [MaxLength(10)]
        public string Difficulty { get; set; }
    }
}
