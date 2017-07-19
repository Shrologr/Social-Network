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
    public class UserPhotoService:IUserPhotoService
    {
        private IUnitOfWork Database;
        public UserPhotoService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<UserPhotosDTO> GetAllPhotos()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserPhotos, UserPhotosDTO>());
            return Mapper.Map<IEnumerable<UserPhotos>, IEnumerable<UserPhotosDTO>>(Database.UserPhotos.GetAll());
        }

        public IEnumerable<UserPhotosDTO> GetUserPhotos(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null", "");
            }
            Mapper.Initialize(cfg => cfg.CreateMap<UserPhotos, UserPhotosDTO>());
            return Mapper.Map<IEnumerable<UserPhotos>, IEnumerable<UserPhotosDTO>>(Database.UserPhotos.Find(s=>s.User_ID == id.Value));
        }

        public UserPhotosDTO GetPhoto(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null", "");
            }
            var photo = Database.UserPhotos.Get(id.Value);
            if (photo == null)
            {
                throw new ValidationException("No photo with this id", "");
            }
            Mapper.Initialize(cfg => cfg.CreateMap<UserPhotos, UserPhotosDTO>());
            return Mapper.Map<UserPhotos, UserPhotosDTO>(photo);
        }

        public void CreatePhoto(UserPhotosDTO photo)
        {
            throw new NotImplementedException();
        }

        public void DeletePhoto(UserPhotosDTO photo)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
