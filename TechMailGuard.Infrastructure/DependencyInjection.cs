using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechMailGuard.Domain.Interfaces;
using TechMailGuard.Infrastructure.Persistence;
using TechMailGuard.Infrastructure.Persistence.Repositories;

namespace TechMailGuard.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<TechMailGuardDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TechMailGuardDbContext>());

        services.AddScoped<IMailboxRepository, MailboxRepository>();

        return services;
    }
}
