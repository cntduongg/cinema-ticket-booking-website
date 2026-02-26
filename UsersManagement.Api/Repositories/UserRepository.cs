using UsersManagement.Api.Repositories.Interfaces;
using UsersManagement.Api.Data;
using UsersManagement.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace UsersManagement.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string account)
            => await _context.Users.AnyAsync(u => u.Account == account);

        public async Task<User?> GetByAccountAsync(string account)
            => await _context.Users.FirstOrDefaultAsync(u => u.Account == account);

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

        public async Task<int> GetTotalCountAsync(string? role = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(u => u.Role == role);
            }
            else
            {
                query = query.Where(u => u.Role == "Customer" || u.Role == "Employee");
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<User>> GetPagedUsersAsync(int skip, int take)
        {
            return await _context.Users
                .Where(u => u.Role == "Customer" || u.Role == "Employee") // Giữ bộ lọc này cho admin
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetPagedUsersByRoleAsync(string role, int skip, int take)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsLocked = true;           // khoá tài khoản
            await _context.SaveChangesAsync();
            return true;
        }

        // ...

    }
}
