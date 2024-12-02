using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUser, User>()
            .ForMember(
            dest => dest.Password,
            opt => opt.Ignore());

            CreateMap<RequestUpdateUser, User>();

            CreateMap<string, Ingredient>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

            CreateMap<Domain.Enums.DishType, DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

            CreateMap<RequestRecipe, Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

            CreateMap<RequestInstruction, Instruction>();
            
            CreateMap<RequestFilterRecipe, FilterRecipeDto>();
        }
        private void DomainToResponse()
        {
            CreateMap<User, ResponseUserProfile>();
            
            CreateMap<Recipe, ResponseRegisteredRecipe>();
            
            CreateMap<Recipe, ResponseRecipe>()
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Select(r => r.Type)))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Select(r => r.Item)))
            .ForMember(dest => dest.RecipeOwner, opt => opt.MapFrom(source => source.RecipeOwner.Name));
            
            CreateMap<Instruction, ResponseInstruction>();

            CreateMap<Recipe, ResponseShortRecipe>()
            .ForMember(dest => dest.AmountIngredients, config => config.MapFrom(source => source.Ingredients.Count))
            .ForMember(dest => dest.AmountInstructions, config => config.MapFrom(source => source.Instructions.Count))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Select(r => r.Type)))
            .ForMember(dest => dest.RecipeOwner, opt => opt.MapFrom(source => source.RecipeOwner.Name));
        }
    }
}
