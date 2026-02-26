using Microsoft.EntityFrameworkCore;
using UsersManagement.Api.Models.Dtos;
using UsersManagement.Api.Models.DTOs;
using UsersManagement.Api.Repositories.Interfaces;
using UsersManagement.Api.Services.Interfaces;

namespace UsersManagement.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //get list users (paged)
        public async Task<PagedResult<UserResponseDto>> GetAllUsersAsync(int currentPage = 1, int size = 10)
        {
            if (currentPage < 1) currentPage = 1;
            if (size <= 0) size = 10;

            int total = await _userRepository.GetTotalCountAsync();
            int totalPage = (int)Math.Ceiling((double)total / size);

            var users = await _userRepository.GetPagedUsersAsync((currentPage - 1) * size, size);

            var userDtos = users.OrderBy(user => user.Id).Select(user => new UserResponseDto
            {
                Id = user.Id,
                Account = user.Account,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Sex = user.Sex,
                Email = user.Email,
                IdentityCard = user.IdentityCard,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role,
                IsLocked = user.IsLocked,
                // Add Password property to UserResponseDto if not present
                // Password = user.Password
            }).ToList();

            return new PagedResult<UserResponseDto>
            {
                Data = userDtos,
                Total = total,
                Size = size,
                CurrentPage = currentPage,
                TotalPage = totalPage,
                Message = userDtos.Any() ? "success" : "no data"
            };
        }

        public async Task<PagedResult<UserResponseDto>> GetPagedUsersByRoleAsync(string role, int currentPage = 1, int size = 10)
        {
            if (currentPage < 1) currentPage = 1;
            if (size <= 0) size = 10;

            int total = await _userRepository.GetTotalCountAsync(role);
            int totalPage = (int)Math.Ceiling((double)total / size);

            var users = await _userRepository.GetPagedUsersByRoleAsync(role, (currentPage - 1) * size, size);

            var userDtos = users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                Account = user.Account,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Sex = user.Sex,
                Email = user.Email,
                IdentityCard = user.IdentityCard,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role,
                IsLocked = user.IsLocked
            }).ToList();

            return new PagedResult<UserResponseDto>
            {
                Data = userDtos,
                Total = total,
                Size = size,
                CurrentPage = currentPage,
                TotalPage = totalPage,
                Message = userDtos.Any() ? "success" : "no data"
            };
        }

        //get all users (no paging)
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                Account = user.Account,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Sex = user.Sex,
                Email = user.Email,
                IdentityCard = user.IdentityCard,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role,
                IsLocked = user.IsLocked
            }).ToList();
        }

        //change password
        public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            if (user.Password != oldPassword)
                return (false, "Old password is incorrect.");

            user.Password = newPassword;
            await _userRepository.SaveAsync();
            return (true, "Password changed successfully.");
        }

        //update user information
        public async Task<(bool Success, string Message)> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return (false, "User not found!");

            user.FullName = dto.FullName;
            user.DateOfBirth = dto.DateOfBirth;
            user.Sex = dto.Sex;
            user.Email = dto.Email;
            user.IdentityCard = dto.IdentityCard;
            user.PhoneNumber = dto.PhoneNumber;
            user.Address = dto.Address;
            user.Role = dto.Role;
            user.IsLocked = dto.IsLocked;

            await _userRepository.SaveAsync();
            return (true, "User updated successfully.");
        }
        //delete user
        public async Task<(bool Success, string Message)> DeleteUserAsync(int id)
        {
            var deleted = await _userRepository.DeleteAsync(id);
            if (!deleted) return (false, "User not found.");
            return (true, "User has been locked successfully.");
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null; // Hoặc ném ngoại lệ, tùy vào cách xử lý lỗi mong muốn
            }
            return new UserResponseDto
            {
                Id = user.Id,
                Account = user.Account,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Sex = user.Sex,
                Email = user.Email,
                IdentityCard = user.IdentityCard,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role,
                IsLocked = user.IsLocked
            };
        }
    }
}
