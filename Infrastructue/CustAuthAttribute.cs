using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace Ames.Infrastructue
{
    public class CustAuthAttribute : AuthorizeAttribute
    {
        private bool localAllowed;
    
        public CustAuthAttribute(bool allowedParam) {
            localAllowed = allowedParam;
        }
        
        protected override bool AuthorizeCore(HttpContextBase httpContext) {
            if (httpContext.Request.IsLocal) {
                return localAllowed;
            } else {
                return true;
            }
        }

    }
}
