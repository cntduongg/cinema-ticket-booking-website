using System;
using Microsoft.EntityFrameworkCore;
using BookingManagement.Api.Models.Entities;

namespace BookingManagement.Api.Data
{
    public class ShowtimeManagementDbContext : DbContext
    {
        public ShowtimeManagementDbContext(DbContextOptions<ShowtimeManagementDbContext> options)
            : base(options)
        {
        }

        // DbSet cho các entities
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<ShowtimeSeat> ShowtimeSeats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketSeat> TicketSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureRelationships(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // ShowtimeSeat - Composite Primary Key
            modelBuilder.Entity<ShowtimeSeat>()
                .HasKey(ss => new { ss.ShowtimeId, ss.SeatId });

            // Room -> Cinema
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Cinema)
                .WithMany(c => c.Rooms)
                .HasForeignKey(r => r.CinemaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seat -> Room
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Seats)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Showtime -> Room
            modelBuilder.Entity<Showtime>()
                .HasOne(st => st.Room)
                .WithMany(r => r.Showtimes)
                .HasForeignKey(st => st.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // ShowtimeSeat -> Showtime
            modelBuilder.Entity<ShowtimeSeat>()
                .HasOne(ss => ss.Showtime)
                .WithMany(st => st.ShowtimeSeats)
                .HasForeignKey(ss => ss.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShowtimeSeat -> Seat
            modelBuilder.Entity<ShowtimeSeat>()
                .HasOne(ss => ss.Seat)
                .WithMany(s => s.ShowtimeSeats)
                .HasForeignKey(ss => ss.SeatId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ticket -> Showtime
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Showtime)
                .WithMany(st => st.Tickets)
                .HasForeignKey(t => t.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade);

            // TicketSeat -> Ticket
            modelBuilder.Entity<TicketSeat>()
                .HasOne(ts => ts.Ticket)
                .WithMany(t => t.TicketSeats)
                .HasForeignKey(ts => ts.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // TicketSeat -> Seat
            modelBuilder.Entity<TicketSeat>()
                .HasOne(ts => ts.Seat)
                .WithMany(s => s.TicketSeats)
                .HasForeignKey(ts => ts.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình DateTime cho PostgreSQL
            modelBuilder.Entity<Showtime>(entity =>
            {
                entity.Property(s => s.ShowDate).HasColumnType("date");
                entity.Property(s => s.StartTime).HasColumnType("time");
                entity.Property(s => s.EndTime).HasColumnType("time");
            });

            modelBuilder.Entity<Ticket>()
                .Property(t => t.BookingTime)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<ShowtimeSeat>()
                .Property(ss => ss.ReservedAt)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<ShowtimeSeat>()
                .Property(ss => ss.ExpiresAt)
                .HasColumnType("timestamp without time zone");

            // Indexes cho performance
            modelBuilder.Entity<ShowtimeSeat>()
                .HasIndex(ss => new { ss.ShowtimeId, ss.Status })
                .HasDatabaseName("IX_ShowtimeSeat_Showtime_Status");

            modelBuilder.Entity<ShowtimeSeat>()
                .HasIndex(ss => ss.ExpiresAt)
                .HasDatabaseName("IX_ShowtimeSeat_ExpiresAt");

            modelBuilder.Entity<ShowtimeSeat>()
                .HasIndex(ss => ss.SessionId)
                .HasDatabaseName("IX_ShowtimeSeat_SessionId");

        }
    }
}
