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
    public class UserController : Controller
    {
        IUserService userService;
        public UserController(IUserService service)
        {
            userService = service;
        }
        [NetworkAuthentication]
        public ActionResult MainPage()
        {
            var guid = Guid.Parse(HttpContext.Request.Cookies["SocialNetworkID"].Value);
            var user = userService.GetUser(guid);
            Mapper.Initialize(cfg => cfg.CreateMap<NetworkUsersDTO, UserInfoViewModel>());
            var userInfo = Mapper.Map<NetworkUsersDTO, UserInfoViewModel>(user);
            return View(userInfo);
        }
	}
}