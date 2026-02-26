using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Web.Areas.UserManagement.Models
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "The FullName field is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "The Sex field is required")]
        [Display(Name ="Sex")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The IdentityCard field is required")]
        [Display(Name = "Identity Card")]
        public string IdentityCard { get; set; }

        [Required(ErrorMessage = "The PhoneNumber field is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Address field is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The Role field is required")]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "IsLocked")]
        public bool IsLocked { get; set; }
    }
}
