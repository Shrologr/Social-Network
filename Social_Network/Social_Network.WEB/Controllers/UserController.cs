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
        private IPostsService postsService;
        public UserController(IUserService service, IUserPhotoService otherService, IPostsService postsServiceParam)
        {
            userService = service;
            photoService = otherService;
            postsService = postsServiceParam;
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

        public ActionResult PostImage(int? id)
        {
            if (id == null)
                return File(new byte[0], "jpeg");
            else
            {
                var photo = postsService.GetPost(id);
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
                    if (userService.GetAllUsers().Where(s => (s.Mail != NetworkAuthentication.AuthenticatedUser.Mail && s.Mail == model.Mail) || (s.URL != NetworkAuthentication.AuthenticatedUser.URL && s.URL == model.URL)).Any())
                        return View(model);
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
        [HttpPost]
        public ActionResult UserList(string text)
        {
            var users = userService.GetAllUsers().Where(s => s.URL != NetworkAuthentication.AuthenticatedUser.URL && (s.Name+" "+s.Surname).ToUpper().Contains(text.ToUpper()));
            List<UserListItemViewModel> userList = new List<UserListItemViewModel>();
            foreach (var item in users)
            {
                userList.Add(new UserListItemViewModel() 
                { 
                    URL = item.URL,
                    Name = item.Name,
                    Surname = item.Surname
                });
            }
            return PartialView("UserList", userList);
        }

        [HttpPost]
        public ActionResult PostCreate(int? id, string text, HttpPostedFileBase image)
        {
            if (id != null)
            {
                var post = new PostsDTO()
                {
                    Date = DateTime.Now,
                    Image = StreamToByteArray(image.InputStream),
                    Poster_ID = NetworkAuthentication.AuthenticatedUser.ID,
                    User_ID = id.Value,
                    Text = text
                };
                postsService.CreatePost(post);
            }
            return Json(new { result = "Something" });
        }



        [HttpPost]
        public ActionResult PostList(int? id)
        {
            var posts = postsService.GetUserPosts(id);
            var newPosts = new List<PostListItemViewModel>();
            foreach (var item in posts)
            {
                var user = userService.GetUser(item.Poster_ID);
                newPosts.Add(new PostListItemViewModel()
                {
                    ID = item.ID,
                    PosterFullName = user.Name + " " + user.Surname,
                    Text = item.Text,
                    Date = item.Date,
                    URL = user.URL,
                    Authenticated_URL = NetworkAuthentication.AuthenticatedUser.URL,
                    Likes = postsService.GetPostLikes(item.ID).Count()
                }
                );
            }
            return PartialView("PostList", newPosts);
        }

        [HttpPost]
        public ActionResult LikePost(int? id)
        {
            if (id != null)
            {
                postsService.ChangePostLike(NetworkAuthentication.AuthenticatedUser.ID, id);
                return Json(new { likes = postsService.GetPostLikes(id).Count() });
            }
            return Json(new { likes = 0 });
        }

        [HttpPost]
        public ActionResult RemovePost(int? id)
        {
            if (id != null)
            {
                postsService.DeletePost(id);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ShowUserLikes(int? id) 
        {
            if (id != null) 
            {
                var likes = postsService.GetPostLikes(id);
                List<UserListItemViewModel> userList = new List<UserListItemViewModel>();
                foreach (var item in likes)
                {
                    userList.Add(new UserListItemViewModel()
                    {
                        URL = item.URL,
                        Name = item.Name,
                        Surname = item.Surname
                    });
                }
                return PartialView("LikeList", userList);
            }
            return new EmptyResult();
        }
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}