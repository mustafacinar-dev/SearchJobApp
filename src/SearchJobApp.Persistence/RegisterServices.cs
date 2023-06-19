using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Persistence.Context;
using SearchJobApp.Persistence.ElasticSearch;
using SearchJobApp.Persistence.Repositories;

namespace SearchJobApp.Persistence;

public static class RegisterServices
{
    public static void AddPersistenceServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<SearchJobAppDbContext>(options =>
            options.UseNpgsql("name=ConnectionStrings:SearchJobAppDbContext"));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        serviceCollection.AddSingleton<ElasticClientProvider>();
        serviceCollection.AddScoped<IEmployerRepository, EmployerRepository>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        serviceCollection.AddScoped<IPostElasticSearchService, PostElasticSearchService>();
        serviceCollection.AddScoped<IEmployerElasticSearchService, EmployerElasticSearchService>();
    }
}