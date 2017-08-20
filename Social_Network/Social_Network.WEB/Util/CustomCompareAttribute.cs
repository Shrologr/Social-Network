using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Social_Network.WEB.Util
{
    public class CustomCompareAttribute : CompareAttribute
    {
        public CustomCompareAttribute(string otherProperty, string ErrorCode)
            : base(otherProperty)
        {
            ErrorMessage = CustomMessage(ErrorCode);
        }

        private static string CustomMessage(string ErrorCode)
        {
            string ErrorMsg = ErrorCode;
            return ErrorMsg;
        }
    }
}