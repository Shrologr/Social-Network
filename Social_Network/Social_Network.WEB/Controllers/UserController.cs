using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social_Network.WEB.Models;
using AutoMapper;
using Social_Network.BLL.DTO;
using Social_Network.BLL.Infrastructure;
using Social_Network.BLL.Interfaces;
using Social_Network.WEB.Filters;
using System.IO;

namespace Social_Network.WEB.Controllers
{
    [NetworkAuthentication]
    public class UserController : Controller
    {
        public IUserService userService;
        private IUserPhotoService photoService;
        public UserController(IUserService service, IUserPhotoService otherService)
        {
            userService = service;
            photoService = otherService;
        }
        public ActionResult MainPage(string id)
        {
            NetworkUsersDTO urlUser = null;
            if (id != null)
            {
                urlUser = userService.GetUser(id);
            }
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsersDTO, UserInfoViewModel>());
            UserInfoViewModel userInfo;
            userInfo = Mapper.Map<NetworkUsersDTO, UserInfoViewModel>((urlUser != null) ? urlUser : NetworkAuthentication.AuthenticatedUser);
            userInfo.AuthenticatedURL = NetworkAuthentication.AuthenticatedUser.URL;
            return View(userInfo);
        }
        public ActionResult Image(int? id)
        {
            if (id == null)
                return File(new byte[0], "jpeg");
            else 
            {
                var photo = photoService.GetPhoto(id);
                return File(photo.Image, "jpeg");
            }
        }
        [HttpPost]
        public ActionResult MainPage(HttpPostedFileBase ImageFile)
        {
            UserInfoViewModel userInfo;
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsersDTO, UserInfoViewModel>());
            userInfo = Mapper.Map<NetworkUsersDTO, UserInfoViewModel>(NetworkAuthentication.AuthenticatedUser);
            userInfo.AuthenticatedURL = NetworkAuthentication.AuthenticatedUser.URL;
            if (ImageFile == null)
                return View(userInfo);
            else
            {
                var image = StreamToByteArray(ImageFile.InputStream);
                if (userInfo.Photo_ID == null)
                {
                    var newPhoto = new UserPhotosDTO()
                    {
                        Image = image
                    };
                    photoService.CreatePhoto(newPhoto);
                    NetworkAuthentication.AuthenticatedUser.Photo_ID = photoService.GetAllPhotos().Last().ID;
                    userService.UpdateUser(NetworkAuthentication.AuthenticatedUser);
                    userInfo.Photo_ID = NetworkAuthentication.AuthenticatedUser.Photo_ID;
                    return View(userInfo);
                }
                var photo = photoService.GetPhoto(userInfo.Photo_ID);
                photo.Image = image;
                photoService.UpdatePhoto(photo);
                return View(userInfo);
            }
        }

        public ActionResult Edit()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsersDTO, EditViewModel>());
            var model = Mapper.Map<NetworkUsersDTO, EditViewModel>(NetworkAuthentication.AuthenticatedUser);
            return View(model);
        }
        private byte[] StreamToByteArray(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        [HttpPost]
        public ActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<EditViewModel, NetworkUsersDTO>());
                    var user = Mapper.Map<EditViewModel, NetworkUsersDTO>(model);
                    user.UserGUID = NetworkAuthentication.AuthenticatedUser.UserGUID;
                    userService.UpdateUser(user);
                    NetworkAuthentication.AuthenticatedUser = userService.GetUser(user.ID);
                }
                catch
                {
                    return View(model);
                }
            }
            return RedirectToAction("MainPage", "User");
        }
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}