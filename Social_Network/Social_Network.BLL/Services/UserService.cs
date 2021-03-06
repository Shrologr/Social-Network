﻿using System;
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
    public class UserService: IUserService
    {
        private IUnitOfWork Database { get; set; }
        private IPostsService PostService { get; set; }
        private IUserPhotoService UserPhotoService { get; set; }
        public UserService(IUnitOfWork uow, IPostsService postService, IUserPhotoService userPhotoService)
        {
            Database = uow;
            PostService = postService;
            UserPhotoService = userPhotoService;
        }
        public NetworkUsersDTO GetUser(string email, string password)
        {
            var users = Database.NetworkUsers.Find(s => s.Mail == email && s.User_Password == password);
            if (users.Count() == 0)
            {
                return null;
            }
            if (users.Count() > 1)
            {
                throw new ValidationException("Error. To many users...", ""); 
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<NetworkUsers>, IEnumerable<NetworkUsersDTO>>(users).First();
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<NetworkUsers, NetworkUsersDTO>(user);
        }

        public IEnumerable<NetworkUsersDTO> GetAllUsers()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<NetworkUsers>, IEnumerable<NetworkUsersDTO>>(Database.NetworkUsers.GetAll());
        }

        public void CreateUser(NetworkUsersDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsersDTO, NetworkUsers>());
            var mapper = config.CreateMapper();
            var newUser = mapper.Map<NetworkUsersDTO, NetworkUsers>(user);
            if (Database.NetworkUsers.Find(s => (s.Mail == newUser.Mail) || (s.URL == newUser.URL)).Any())
            {
                throw new ValidationException("Such user already exists", ""); 
            }
            Database.NetworkUsers.Create(newUser);
            Database.Save();
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
            Database.NetworkUsers.Get(id.Value).Posts.ToList().ForEach(a => PostService.ChangePostLike(id, a.ID));
            Database.NetworkUsers.Get(id.Value).PostsPosterID.ToList().ForEach(a => PostService.DeletePost(a.ID));
            Database.NetworkUsers.Get(id.Value).PostsUserID.ToList().ForEach(a => PostService.DeletePost(a.ID));
            Database.NetworkUsers.Delete(id.Value);
            Database.Save();
        }
        public void UpdateUser(NetworkUsersDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsersDTO, NetworkUsers>());
            var mapper = config.CreateMapper();
            var userForUpdate = mapper.Map<NetworkUsersDTO, NetworkUsers>(user);
            Database.NetworkUsers.Update(userForUpdate);
            Database.Save();
        }
        public void Dispose()
        {
            Database.Dispose();
            PostService.Dispose();
            UserPhotoService.Dispose();
        }


        public NetworkUsersDTO GetUser(Guid guid)
        {
            var users = Database.NetworkUsers.Find(s => s.UserGUID == guid);
            if (users.Count() == 0)
            {
                return null;
            }
            if (users.Count() > 1)
            {
                throw new ValidationException("Error. To many users...", "");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<NetworkUsers>, IEnumerable<NetworkUsersDTO>>(users).First();
        }


        public NetworkUsersDTO GetUser(string url)
        {
            var users = Database.NetworkUsers.Find(s => s.URL == url);
            if (users.Count() == 0)
            {
                return null;
            }
            if (users.Count() > 1)
            {
                throw new ValidationException("Error. To many users...", "");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsers, NetworkUsersDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<NetworkUsers>, IEnumerable<NetworkUsersDTO>>(users).First();
        }
    }
}