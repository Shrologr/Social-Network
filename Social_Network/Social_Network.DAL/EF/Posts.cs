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
    
    public partial class Posts
    {
        public Posts()
        {
            this.NetworkUsers = new HashSet<NetworkUsers>();
        }
    
        public int ID { get; set; }
        public int User_ID { get; set; }
        public int Poster_ID { get; set; }
        public string Text { get; set; }
        public System.DateTime Date { get; set; }
        public byte[] Image { get; set; }
    
        public virtual ICollection<NetworkUsers> NetworkUsers { get; set; }
        public virtual NetworkUsers NetworkUsersPosterID { get; set; }
        public virtual NetworkUsers NetworkUsersUserID { get; set; }
    }
}
