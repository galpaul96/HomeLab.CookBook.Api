using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeLab.CookBook.Domain.Entities
{
    public class SubStep : Audit
    {
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid StepId { get; set; }
        public Step Step { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }

    }
}
