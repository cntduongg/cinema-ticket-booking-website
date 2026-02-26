using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Web.Areas.UserManagement.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Account")]
        public string Account { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
