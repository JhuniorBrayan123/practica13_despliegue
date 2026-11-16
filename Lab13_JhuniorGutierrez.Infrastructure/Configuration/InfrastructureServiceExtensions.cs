using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;
using Lab13_JhuniorGutierrez.Infrastructure.Data;
using Lab13_JhuniorGutierrez.Infrastructure.Repositories;
using Lab13_JhuniorGutierrez.Infrastructure.Security;
using Lab13_JhuniorGutierrez.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab13_JhuniorGutierrez.Infrastructure.Configuration;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured (use user-secrets or appsettings).");

        services.AddDbContext<Lab13_JhuniorGutierrezDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IExcelReportGenerator, ClosedXmlReportGenerator>();

        return services;
    }
}
