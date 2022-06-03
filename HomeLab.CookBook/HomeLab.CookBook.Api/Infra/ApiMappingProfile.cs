using AutoMapper;
using HomeLab.CookBook.API.Models;
using HomeLab.CookBook.Domain.Service;

namespace HomeLab.CookBook.API.Infra
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<RecipeCreateModel, RecipeModel>();
            CreateMap<RecipeModel, RecipeDetailsModel>();
            CreateMap<RecipeModel, RecipeOverviewModel>()
                .ForMember(x => x.NoOfSteps, opt => opt.MapFrom(from => from.Steps.Count()))
                .ForMember(x => x.Duration, opt => opt.MapFrom(from => TimeSpan.FromTicks(from.Steps.Sum(x => x.Duration.Ticks)).ToString("g")));

            CreateMap<StepModel, StepOverviewModel>()
                .ForMember(x => x.NoOfIngredients, opt => opt.MapFrom(from => from.SubSteps.Count()))
                .ForMember(x => x.Duration, opt => opt.MapFrom(from => from.Duration.ToString("g")));
            CreateMap<StepModel, StepDetailsModel>();
            CreateMap<StepCreateModel, StepModel>()
                .ForMember(x=>x.Duration,opt=>opt.MapFrom(from => TimeSpan.Parse(from.Duration)));

            CreateMap<SubStepModel, SubStepOverviewModel>()
                .ForMember(x => x.NoOfSteps, opt => opt.MapFrom(from => from.Ingredients.Count()))
                .ForMember(x => x.Duration, opt => opt.MapFrom(from => from.Duration.ToString("g")));
            CreateMap<SubStepModel, SubStepDetailsModel>();
            CreateMap<SubStepCreateModel, SubStepModel>()
                .ForMember(x => x.Duration, opt => opt.MapFrom(from => TimeSpan.Parse(from.Duration)));

            CreateMap<IngredientModel, IngredientOverviewModel>()
                .ForMember(x=>x.Amount,opts=>opts.MapFrom(from=>$"{from.Amount} {from.AmountType}"));
            CreateMap<IngredientModel, IngredientDetailsModel>();
            CreateMap<IngredientCreateModel, IngredientModel>();
        }
    }
}
