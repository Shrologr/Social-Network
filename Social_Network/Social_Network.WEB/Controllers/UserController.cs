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

namespace Social_Network.WEB.Controllers
{
    [NetworkAuthentication]
    public class UserController : Controller
    {
        IUserService userService;
        public UserController(IUserService service)
        {
            userService = service;
        }
        public ActionResult MainPage(string id)
        {
            var user = GetAuthenticatedUser();
            NetworkUsersDTO urlUser = null;
            if (id != null)
            {
                urlUser = userService.GetUser(id);
            }
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsersDTO, UserInfoViewModel>());
            UserInfoViewModel userInfo;
            userInfo = Mapper.Map<NetworkUsersDTO, UserInfoViewModel>((urlUser != null) ? urlUser : user);
            userInfo.AuthenticatedURL = user.URL;
            return View(userInfo);
        }

        public ActionResult Edit() 
        {
            var user = GetAuthenticatedUser();
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsersDTO, EditViewModel>());
            var model = Mapper.Map<NetworkUsersDTO, EditViewModel>(user);
            return View(model);
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
                    user.UserGUID = Guid.Parse(HttpContext.Request.Cookies["SocialNetworkID"].Value);
                    userService.UpdateUser(user);
                }
                catch 
                {
                    return View(model);
                }
            }
            return RedirectToAction("MainPage", "User");
        }

        private NetworkUsersDTO GetAuthenticatedUser()
        {
            var guid = Guid.Parse(HttpContext.Request.Cookies["SocialNetworkID"].Value);
            return userService.GetUser(guid);
        }
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}