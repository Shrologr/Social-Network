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
    public class NetworkUsersRepository: IRepository<NetworkUsers>
    {
        private SocialNetworkDatabaseContext db;
        public NetworkUsersRepository(SocialNetworkDatabaseContext context)
        {
            this.db = context;
        }
        public IEnumerable<NetworkUsers> GetAll()
        {
            return db.NetworkUsers;
        }

        public NetworkUsers Get(int id)
        {
            return db.NetworkUsers.Find(id);
        }

        public IEnumerable<NetworkUsers> Find(Expression<Func<NetworkUsers, bool>> predicate)
        {
            return db.NetworkUsers.Where(predicate).ToList();
        }

        public void Create(NetworkUsers item)
        {
            db.NetworkUsers.Add(item);
        }

        public void Update(NetworkUsers item)
        {
            var oldUser = Get(item.ID);
            db.Entry(oldUser).State = EntityState.Detached;
            db.NetworkUsers.Attach(item);
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            NetworkUsers user = db.NetworkUsers.Find(id);
            if (user != null)
                db.NetworkUsers.Remove(user);
        }
    }
}
