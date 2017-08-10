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
using NLog;
using System.Web.UI;

namespace Social_Network.WEB.Controllers
{
    [NetworkAuthentication]
    public class UserController : Controller
    {
        private NetworkUsersDTO AuthenticatedUser 
        { 
            get 
            {
                if (_authenticatedUser == null) 
                {
                    _authenticatedUser = userService.GetUser(Guid.Parse(HttpContext.Request.Cookies["SocialNetworkID"].Value));
                }
                return _authenticatedUser;
            }
            set 
            {
                _authenticatedUser = value;
            }
        }
        private NetworkUsersDTO _authenticatedUser;
        public IUserService userService;
        private IUserPhotoService photoService;
        private IPostsService postsService;
        ILogger logger;
        public UserController(IUserService service, IUserPhotoService otherService, IPostsService postsServiceParam)
        {
            userService = service;
            photoService = otherService;
            postsService = postsServiceParam;
            logger = LogManager.GetCurrentClassLogger();
        }


        public ActionResult MainPage(string id)
        {
            NetworkUsersDTO urlUser = null;
            if (id != null)
            {
                try
                {
                    urlUser = userService.GetUser(id);
                }
                catch
                {

                }
            }
            UserInfoViewModel userInfo;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsersDTO, UserInfoViewModel>());
            var mapper = config.CreateMapper();
            userInfo = mapper.Map<NetworkUsersDTO, UserInfoViewModel>((urlUser != null) ? urlUser : AuthenticatedUser);
            userInfo.AuthenticatedURL = AuthenticatedUser.URL;
            return View(userInfo);
        }
        [OutputCache(Duration=50, Location= OutputCacheLocation.Server, VaryByParam="id")]
        public ActionResult Image(int? id)
        {
            if (id == null)
                return File(new byte[0], "jpeg");
            else
            {
                try
                {
                    var photo = photoService.GetPhoto(id);
                    return File(photo.Image, "jpeg");
                }
                catch
                {
                    return File(new byte[0], "jpeg");
                }
            }
        }
        [OutputCache(Duration = 50, Location = OutputCacheLocation.Server, VaryByParam = "id")]
        public ActionResult PostImage(int? id)
        {
            if (id == null)
                return File(new byte[0], "jpeg");
            else
            {
                try
                {
                    var photo = postsService.GetPost(id);
                    return File(photo.Image, "jpeg");
                }
                catch
                {
                    return File(new byte[0], "jpeg");
                }
            }
        }
        [HttpPost]
        public ActionResult MainPage(HttpPostedFileBase ImageFile)
        {
            UserInfoViewModel userInfo;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsersDTO, UserInfoViewModel>());
            var mapper = config.CreateMapper();
            userInfo = mapper.Map<NetworkUsersDTO, UserInfoViewModel>(AuthenticatedUser);
            userInfo.AuthenticatedURL = AuthenticatedUser.URL;
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
                    AuthenticatedUser.Photo_ID = photoService.GetAllPhotos().Last().ID;
                    userService.UpdateUser(AuthenticatedUser);
                    userInfo.Photo_ID = AuthenticatedUser.Photo_ID;
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NetworkUsersDTO, EditViewModel>());
            var mapper = config.CreateMapper();
            var model = mapper.Map<NetworkUsersDTO, EditViewModel>(AuthenticatedUser);
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
                    if (userService.GetAllUsers().Where(s => (s.Mail != AuthenticatedUser.Mail && s.Mail == model.Mail) || (s.URL != AuthenticatedUser.URL && s.URL == model.URL)).Any())
                    {
                        return View(model);
                    }
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<EditViewModel, NetworkUsersDTO>());
                    var mapper = config.CreateMapper();
                    var user = mapper.Map<EditViewModel, NetworkUsersDTO>(model);
                    user.UserGUID = AuthenticatedUser.UserGUID;
                    userService.UpdateUser(user);
                    AuthenticatedUser = userService.GetUser(user.ID);
                    return RedirectToAction("MainPage", "User");
                }
                catch
                {
                    return View(model);
                }
            }
            return View(model);

        }
        [HttpPost]
        public ActionResult UserList(string text)
        {
            var users = userService.GetAllUsers().Where(s => s.URL != AuthenticatedUser.URL && (s.Name + " " + s.Surname).ToUpper().Contains(text.ToUpper()));
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
                try
                {
                    var post = new PostsDTO()
                    {
                        Date = DateTime.Now,
                        Image = (image != null) ? StreamToByteArray(image.InputStream) : null,
                        Poster_ID = AuthenticatedUser.ID,
                        User_ID = id.Value,
                        Text = text
                    };
                    postsService.CreatePost(post);
                }
                catch 
                { 
                    
                }
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
                    PosterID = item.Poster_ID,
                    UserID = item.User_ID,
                    AuthenticatedID = AuthenticatedUser.ID,
                    URL = user.URL,
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
                postsService.ChangePostLike(AuthenticatedUser.ID, id);
                return Json(new { likes = postsService.GetPostLikes(id).Count() });
            }
            return Json(new { likes = 0 });
        }

        [HttpPost]
        public ActionResult RemovePost(int? id)
        {
            if (id != null)
            {
                try
                {
                    var post = postsService.GetPost(id);
                    if (post.Poster_ID == AuthenticatedUser.ID || post.User_ID == AuthenticatedUser.ID)
                    {
                        postsService.DeletePost(id);
                    }
                }
                catch(Exception ex)
                {
                    logger.Log(LogLevel.Error, ex.Message);
                }
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