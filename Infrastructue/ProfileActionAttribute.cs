using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using Ames.Entities;


namespace Ames.Infrastructue {
    public class ProfileActionAttribute : ActionFilterAttribute {
        
        private Stopwatch timer;
        private EFAmesInfra db = new EFAmesInfra();
        string moduleName;
        string controllerName;
        string actionName;
        string machineName;
        string absoluteURL;

        private decimal actionDuration;
        private decimal resultDuration;
       


        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            machineName = filterContext.HttpContext.Server.MachineName;
            absoluteURL = filterContext.HttpContext.Request.Url.AbsoluteUri;
            timer = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            timer.Stop();
            actionDuration = (decimal) timer.Elapsed.TotalMilliseconds;
            moduleName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.UnderlyingSystemType.Module.Name;
            controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            actionName = filterContext.ActionDescriptor.ActionName;

            if (filterContext.Exception != null) {
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
                    ErrorMessage = filterContext.Exception.Message + ">>" + filterContext.Exception.StackTrace,
                };
                // save the profilelog into Db
                db.ProfileActionLogs.Add(record);
                db.SaveChanges();
            }
        }

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

    }
}
