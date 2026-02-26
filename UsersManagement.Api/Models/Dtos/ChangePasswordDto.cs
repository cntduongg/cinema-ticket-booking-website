using System.ComponentModel.DataAnnotations;

namespace UsersManagement.Api.Models.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        [MaxLength(100)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string NewPassword { get; set; }

        [Required]
        [MaxLength(100)]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
