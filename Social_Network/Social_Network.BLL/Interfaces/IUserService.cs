using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social_Network.BLL.DTO;

namespace Social_Network.BLL.Interfaces
{
    public interface IUserService
    {
        NetworkUsersDTO GetUser(string email, string password);
        NetworkUsersDTO GetUser(int? id);
        IEnumerable<NetworkUsersDTO> GetAllUsers();
        void CreateUser(NetworkUsersDTO user);
        void DeleteUser(int? id);
        void Dispose();
    }
}
