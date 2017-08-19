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
using NLog;
using Social_Network.WEB.Util;

namespace Social_Network.WEB.Controllers
{
    [Log]
    public class UserLoginController : Controller
    {
        ILogger logger;
        IUserService userService;
        public UserLoginController(IUserService service)
        {
            userService = service;
            logger = LogManager.GetCurrentClassLogger();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = userService.GetUser(model.Mail, model.User_Password);
                    var sessionGuid = Guid.NewGuid();
                    user.UserGUID = sessionGuid;
                    userService.UpdateUser(user);
                    HttpContext.Response.Cookies["SocialNetworkID"].Value = sessionGuid.ToString();
                    NetworkAuthentication.AuthenticatedUsersIDs.Add(sessionGuid, user.ID);
                    return RedirectToAction("MainPage", "User");
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ex.FullWebMessage(HttpContext));
                    ModelState.AddModelError("", "Wrong email or password");
                    return View();
                }
            }
            return View();
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
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<RegistrarionViewModel, NetworkUsersDTO>());
                    var mapper = config.CreateMapper();
                    var newUser = mapper.Map<RegistrarionViewModel, NetworkUsersDTO>(model);
                    if (userService.GetUser(newUser.Mail, newUser.User_Password) != null)
                    {
                        ModelState.AddModelError("", "User with this email already exists.");
                        return View();
                    }
                    if (userService.GetUser(newUser.URL) != null)
                    {
                        ModelState.AddModelError("", "User with this url already exists.");
                        return View();
                    }
                    userService.CreateUser(newUser);
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ex.FullWebMessage(HttpContext));
                    return View();
                }
                var loginInfo = new LoginViewModel() { Mail = model.Mail, User_Password = model.User_Password };
                return RedirectToAction("Login", loginInfo);
            }
            return View();
        }

        public ActionResult Logout()
        {
            try
            {
                NetworkAuthentication.AuthenticatedUsersIDs.Remove(Guid.Parse(HttpContext.Request.Cookies["SocialNetworkID"].Value));
                HttpContext.Response.Cookies["SocialNetworkID"].Expires = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.FullWebMessage(HttpContext));
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