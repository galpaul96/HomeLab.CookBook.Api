using HomeLab.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeLab.CookBook.EF.Configurations
{
    internal class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(x => x.Details).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.AmountType).IsRequired();

            builder.HasOne(x => x.SubStep)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.SubStepId);
        }
    }
}
