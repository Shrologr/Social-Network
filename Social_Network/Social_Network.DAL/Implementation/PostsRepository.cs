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
    public class PostsRepository:IRepository<Posts>
    {
        private SocialNetworkDatabaseContext db;
        public PostsRepository(SocialNetworkDatabaseContext context)
        {
            this.db = context;
        }
        public IEnumerable<Posts> GetAll()
        {
            return db.Posts;
        }

        public Posts Get(int id)
        {
            return db.Posts.Find(id);
        }

        public IEnumerable<Posts> Find(Expression<Func<Posts, bool>> predicate)
        {
            return db.Posts.Where(predicate).ToList();
        }

        public void Create(Posts item)
        {
            db.Posts.Add(item);
        }

        public void Update(Posts item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Posts post = db.Posts.Find(id);
            if (post != null)
            {
                db.Posts.Remove(post);
            }
        }
    }
}