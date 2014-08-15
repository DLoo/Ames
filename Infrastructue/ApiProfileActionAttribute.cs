using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Data.Entity;
using Ames.Entities;


namespace Ames.Infrastructue {
    public class ApiProfileActionAttribute : ActionFilterAttribute {
        
        private Stopwatch timer;
        private EFAmesInfra db = new EFAmesInfra();
        string moduleName;
        string controllerName;
        string actionName;
        string machineName;
        string absoluteURL;

        private decimal actionDuration;
//        private decimal resultDuration;



        public override void OnActionExecuting(HttpActionContext filterContext) {
            machineName = System.Web.HttpContext.Current.Request.UserHostName + " = " +
                System.Web.HttpContext.Current.Request.UserHostAddress;  //filterContext.HttpContext.Server.MachineName;
            absoluteURL = filterContext.Request.RequestUri.AbsoluteUri; //filterContext.HttpContext.Request.Url.AbsoluteUri;
            timer = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext) {
            timer.Stop();
            actionDuration = (decimal) timer.Elapsed.TotalMilliseconds;
            moduleName = filterContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType.UnderlyingSystemType.Module.Name;  //filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.UnderlyingSystemType.Module.Name;
            controllerName = (filterContext.ActionContext.ActionDescriptor).ControllerDescriptor.ControllerName; //filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            actionName = filterContext.Request.Method.Method + " - " + (filterContext.ActionContext.ActionDescriptor).ActionName; //filterContext.ActionDescriptor.ActionName;

            
                ProfileActionLog record = new ProfileActionLog {
                    //ProfileID = 0,
                    DateTime = System.DateTime.Now,
                    UserName = filterContext.ActionContext.RequestContext.Principal.Identity.Name, // filterContext.RequestContext.HttpContext.User.Identity.Name,
                    Module = moduleName,
                    Controller = controllerName,
                    Action = actionName,
                    BrowserType = ((System.Web.Configuration.HttpCapabilitiesBase)(System.Web.HttpContext.Current.Request.Browser)).Browser, //filterContext.HttpContext.Request.Browser.Type,
                    MachineName = machineName,
                    AbsoluteURL = absoluteURL,
                    ActionDuration = actionDuration,
                    //ErrorMessage = filterContext.Exception.Message + ">>" + filterContext.Exception.StackTrace,
                };
                if (filterContext.Exception != null)
                    record.ErrorMessage = filterContext.Exception.Message + ">>" + filterContext.Exception.StackTrace;
                // save the profilelog into Db
                db.ProfileActionLogs.Add(record);
                db.SaveChanges();
            
        }

        /*
        public override void OnResultExecuting(ResultExecutingContext filterContext) {
            timer.Restart();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext) {
            
            timer.Stop();
            string errorMessage = null;
            
            resultDuration = (decimal)timer.Elapsed.TotalMilliseconds;
                
            if (filterContext.Exception != null) {
                errorMessage = filterContext.Exception.Message + ">>" + filterContext.Exception.StackTrace;
            }
            // save the profilelog into Db  
            ProfileActionLog record = new ProfileActionLog {
                //ProfileID = 0,
                DateTime = System.DateTime.Now,
                UserName = filterContext.RequestContext.HttpContext.User.Identity.Name,
                Module = moduleName,
                Controller = controllerName,
                Action = actionName,
                BrowserType = filterContext.HttpContext.Request.Browser.Type,
                MachineName = machineName,
                AbsoluteURL = absoluteURL,
                ActionDuration = actionDuration,
                ResultDuration = resultDuration,
                ErrorMessage = errorMessage,
            };
            db.ProfileActionLogs.Add(record);
            db.SaveChanges();
        }
        */

    }
}
