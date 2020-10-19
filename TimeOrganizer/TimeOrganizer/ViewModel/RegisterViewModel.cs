using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeOrganizer.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only pleease")]
        [MaxLength(20, ErrorMessage = "First name cannot be larger then 20 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only pleease")]
        [MaxLength(20, ErrorMessage = "Last name cannot be larger then 20 characters")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select your school")]
        public int SchoolId { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password do not match.")]
        public string RepeatPassword { get; set; }
    }
}
