using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TracNghiemOnline.Modell;

namespace TracNghiemOnline.Controllers
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
                    new System.Web.Routing.RouteValueDictionary( new { controller = "Login", action = "Login" } ));
            }
    
            base.OnActionExecuting(filterContext);
        }
      
    }
}