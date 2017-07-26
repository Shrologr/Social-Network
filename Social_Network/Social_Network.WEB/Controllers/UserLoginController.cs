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

namespace Social_Network.WEB.Controllers
{
    public class UserLoginController : Controller
    {
        IUserService userService;
        public UserLoginController(IUserService service)
        {
            userService = service;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                var user = userService.GetUser(model.Mail, model.User_Password);
                var sessionGuid = Guid.NewGuid();

                user.UserGUID = sessionGuid;
                userService.UpdateUser(user);
                HttpContext.Response.Cookies["SocialNetworkID"].Value = sessionGuid.ToString();
                return RedirectToAction("MainPage", "User");
            }
            catch 
            {
                return View();
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegistrarionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<RegistrarionViewModel, NetworkUsersDTO>());
                    var newUser = Mapper.Map<RegistrarionViewModel, NetworkUsersDTO>(model);
                    userService.CreateUser(newUser);
                }
                catch
                {
                    return View();
                }
                var loginInfo = new LoginViewModel() { Mail = model.Mail, User_Password = model.User_Password };
                return RedirectToAction("Login", loginInfo);
            }
            return View();
        }

        public ActionResult Logout() 
        {
            HttpContext.Response.Cookies["SocialNetworkID"].Expires = DateTime.Now;
            return RedirectToAction("MainPage", "User");
        }
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}