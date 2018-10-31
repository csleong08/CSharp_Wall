using System;
using System.ComponentModel.DataAnnotations;

namespace Wall.Models
{
    public class Logins
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Passwords must be at least 8 characters")]
        // [Compare("password", ErrorMessage = "The passwords do not match.")]
        public string password { get; set; }
    }
}