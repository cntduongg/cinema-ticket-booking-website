using System.Threading.Tasks;
using UsersManagement.Api.Models.Dtos;
using UsersManagement.Api.Models.DTOs;

namespace UsersManagement.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserResponseDto>> GetAllUsersAsync(int currentPage = 1, int size = 10);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<(bool Success, string Message)> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<(bool Success, string Message)> DeleteUserAsync(int id);
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<PagedResult<UserResponseDto>> GetPagedUsersByRoleAsync(string role, int currentPage = 1, int size = 10);
    }
}
