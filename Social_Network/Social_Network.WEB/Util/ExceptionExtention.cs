using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social_Network.WEB.Util
{
    public static class ExceptionExtention
    {
        public static string FullWebMessage(this Exception ex, HttpContextBase context) 
        {
            var request = context.Request;
            string address = request.RawUrl;
            string ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            string method = request.HttpMethod;
            return method + " " + ip + "/" + address + "StackTrace: " + ex.StackTrace + "Message: " + ex.Message;
        }
    }
}