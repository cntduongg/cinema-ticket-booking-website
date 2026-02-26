using UsersManagement.Api.Models.Dtos;
using UsersManagement.Api.Models.Entities;

namespace UsersManagement.Api.Services.IServices
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, User? User, string Message)> LoginAsync(LoginDto dto);

    }
}
