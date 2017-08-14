using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace Social_Network.WEB.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            ILogger logger = LogManager.GetCurrentClassLogger();
            string address = request.RawUrl;
            string ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            string method = request.HttpMethod;
            logger.Log(LogLevel.Info, method + " " + ip + "/" + address);
            base.OnActionExecuting(filterContext);
        }
    }
}