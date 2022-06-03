using HomeLab.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeLab.CookBook.EF.Configurations
{
    internal class SubStepEntityTypeConfiguration : IEntityTypeConfiguration<SubStep>
    {
        public void Configure(EntityTypeBuilder<SubStep> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasMany(x => x.Ingredients)
                .WithOne(x => x.SubStep)
                .HasForeignKey(x => x.SubStepId);
        }
    }
}
