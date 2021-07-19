using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TracNghiemOnline.Modell;

namespace TracNghiemOnline.Areas.Admin
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            if (session == null)
            {
                     filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary( new { controller = "LoginAdmin", action = "Login" } ));
            }
            Session[ComMon.ComMonStants.UserLogin]=session;
            base.OnActionExecuting(filterContext);
        }
      
    }
}