using HomeLab.CookBook.Domain.Interfaces.Repositories;
using HomeLab.CookBook.EF.Infra;
using HomeLab.CookBook.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HomeLab.CookBook.EF
{
    public static class Configuration
    {
        public static void ConfigureRepository(this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.TryAddScoped<IRecipeRepository, RecipeRepository>();
            services.TryAddScoped<IRepository, Repository>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CookBookContext>(
                options =>
                    options.UseNpgsql(
                        connectionString,
                        x => x.MigrationsAssembly("HomeLab.CookBook.EF")));
            
        }
    }
}
