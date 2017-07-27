using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social_Network.BLL.DTO;

namespace Social_Network.BLL.Interfaces
{
    public interface IUserPhotoService
    {
        IEnumerable<UserPhotosDTO> GetAllPhotos();
        UserPhotosDTO GetPhoto(int? id);
        void CreatePhoto(UserPhotosDTO photo);
        void UpdatePhoto(UserPhotosDTO photo);
        void DeletePhoto(int? id);
        void Dispose();
    }
}
