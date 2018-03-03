using SchoolWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebApp.ViewModels
{
    /// <summary>
    /// Fcaulty view model from the faculty model and used by faculty controller
    /// The types of the properties should be the same as faculty model
    /// Especialy nullable and non-nullable properties
    /// </summary>
    public class FacultyViewModel
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

        public string Speciality { get; set; }

        public FacultyLevel? Level { get; set; }

        // Used to display the list of roles
        public string Roles { get; set; }
    }
}