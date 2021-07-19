using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TracNghiemOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "ThiThu",
               url: "ThiThu/{id}",
               defaults: new { controller = "TrangChu", action = "Loald", id = UrlParameter.Optional }
           );

            routes.MapRoute(
            name: "phongthi",
            url: "PhongThi",
            defaults: new { controller = "TrangChu", action = "Phong", id = UrlParameter.Optional }
        );


            routes.MapRoute(
           name: "DeThi",
           url: "DeThi/{id}",
           defaults: new { controller = "Home", action = "DSDETHI", id = UrlParameter.Optional }, namespaces: new[] { "TracNghiemOnline.Areas.Admin.Controllers" }


       );

            routes.MapRoute(
          name: "DiemSo",
          url: "DiemSo/{id}",
          defaults: new { controller = "TrangChu", action = "DiemSo", id = UrlParameter.Optional }
      );


            routes.MapRoute(
            name: "DSDETHI",
            url: "DSDeThi/{id}",
            defaults: new { controller = "TrangChu", action = "DSDETHI", id = UrlParameter.Optional }
        );

            routes.MapRoute(
     name: "Diemthi",
     url: "Diemthi/{id}",
     defaults: new { controller = "TrangChu", action = "Diemthi", id = UrlParameter.Optional }
 );


            routes.MapRoute(
             name: "VaoThi",
             url: "VaoThi/{id}",
             defaults: new { controller = "TrangChu", action = "VaoThi", id = UrlParameter.Optional }
         );


            routes.MapRoute(
            name: "LopHoc",
            url: "LopHocPhan",
            defaults: new { controller = "TrangChu", action = "LopHocPhan", id = UrlParameter.Optional }
        );

            routes.MapRoute(
            name: "Bode",
            url: "Bode/{id}",
            defaults: new { controller = "TrangChu", action = "Bode", id = UrlParameter.Optional }
        );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
