using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Social_Network.DAL.EF;
using Social_Network.DAL.Interfaces;

namespace Social_Network.DAL.Implementation
{
    public class EFUnitOfWork:IUnitofWork
    {
        private SocialNetworkDatabaseContext db;
        private NetworkUsersRepository networkUsersRepository;
        private PostsRepository postsRepository;
        private UserPhotosRepository userPhotosRepository;
        public EFUnitOfWork(string connectionString)
        {
            db = new SocialNetworkDatabaseContext(connectionString);
        }

        public IRepository<NetworkUsers> NetworkUsers
        {
            get 
            {
                if (networkUsersRepository == null)
                    networkUsersRepository = new NetworkUsersRepository(db);
                return networkUsersRepository;
            }
        }

        public IRepository<Posts> Posts
        {
            get
            {
                if (postsRepository == null)
                    postsRepository = new PostsRepository(db);
                return postsRepository;
            }
        }

        public IRepository<UserPhotos> UserPhotos
        {
            get
            {
                if (userPhotosRepository == null)
                    userPhotosRepository = new UserPhotosRepository(db);
                return userPhotosRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
