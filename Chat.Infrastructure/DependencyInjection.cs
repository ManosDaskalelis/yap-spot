using Chat.Application.Abstractions;
using Chat.Infrastructure.Auth;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    o => o.EnableRetryOnFailure());
            });

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ChatDbContext>());

            //services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, TestCurrentUserService>();

            return services;
        }
    }
}