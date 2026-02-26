using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Web.Areas.UserManagement.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Account")]
        public string Account { get; set; }

        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Sex")]
        public string? Sex { get; set; }

        [Display(Name = "Identity Card")]
        public string? IdentityCard { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Is Locked")]
        public bool IsLocked { get; set; }
    }
}
