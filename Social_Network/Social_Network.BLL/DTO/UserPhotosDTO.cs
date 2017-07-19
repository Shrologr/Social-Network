using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.BLL.DTO
{
    public class UserPhotosDTO
    {
        public int ID { get; set; }
        public byte[] Image { get; set; }
        public int User_ID { get; set; }
    }
}
