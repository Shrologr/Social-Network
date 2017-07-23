//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Social_Network.DAL.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class NetworkUsers
    {
        public NetworkUsers()
        {
            this.Posts = new HashSet<Posts>();
            this.PostsPosterID = new HashSet<Posts>();
            this.PostsUserID = new HashSet<Posts>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public System.DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public bool Show_birthday { get; set; }
        public string Mail { get; set; }
        public string URL { get; set; }
        public Nullable<int> Photo_ID { get; set; }
        public string User_Password { get; set; }
    
        public virtual ICollection<Posts> Posts { get; set; }
        public virtual ICollection<Posts> PostsPosterID { get; set; }
        public virtual ICollection<Posts> PostsUserID { get; set; }
        public virtual UserPhotos UserPhotos { get; set; }
    }
}
