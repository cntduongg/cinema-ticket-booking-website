using UsersManagement.Api.Models.Dtos;
using UsersManagement.Api.Models.Entities;
using UsersManagement.Api.Repositories.Interfaces;
using UsersManagement.Api.Services.IServices;

namespace MovieBooking.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.ExistsAsync(dto.Account))
                return (false, "Account already exists.");

            var user = new User
            {
                Account = dto.Account,
                Password = dto.Password,
                FullName = dto.FullName,
                DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc),
                Sex = dto.Sex,
                Email = dto.Email,
                IdentityCard = dto.IdentityCard,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = "Customer",
                IsLocked = false
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
            return (true, "Registration successful. Please login.");
        }

        public async Task<(bool Success, User? User, string Message)> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByAccountAsync(dto.Account);
            if (user == null || user.Password != dto.Password)
                return (false, null, "Username/ password is invalid. Please try again!");

            if (user.IsLocked)
                return (false, null, "Account has been locked!");

            return (true, user, "Login successful!");
        }
    }
}
