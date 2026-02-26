using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    public DbSet<CinemaRoom> CinemaRooms { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Combo> Combos { get; set; }
    public DbSet<ComboDetail> ComboDetails { get; set; }
    public DbSet<SeatSchedule> SeatSchedules { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<ScoreHistory> ScoreHistories { get; set; }
    public DbSet<AppConfig> AppConfig { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=BookingService;Username=postgres;Password=12345");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SeatSchedule>()
            .HasKey(ss => new { ss.SeatId, ss.ScheduleId });

        modelBuilder.Entity<ScoreHistory>()
       .HasOne(sh => sh.Order)
       .WithMany() // hoặc WithMany(o => o.ScoreHistories) nếu bạn thêm navigation property bên Order
       .HasForeignKey(sh => sh.OrderId);

        modelBuilder.Entity<SeatSchedule>()
            .Property(ss => ss.Status)
            .HasConversion<string>();

        modelBuilder.Entity<SeatSchedule>()
            .HasOne(ss => ss.Seat)
            .WithMany()
            .HasForeignKey(ss => ss.SeatId);

        modelBuilder.Entity<SeatSchedule>()
            .HasOne(ss => ss.Schedule)
            .WithMany()
            .HasForeignKey(ss => ss.ScheduleId);

        // Seed data for AppConfig
        modelBuilder.Entity<AppConfig>().HasData(
            new AppConfig { Id = 1, ConfigKey = "HOLD_SEAT_TIMEOUT", ConfigValue = "20" },
            new AppConfig { Id = 2, ConfigKey = "RELEASE_SEAT_TIMEOUT", ConfigValue = "10" }
        );
    }
}