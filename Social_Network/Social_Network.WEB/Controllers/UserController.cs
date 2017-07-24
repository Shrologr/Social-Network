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
    public class UserController : Controller
    {
        IUserService userService;
        public UserController(IUserService service)
        {
            userService = service;
        }
        public ActionResult Login()
        {
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
                    Mapper.Initialize(cfg => cfg.CreateMap<RegistrarionViewModel, NetworkUsersDTO>());
                    var newUser = Mapper.Map<RegistrarionViewModel, NetworkUsersDTO>(model);
                    userService.CreateUser(newUser);
                }
                catch (Exception ex)
                {
                    return View();
                }
                var loginInfo = new LoginViewModel() { Mail = model.Mail, User_Password = model.User_Password };
                return View("Login", loginInfo);
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}