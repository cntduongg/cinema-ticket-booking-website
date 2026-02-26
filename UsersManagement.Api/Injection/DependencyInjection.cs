using UsersManagement.Api.Repositories;
using UsersManagement.Api.Repositories.Interfaces;
using UsersManagement.Api.Services;
using UsersManagement.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using UsersManagement.Api.Data;
using MovieBooking.Services;
using UsersManagement.Api.Services.IServices;

namespace UsersManagement.Api.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMovieBookingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            
            return services;
        }
    }
}
