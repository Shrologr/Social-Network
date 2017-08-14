using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Social_Network.BLL.DTO;

namespace Social_Network.WEB.Filters
{
    public class NetworkAuthentication : FilterAttribute, IAuthenticationFilter
    {
        public static Dictionary<Guid, int> AuthenticatedUsersIDs = new Dictionary<Guid, int>();
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var authenticatedStatus = filterContext.HttpContext.Request.Cookies["SocialNetworkID"];
            if (authenticatedStatus == null || authenticatedStatus.Value == "")
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
            Guid userGuid = Guid.Parse(authenticatedStatus.Value);
            if (!AuthenticatedUsersIDs.ContainsKey(userGuid))
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var authenticatedStatus = filterContext.HttpContext.Request.Cookies["SocialNetworkID"];
            if (authenticatedStatus == null || authenticatedStatus.Value == "")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { 
                    { "controller", "UserLogin" }, { "action", "Login" } 
                   });
                return;
            }
            Guid userGuid = Guid.Parse(authenticatedStatus.Value);
            if (!AuthenticatedUsersIDs.ContainsKey(userGuid))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { 
                    { "controller", "UserLogin" }, { "action", "Login" } 
                   });
                return;
            }
        }
    }
}