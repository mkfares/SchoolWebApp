using System.ComponentModel.DataAnnotations;

namespace SchoolWebApp.ViewModels
{
    /// <summary>
    /// Employee view model from employee model and used by employee controller
    /// </summary>
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[StringLength(256)]
        //public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Used to diaplay the list of roles
        public string Roles { get; set; }

        // Returns First name and Last Name in one string
        // This is also called calculated or derived property
        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + ", " + LastName; }
        }
    }
}