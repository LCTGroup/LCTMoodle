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

            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/Tam"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/ChuDe_HinhDaiDien"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/KhoaHoc_HinhDaiDien"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/BaiVietDienDan_TapTin"));
        }

        protected void Session_Start()
        {
            Session["NguoiDung"] = null;
        }
    }
}
