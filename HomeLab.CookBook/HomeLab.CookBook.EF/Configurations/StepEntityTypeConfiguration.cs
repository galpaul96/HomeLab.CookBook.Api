using HomeLab.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeLab.CookBook.EF.Configurations
{
    internal class StepEntityTypeConfiguration : IEntityTypeConfiguration<Step>
    {
        public void Configure(EntityTypeBuilder<Step> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            
            builder.HasMany(x => x.SubSteps)
                .WithOne(x => x.Step)
                .HasForeignKey(x => x.StepId);
        }
    }
}
