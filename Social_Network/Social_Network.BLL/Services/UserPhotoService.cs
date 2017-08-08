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
    public class UserPhotoService: IUserPhotoService
    {
        private IUnitOfWork Database;
        public UserPhotoService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<UserPhotosDTO> GetAllPhotos()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserPhotos, UserPhotosDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<UserPhotos>, IEnumerable<UserPhotosDTO>>(Database.UserPhotos.GetAll());
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserPhotos, UserPhotosDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<UserPhotos, UserPhotosDTO>(photo);
        }

        public void CreatePhoto(UserPhotosDTO photo)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserPhotosDTO, UserPhotos>());
            var mapper = config.CreateMapper();
            var newPhoto = mapper.Map<UserPhotosDTO, UserPhotos>(photo);
            Database.UserPhotos.Create(newPhoto);
            Database.Save();
        }

        public void DeletePhoto(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Id is null", "");
            }
            if (Database.UserPhotos.Get(id.Value) == null)
            {
                throw new ValidationException("Such photo does not exist", "");
            }
            var somePhoto = Database.UserPhotos.Get(id.Value);
            var user = Database.NetworkUsers.Find(s=>s.Photo_ID == id.Value).FirstOrDefault();
            if (user != null)
            {
                user.Photo_ID = null;
            }
            Database.UserPhotos.Delete(id.Value);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }


        public void UpdatePhoto(UserPhotosDTO photo)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserPhotosDTO, UserPhotos>());
            var mapper = config.CreateMapper();
            var photoForUpdate = mapper.Map<UserPhotosDTO, UserPhotos>(photo);
            Database.UserPhotos.Update(photoForUpdate);
            Database.Save();
        }
    }
}