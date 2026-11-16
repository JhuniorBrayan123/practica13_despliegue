using Microsoft.Extensions.DependencyInjection;

namespace Lab13_JhuniorGutierrez.Application;

public static class DependencyInjection
{
  
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var useCases = typeof(DependencyInjection).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.EndsWith("UseCase"));

        foreach (var useCase in useCases)
            services.AddScoped(useCase);

        return services;
    }
}
