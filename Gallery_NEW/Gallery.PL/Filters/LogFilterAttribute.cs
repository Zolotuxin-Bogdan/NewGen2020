using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace Gallery.PL.Filters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            
            var ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            var dnsName = request.UserHostName;
            var machineName = filterContext.HttpContext.Server.MachineName;
            var rawUrl = request.RawUrl;
            var userAgent = request.UserAgent;
            var httpMethod = request.HttpMethod;

            string messageRequest = "\n<<<\n[Request]" +
                                    "\nIP: " + ipAddress + 
                                    "\nDNS-name: " + dnsName + 
                                    "\nMachineName: " + machineName + 
                                    "\nURL-address: " + rawUrl + 
                                    "\nUserAgent: " + userAgent +
                                    "\nHttpMethod: " + httpMethod + "\n>>>\n";
            Logger.Info(messageRequest);

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            var status = response.Status;

            string messageResponse = "\n<<<\n[Response]" +
                                     "\nStatus: " + status + "\n>>>\n";
            Logger.Info(messageResponse);
            base.OnActionExecuted(filterContext);
        }
    }
}