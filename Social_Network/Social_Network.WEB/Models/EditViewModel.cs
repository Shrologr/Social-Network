using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Models
{
    public class EditViewModel
    {
        public int ID { get; set; }
        public int? Photo_ID { get; set; }
        [Required(ErrorMessage = "Please, enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please, enter your surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Specify birthday (for example, 1994-11-20)")]
        public System.DateTime Birthday { get; set; }
        [Required]
        public bool Show_birthday { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please, enter e-mail")]
        [EmailAddress(ErrorMessage = "Wrong format for e-mail")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Enter URL, please")]
        [RegularExpression(@"^(\d|[a-z]|[A-Z])*$", ErrorMessage = "Wrong format for URL. Text and numbers only")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "URL must be between 5 and 60 symbols")]
        public string URL { get; set; }
        [Required(ErrorMessage = "Please, enter password")]
        [StringLength(60, MinimumLength = 15, ErrorMessage = "Password must be between 15 and 60 symbols")]
        [DataType(DataType.Password)]
        public string User_Password { get; set; }
        [Required(ErrorMessage = "Confirm the password")]
        [Compare("User_Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}