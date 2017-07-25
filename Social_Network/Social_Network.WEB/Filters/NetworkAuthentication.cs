using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Social_Network.WEB.Filters
{
    public class NetworkAuthentication:FilterAttribute,IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var authenticatedStatus = filterContext.HttpContext.Request.Cookies["SocialNetworkID"];
            if (authenticatedStatus == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var authenticatedStatus = filterContext.HttpContext.Request.Cookies["SocialNetworkID"];
            if (authenticatedStatus == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { 
                    { "controller", "UserLogin" }, { "action", "Login" } 
                   });
            }
        }
    }
}