using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Application.UserCases.Login.DoLogin;
using MyRecipeBook.Application.UserCases.User.ChangePassword;
using MyRecipeBook.Application.UserCases.User.Delete;
using MyRecipeBook.Application.UserCases.User.Delete.DeleteUser;
using MyRecipeBook.Application.UserCases.User.Delete.DeleteUserAccountActive;
using MyRecipeBook.Application.UserCases.User.Profile;
using MyRecipeBook.Application.UserCases.User.Register;
using MyRecipeBook.Domain.Dto;

namespace MyRecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IDeleteUserAccountTimerUseCase, DeleteUserAccountTimerUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
            services.AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>();
            services.AddScoped<IRecipeGetByIdUseCase, RecipeGetByIdUseCase>();
            services.AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>();
            services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
            services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
            
            services.AddScoped(option =>
            new MapperConfiguration(options => { options.AddProfile(new AutoMapping()); }).CreateMapper());
        }
    }
}
