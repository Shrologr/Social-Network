using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="Please, enter e-mail")]
        [EmailAddress(ErrorMessage="Wrong format for e-mail")]
        public string Mail { get; set; }
        [Required(ErrorMessage="Please, enter password")]
        [DataType(DataType.Password)]
        public string User_Password { get; set; }
    }
}