using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.BLL.DTO
{
    public class NetworkUsersDTO
    {    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public System.DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public bool Show_birthday { get; set; }
        public string Mail { get; set; }
        public string URL { get; set; }
        public int? Photo_ID { get; set; }
        public string User_Password { get; set; }
    }
}
