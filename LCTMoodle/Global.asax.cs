using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;

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

            taoThuMuc(Server.MapPath("~/Uploads/HinhDaiDien_ChuDe"));
            taoThuMuc(Server.MapPath("~/Uploads/HinhDaiDien_KhoaHoc"));
        }

        static private void taoThuMuc(string duongDanThuMuc)
        {
            if (!Directory.Exists(duongDanThuMuc))
            {
                Directory.CreateDirectory(duongDanThuMuc);
            }
        }

        protected void Session_Start()
        {
            Session["NguoiDung"] = null;
        }
    }
}
