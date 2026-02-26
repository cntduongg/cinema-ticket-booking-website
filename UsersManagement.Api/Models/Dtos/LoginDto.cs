using System.ComponentModel.DataAnnotations;

namespace UsersManagement.Api.Models.Dtos
{
    public class LoginDto
    {
        [Required]
        [MaxLength(50)]
        public string Account { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}