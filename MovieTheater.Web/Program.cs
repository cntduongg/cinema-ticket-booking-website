using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieTheater.Web.Areas.MovieManagement.Services;
using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Service;
using MovieTheater.Web.Areas.Booking.Services;
using MovieThreatUI.Areas.Booking.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFE", policy =>
    {
        policy
            .WithOrigins("https://localhost:7205") // Domain FE
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Bắt buộc cho SignalR
    });
});

// Đăng ký các service Movie và Booking
builder.Services.AddScoped<IMovieService, MovieService>();

// Đăng ký HttpClientFactory trước (quan trọng!)
builder.Services.AddHttpClient();

// Đăng ký các service bổ sung ở khu vực Booking
builder.Services.AddScoped<ICinemaRoomService, CinemaRoomService>();
builder.Services.AddScoped<IScheduleService,  ScheduleService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();

// Xác thực bằng cookie (sửa lại để tương thích với code cũ)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "UserLoginCookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.LoginPath = "/UserManagement/Account/Login";
    options.AccessDeniedPath = "/UserManagement/Account/AccessDenied";
});

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
    options.AddPolicy("CustomerOrAdmin", policy => policy.RequireRole("Admin", "Customer"));
});

// Cấu hình các HttpClient
builder.Services.AddHttpClient("UserApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5178/api/");
});
builder.Services.AddHttpClient("MovieApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5292/api/");
});
builder.Services.AddHttpClient("BookingApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7092/api/");
});

// Cấu hình HttpClient cho MovieService
builder.Services.AddHttpClient<IMovieService, MovieService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MovieApi:BaseUrl"] ?? "http://localhost:5292/api/");
});

// Xóa các HttpClient riêng cho service vì đã dùng IHttpClientFactory

// Đăng ký SignalR (nếu cần)
builder.Services.AddSignalR();

// Đăng ký các service khác trong project (UserManagement, DBContext, v.v.)

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFE");
app.UseAuthentication();
app.UseAuthorization();

// Cấu hình các route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "root",
    pattern: "",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "userManagement",
    pattern: "UserManagement/{controller=Account}/{action=Index}/{id?}",
    defaults: new { area = "UserManagement" });

app.MapControllerRoute(
    name: "movieManagement",
    pattern: "MovieManagement/{controller=Movie}/{action=Index}/{id?}",
    defaults: new { area = "MovieManagement" });

app.MapControllerRoute(
    name: "booking",
    pattern: "Booking/{controller=CinemaRoom}/{action=Index}/{id?}",
    defaults: new { area = "Booking" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Nếu có SignalR Hub
// app.MapHub<SeatHub>("/seathub").RequireCors("AllowFE");

app.MapControllers();

app.Run();