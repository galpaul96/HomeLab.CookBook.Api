using System.Reflection;
using HomeLab.CookBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeLab.CookBook.EF.Infra
{
    internal class CookBookContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public CookBookContext(DbContextOptions<CookBookContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(CookBookContext)));
        }
    }

    internal class CookBookContextFactory : IDesignTimeDbContextFactory<CookBookContext>
    {
        private const string ApplicationName = "Homelab.Api.CookBook";

        public CookBookContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CookBookContext>();
            string connectionString = Environment.GetEnvironmentVariable("DbConnectionString")!;

            //string connectionString = "Server=192.168.1.206;Port=5432;Database=CookBook.Dev;Userid=postgres;Password=TUPms4k@Homelab;";
            string applicationConnectionString = $"Application Name={ApplicationName};{connectionString}";

            optionsBuilder.UseNpgsql(applicationConnectionString, x =>
            {
                x.EnableRetryOnFailure();
            });

            return new CookBookContext(optionsBuilder.Options);
        }
    }
}
