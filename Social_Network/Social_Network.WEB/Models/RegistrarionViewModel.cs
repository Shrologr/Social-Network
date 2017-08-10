using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Models
{
    public class RegistrarionViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public System.DateTime Birthday { get; set; }
        [Required]
        public bool Show_birthday { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        [RegularExpression(@"^(\d|[a-z]|[A-Z])*$", ErrorMessage = "Wrong format for URL. Text and numbers only")]
        [StringLength(35, MinimumLength = 5, ErrorMessage = "URL must be between 5 and 35 symbols")]
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