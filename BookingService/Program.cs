using BookingService;
using BookingService.Models;
using BookingService.Models.VnPayModels;
using BookingService.Repositories;
using BookingService.Services;
using BookingService.Services.SignalRConfig;
using BookingService.Services.VNPAY;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VNPAY.NET;

var builder = WebApplication.CreateBuilder(args);
// Th�m Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICinemaRoomRepository, CinemaRoomRepository>();
builder.Services.AddScoped<ICinemaRoomService, CinemaRoomService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<IDrinkService, DrinkService>();
builder.Services.AddScoped<IScoreHistoryService, ScoreHistoryService>();
builder.Services.AddScoped<IScoreHistoryRepository, ScoreHistoryRepository>();

//  Th�m DI cho Repository v� Service(Yogurt was hereXD)
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

builder.Services.AddScoped<IComboRepository, ComboRepository>();
builder.Services.AddScoped<IComboService, ComboService>();

builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

builder.Services.AddScoped<BookingService.Repositories.IAppConfigRepository, BookingService.Repositories.AppConfigRepository>();
builder.Services.AddScoped<BookingService.Services.IService.IAppConfigService, BookingService.Services.AppConfigService>();

builder.Services.AddHttpClient("MovieService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7214");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFE", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5073", // FE cũ
            "https://localhost:7205" // FE hiện tại
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
builder.Services.AddHttpClient<IUserService, UserService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7041/");
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IComboDetailRepository, ComboDetailRepository>();
builder.Services.AddScoped<IComboDetailService, ComboDetailService>();
builder.Services.AddScoped<ISeatScheduleRepository, SeatScheduleRepository>();
builder.Services.AddScoped<ISeatScheduleService, SeatScheduleService>();
builder.Services.AddSignalR()
    .AddHubOptions<SeatHub>(options =>
    {
        options.EnableDetailedErrors = true;
    });
builder.Services.AddHostedService<ExpireHeldSeatsJob>();
// cấu hình API VNPAY
builder.Services.Configure<VnpayOptions>(builder.Configuration.GetSection("Vnpay"));
builder.Services.AddSingleton<IVnpay, Vnpay>();
builder.Services.AddScoped<IVnpayPaymentService, VnpayPaymentService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Kh?i t?o Swagger JSON API
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // ???ng d?n Swagger
        c.RoutePrefix = string.Empty; // C?u h�nh Swagger UI ? ???ng d?n g?c
    });

}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowFE");
app.MapControllers();
app.MapHub<SeatHub>("/seathub").RequireCors("AllowFE");

app.Run();

