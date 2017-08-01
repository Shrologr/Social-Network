using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Models
{
    public class PostListItemViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string PosterFullName { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string URL { get; set; }
        public string Authenticated_URL { get; set; }
        public int Likes { get; set; }
    }
}