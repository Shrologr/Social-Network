using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Models
{
    public class RegistrarionViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public System.DateTime Birthday { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "URL must be between 5 and 15 symbols")]
        public string URL { get; set; }
        [Required]
        [StringLength(50, MinimumLength=15, ErrorMessage="Password must be between 15 and 50 symbols")]
        [DataType(DataType.Password)]
        public string User_Password { get; set; }
        [Required]
        [Compare("User_Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}