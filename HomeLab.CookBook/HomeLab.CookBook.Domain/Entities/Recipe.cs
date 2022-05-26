namespace HomeLab.CookBook.Domain.Entities
{
    public class Recipe : Audit
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        public ICollection<Instruction> Instructions { get; set; }
    }
}
