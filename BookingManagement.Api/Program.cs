using BookingManagement.Api.Data;
using BookingManagement.Api.Repositories;
using BookingManagement.Api.Repositories.IRepository;
using BookingManagement.Api.Services;
using BookingManagement.Api.Services.IService;
using BookingManagement.Api.Services.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using BookingManagement.Api.Models.Config; // THÊM DÒNG NÀY

var builder = WebApplication.CreateBuilder(args);

// Authentication with Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

// Add DbContext and PostgreSQL provider
builder.Services.AddDbContext<ShowtimeManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add HttpClient for calling Movie API
builder.Services.AddHttpClient("MovieApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalServices:MovieApiBaseUrl"] ?? "https://localhost:7001");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add general HttpClientFactory
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure SeatPricing - THÊM DÒNG NÀY
builder.Services.Configure<SeatPricingConfig>(
    builder.Configuration.GetSection("SeatPricing"));
// Add DI for all services
builder.Services.AddServiceDependencies();

//builder.Services.AddScoped<ITicketRepository, TicketRepository>();
//builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddHttpClient<ITicketService, TicketService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

var app = builder.Build();

// ===== THÊM SEED DATA Ở ĐÂY =====
//using (var scope = app.Services.CreateScope())
//{
//    try
//    {
//        var context = scope.ServiceProvider.GetRequiredService<ShowtimeManagementDbContext>();
//        await BookingManagement.Api.Data.SeedData.Initialize(context);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"❌ Lỗi seed data: {ex.Message}");
//        Console.WriteLine(ex.StackTrace);
//    }
//}
// ===== KẾT THÚC SEED DATA =====
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();

public static class DependencyInjection
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        // Repository registrations
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
        services.AddScoped<IShowtimeSeatRepository, ShowtimeSeatRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketSeatRepository, TicketSeatRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        // Service registrations
        services.AddScoped<ISeatService, SeatService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IShowtimeService, ShowtimeService>();
        services.AddScoped<IShowtimeSeatService, ShowtimeSeatService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ITicketSeatService, TicketSeatService>();
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }
}
