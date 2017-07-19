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
        IEnumerable<UserPhotosDTO> GetUserPhotos(int? id);
        UserPhotosDTO GetPhoto(int? id);
        void CreatePhoto(UserPhotosDTO photo);
        void DeletePhoto(UserPhotosDTO photo);
        void Dispose();
    }
}
