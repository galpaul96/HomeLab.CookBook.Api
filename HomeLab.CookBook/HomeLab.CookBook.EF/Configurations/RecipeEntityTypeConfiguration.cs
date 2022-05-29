using HomeLab.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeLab.CookBook.EF.Configurations
{
    internal class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(x => x.Title).HasMaxLength(25).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Difficulty).IsRequired();
            
            builder.HasMany(x => x.Instructions)
                .WithOne(x => x.Recipe)
                .HasForeignKey(x => x.RecipeId);
        }
    }
}
