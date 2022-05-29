using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeLab.CookBook.Domain.Entities
{
    public class Instruction : Audit
    {
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public ICollection<Step> Steps { get; set; }

    }
}
