using Microsoft.EntityFrameworkCore;
using UsersManagement.Api.Models.Entities;

namespace UsersManagement.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Account = "admin",
                    Password = "admin123",
                    FullName = "Admin User",
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1990, 1, 1), DateTimeKind.Utc),
                    Sex = "Nam",
                    Email = "admin@example.com",
                    IdentityCard = "123456789",
                    PhoneNumber = "0123456789",
                    Address = "Admin Address",
                    Role = "Admin",
                    IsLocked = false
                },
                new User
                {
                    Id = 2,
                    Account = "employee",
                    Password = "employee123",
                    FullName = "Employee User",
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1995, 5, 5), DateTimeKind.Utc),
                    Sex = "Nữ",
                    Email = "employee@example.com",
                    IdentityCard = "987654321",
                    PhoneNumber = "0987654321",
                    Address = "Employee Address",
                    Role = "Employee",
                    IsLocked = false
                },
                new User
                {
                    Id = 3,
                    Account = "customer",
                    Password = "customer123",
                    FullName = "Customer User",
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(2000, 10, 10), DateTimeKind.Utc),
                    Sex = "Nam",
                    Email = "customer@example.com",
                    IdentityCard = "111222333",
                    PhoneNumber = "0111222333",
                    Address = "Customer Address",
                    Role = "Customer",
                    IsLocked = false
                }
            );
        }
    }
} 