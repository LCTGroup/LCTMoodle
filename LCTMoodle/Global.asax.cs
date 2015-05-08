using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;
using Helpers;

namespace LCTMoodle
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/HinhDaiDien_ChuDe"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/HinhDaiDien_KhoaHoc"));
        }

        protected void Session_Start()
        {
            Session["NguoiDung"] = null;
        }
    }
}
