namespace UsersManagement.Api.Models.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateUserDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string IdentityCard { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string Role { get; set; } 
        public bool IsLocked { get; set; }
    }
}
