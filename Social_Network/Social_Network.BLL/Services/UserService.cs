using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social_Network.BLL.Interfaces;
using Social_Network.BLL.DTO;
using Social_Network.BLL.Infrastructure;
using Social_Network.DAL.Interfaces;
using Social_Network.DAL.EF;
using AutoMapper;

namespace Social_Network.BLL.Services
{
    public class UserService:IUserService
    {
        private IUnitofWork Database { get; set; }

        public UserService(IUnitofWork uow)
        {
            Database = uow;
        }
        public NetworkUsersDTO GetUser(string email, string password)
        {
            var users = Database.NetworkUsers.Find(s => s.Mail == email && s.User_Password == password);
            if (users.Count() == 0)
            {
                throw new ValidationException("Wrong email or password","");
            }
            if (users.Count() > 1)
            {
                throw new ValidationException("Error. To many users...", ""); 
            }
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            return Mapper.Map<IEnumerable<NetworkUsers>, IEnumerable<NetworkUsersDTO>>(users).First();
        }

        public NetworkUsersDTO GetUser(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null", "");            
            }
            var user = Database.NetworkUsers.Get(id.Value);
            if (user == null)
            {
                throw new ValidationException("No user with this id", "");
            }
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            return Mapper.Map<NetworkUsers, NetworkUsersDTO>(user);
        }

        public IEnumerable<NetworkUsersDTO> GetAllUsers()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            return Mapper.Map<IEnumerable<NetworkUsers>, IEnumerable<NetworkUsersDTO>>(Database.NetworkUsers.GetAll());
        }

        public void CreateUser(NetworkUsersDTO user)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsersDTO, NetworkUsers>());
            var newUser = Mapper.Map<NetworkUsersDTO, NetworkUsers>(user);
            if (Database.NetworkUsers.Find(s => s.Mail == newUser.Mail).Count() > 0)
            {
                throw new ValidationException("Such user already exists", ""); 
            }
            Database.NetworkUsers.Create(newUser);
        }

        public void DeleteUser(int? id)
        {
            if (id == null)
            { 
                throw new ValidationException("Id is null", "");                 
            }
            if (Database.NetworkUsers.Get(id.Value) == null)
            {
                throw new ValidationException("Such user does not exist", "");                 
            }
            Database.NetworkUsers.Delete(id.Value);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
