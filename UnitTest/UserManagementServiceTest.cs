using Xunit;
using Moq;
using FluentAssertions;
using UsersManagement.Api.Models.Dtos;
using UsersManagement.Api.Models.Entities;
using UsersManagement.Api.Repositories.Interfaces;
using UsersManagement.Api.Services;
using MovieBooking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace UnitTest.Spring1
{
    using SeverityLevel = Allure.Net.Commons.SeverityLevel;

    /// <summary>
    /// Unit tests for User Management Services using mocked IUserRepository
    /// </summary>
    [AllureSuite("User Management Service Unit Tests")]
    public class UserManagementServiceTest
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public UserManagementServiceTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _authService = new AuthService(_userRepoMock.Object);
            _userService = new UserService(_userRepoMock.Object);
        }

        // -------------------------------
        // AuthService - Register
        // -------------------------------

        [Fact]
        [AllureFeature("Authentication")]
        [AllureStory("User Registration")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "auth", "register")]
        public async Task RegisterAsync_ShouldFail_WhenAccountExists()
        {
            // Arrange
            var dto = new RegisterDto { Account = "existingUser" };
            _userRepoMock.Setup(r => r.ExistsAsync(dto.Account)).ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Account already exists.");
        }

        [Fact]
        [AllureFeature("Authentication")]
        [AllureStory("User Registration")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "auth", "register")]
        public async Task RegisterAsync_ShouldSucceed_WhenAccountIsUnique()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Account = "newuser",
                Password = "123456",
                FullName = "John Doe",
                DateOfBirth = DateTime.UtcNow,
                Email = "john@example.com",
                IdentityCard = "123456789",
                PhoneNumber = "0123456789",
                Address = "123 Main St",
                Sex = "Male"
            };

            _userRepoMock.Setup(r => r.ExistsAsync(dto.Account)).ReturnsAsync(false);
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Registration successful. Please login.");
        }

        // -------------------------------
        // AuthService - Login
        // -------------------------------

        [Fact]
        [AllureFeature("Authentication")]
        [AllureStory("User Login")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "auth", "login")]
        public async Task LoginAsync_ShouldFail_WhenUserNotFound()
        {
            // Arrange
            var dto = new LoginDto { Account = "notfound", Password = "123" };
            _userRepoMock.Setup(r => r.GetByAccountAsync(dto.Account)).ReturnsAsync((User)null!);

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Username/ password is invalid. Please try again!");
            result.User.Should().BeNull();
        }

        [Fact]
        [AllureFeature("Authentication")]
        [AllureStory("User Login")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "auth", "login")]
        public async Task LoginAsync_ShouldFail_WhenPasswordIncorrect()
        {
            // Arrange
            var dto = new LoginDto { Account = "user", Password = "wrong" };
            var user = new User { Account = "user", Password = "correct" };

            _userRepoMock.Setup(r => r.GetByAccountAsync(dto.Account)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Username/ password is invalid. Please try again!");
        }

        [Fact]
        [AllureFeature("Authentication")]
        [AllureStory("User Login")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "auth", "login")]
        public async Task LoginAsync_ShouldFail_WhenAccountLocked()
        {
            // Arrange
            var dto = new LoginDto { Account = "user", Password = "123" };
            var user = new User { Account = "user", Password = "123", IsLocked = true };

            _userRepoMock.Setup(r => r.GetByAccountAsync(dto.Account)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Account has been locked!");
        }

        [Fact]
        [AllureFeature("Authentication")]
        [AllureStory("User Login")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "auth", "login")]
        public async Task LoginAsync_ShouldSucceed_WhenValid()
        {
            // Arrange
            var dto = new LoginDto { Account = "user", Password = "123" };
            var user = new User { Account = "user", Password = "123", IsLocked = false };

            _userRepoMock.Setup(r => r.GetByAccountAsync(dto.Account)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Login successful!");
            result.User.Should().NotBeNull();
        }

        // -------------------------------
        // UserService - Change Password
        // -------------------------------

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Change Password")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "user", "password")]
        public async Task ChangePasswordAsync_ShouldFail_WhenUserNotFound()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null!);

            // Act
            var result = await _userService.ChangePasswordAsync(1, "old", "new");

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found.");
        }

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Change Password")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "user", "password")]
        public async Task ChangePasswordAsync_ShouldFail_WhenOldPasswordWrong()
        {
            // Arrange
            var user = new User { Id = 1, Password = "correct" };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userService.ChangePasswordAsync(1, "wrong", "new");

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Old password is incorrect.");
        }

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Change Password")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "user", "password")]
        public async Task ChangePasswordAsync_ShouldSucceed_WhenValid()
        {
            // Arrange
            var user = new User { Id = 1, Password = "oldpass" };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.ChangePasswordAsync(1, "oldpass", "newpass");

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Password changed successfully.");
        }

        // -------------------------------
        // UserService - Update User
        // -------------------------------

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Update User")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "user", "update")]
        public async Task UpdateUserAsync_ShouldFail_WhenUserNotFound()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null!);

            var dto = new UpdateUserDto();
            
            // Act
            var result = await _userService.UpdateUserAsync(1, dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found!");
        }

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Update User")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "user", "update")]
        public async Task UpdateUserAsync_ShouldSucceed_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, FullName = "Old" };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

            var dto = new UpdateUserDto
            {
                FullName = "Updated Name",
                DateOfBirth = DateTime.UtcNow,
                Sex = "Male",
                Email = "user@example.com",
                IdentityCard = "123456",
                PhoneNumber = "0123456789",
                Address = "New Address",
                Role = "Customer",
                IsLocked = false
            };

            // Act
            var result = await _userService.UpdateUserAsync(1, dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("User updated successfully.");
            user.FullName.Should().Be("Updated Name"); // ensure mapping happened
        }

        // -------------------------------
        // UserService - Delete User
        // -------------------------------

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Delete User")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "user", "delete")]
        public async Task DeleteUserAsync_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            _userRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _userService.DeleteUserAsync(1);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found.");
        }

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Delete User")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "user", "delete")]
        public async Task DeleteUserAsync_ShouldReturnSuccess_WhenUserDeleted()
        {
            // Arrange
            _userRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteUserAsync(1);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("User has been locked successfully.");
        }

        // -------------------------------
        // UserService - Get All Users
        // -------------------------------

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Get All Users")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "user", "list")]
        public async Task GetAllUsersAsync_ShouldReturnEmpty_WhenNoUsersExist()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        [AllureFeature("User Management")]
        [AllureStory("Get All Users")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "user", "list")]
        public async Task GetAllUsersAsync_ShouldReturnUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Account = "user1", FullName = "User One" },
                new User { Id = 2, Account = "user2", FullName = "User Two" }
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.First().Account.Should().Be("user1");
        }
    }
}
