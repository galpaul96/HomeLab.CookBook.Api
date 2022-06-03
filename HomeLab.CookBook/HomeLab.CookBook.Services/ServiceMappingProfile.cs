using AutoMapper;
using HomeLab.CookBook.Domain.Entities;
using HomeLab.CookBook.Domain.Service;

namespace HomeLab.CookBook.Services
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<Recipe, RecipeModel>();
            CreateMap<RecipeModel, Recipe>();

            CreateMap<StepModel, Step>();
            CreateMap<Step, StepModel>();

            CreateMap<SubStepModel, SubStep>();
            CreateMap<SubStep, SubStepModel>();

            CreateMap<IngredientModel, Ingredient>();
            CreateMap<Ingredient, IngredientModel>();
        }
    }
}
