using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social_Network.DAL.EF;

namespace Social_Network.DAL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<NetworkUsers> NetworkUsers { get; }
        IRepository<Posts> Posts { get; }
        IRepository<UserPhotos> UserPhotos { get; }
        void Save();
    }
}
