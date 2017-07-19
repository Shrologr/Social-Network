using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.BLL.DTO
{
    public class PostsDTO
    {
        public int ID { get; set; }
        public int User_ID { get; set; }
        public int Poster_ID { get; set; }
        public string Text { get; set; }
        public System.DateTime Date { get; set; }
        public byte[] Image { get; set; }
    }
}
