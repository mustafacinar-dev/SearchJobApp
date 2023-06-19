using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SearchJobApp.Application.Commands;
using SearchJobApp.Application.Helpers;
using SearchJobApp.Application.Interfaces.Helpers;
using SearchJobApp.Application.Middlewares;
using SearchJobApp.Application.Validations;

namespace SearchJobApp.Application;

public static class RegisterServices
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        serviceCollection.AddScoped<IValidator<CreateEmployerCommand>, CreateEmployerCommandValidator>();
        serviceCollection.AddScoped<IValidator<CreatePostCommand>, CreatePostCommandValdiator>();
        serviceCollection.AddScoped<IAuthTokenHelper, AuthTokenHelper>();
    }

    public static void UseMiddlewares(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
        applicationBuilder.UseMiddleware<AuthMiddleware>();
    }
}