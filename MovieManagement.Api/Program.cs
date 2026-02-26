using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MovieManagement.Api.Repositories;
using MovieManagement.Api.Repositories.IRepositories;
using MovieManagement.Api.Services;
using MovieManagement.Api.Services.IServices;
using MovieManagement.Data;
using System;

namespace MovieManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Authentication with Cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                });

            // Add DbContext with PostgreSQL
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Controllers and Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DI for Movie features
            builder.Services.AddMovieDependencies();

            // Thêm cấu hình CORS cho FE
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:7205")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            // Middleware
            app.UseHttpsRedirection();
            app.UseCors(); // Cho phép CORS cho FE
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Controllers
            app.MapControllers();

            app.Run();
        }
    }

    public static class DependencyInjection
    {
        public static IServiceCollection AddMovieDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAdminMovieRepository, AdminMovieRepository>();
            services.AddScoped<IAdminMovieService, AdminMovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();
            return services;
        }
    }
}
