using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Social_Network.DAL.EF;
using Social_Network.DAL.Interfaces;
using System.Linq.Expressions;

namespace Social_Network.DAL.Implementation
{
    public class UserPhotosRepository:IRepository<UserPhotos>
    {
        private SocialNetworkDatabaseContext db;

        public IEnumerable<UserPhotos> GetAll()
        {
            return db.UserPhotos;
        }
        public UserPhotosRepository(SocialNetworkDatabaseContext context)
        {
            this.db = context;
        }
        public UserPhotos Get(int id)
        {
            return db.UserPhotos.Find(id);
        }

        public IEnumerable<UserPhotos> Find(Expression<Func<UserPhotos, bool>> predicate)
        {
            return db.UserPhotos.Where(predicate).ToList();
        }

        public void Create(UserPhotos item)
        {
            db.UserPhotos.Add(item);
        }

        public void Update(UserPhotos item)
        {
            var oldPhoto = Get(item.ID);
            db.Entry(oldPhoto).State = EntityState.Detached;
            db.UserPhotos.Attach(item);
            db.Entry(item).State = EntityState.Modified;

        }

        public void Delete(int id)
        {
            UserPhotos photo = db.UserPhotos.Find(id);
            if (photo != null)
            {
                db.UserPhotos.Remove(photo);
            }
        }
    }
}
