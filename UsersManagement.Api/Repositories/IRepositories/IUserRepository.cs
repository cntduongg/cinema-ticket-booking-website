using UsersManagement.Api.Models.Entities;

namespace UsersManagement.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string account);
        Task<User?> GetByAccountAsync(string account);
        Task AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<int> GetTotalCountAsync(string? role = null);
        Task<IEnumerable<User>> GetPagedUsersAsync(int skip, int take);
        Task SaveAsync();
        // ... các method khác nếu cần
        // xóa user theo id, trả về false nếu không tìm thấy
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<User>> GetPagedUsersByRoleAsync(string role, int skip, int take);
    }
}
