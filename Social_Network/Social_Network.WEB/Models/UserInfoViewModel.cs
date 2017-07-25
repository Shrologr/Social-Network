using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Models
{
    public class UserInfoViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public System.DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public bool Show_birthday { get; set; }
        public string Mail { get; set; }
        public string URL { get; set; }
        public int? Photo_ID { get; set; }
    }
}