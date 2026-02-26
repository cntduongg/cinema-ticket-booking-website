// Models/DTOs/UserResponseDto.cs
namespace UsersManagement.Api.Models.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string IdentityCard { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
