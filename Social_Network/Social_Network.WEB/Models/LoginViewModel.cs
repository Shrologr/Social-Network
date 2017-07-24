using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string User_Password { get; set; }
    }
}