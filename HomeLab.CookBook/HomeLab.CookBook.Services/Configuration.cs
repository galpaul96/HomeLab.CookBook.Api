using HomeLab.CookBook.Domain.Interfaces.Services;
using HomeLab.CookBook.EF;
using HomeLab.CookBook.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HomeLab.CookBook.Services
{
    public static class Configuration
    {
        public static void ConfigureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddScoped<IRecipesService, RecipesService>();
            services.TryAddScoped<IStepsService, StepsService>();
            services.TryAddScoped<ISubStepsService, SubStepsService>();
            services.TryAddScoped<IIngredientsService, IngredientsService>();

            services.ConfigureRepository(configuration);
        }
    }
}
