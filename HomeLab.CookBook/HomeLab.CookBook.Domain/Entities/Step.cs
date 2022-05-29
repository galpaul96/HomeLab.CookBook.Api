using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeLab.CookBook.Domain.Entities
{
    public class Step : Audit
    {
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }

        public Guid InstructionId { get; set; }
        public Instruction Instruction { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
