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
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/NguoiDung_HinhDaiDien"));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.Cookies)
            {
                //supports the cookies
            }
            else
            {
                //not supports the cookies
                //redirect user on specific page
                //for this or show messages
            }
        }
        protected void Session_Start()
        {
            Response.Cache.SetExpires(DateTime.Now);

            HttpCookie nguoiDungCookie = new HttpCookie("NguoiDung");
            if (nguoiDungCookie.Value == null)
            {
                Session["NguoiDung"] = null;
            }            
        }
    }
}
