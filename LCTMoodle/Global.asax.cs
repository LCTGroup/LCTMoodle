using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;
using Helpers;
using Data;
using BUSLayer;
using DTOLayer;

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
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/BaiVietBaiTap_TapTin"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/BaiVietBaiGiang_TapTin"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/BaiVietTaiLieu_TapTin"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/NguoiDung_HinhDaiDien"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/BinhLuanBaiVietDienDan_TapTin"));
            LCTHelper.taoThuMuc(Server.MapPath("~/Uploads/BaiTapNop_TapTin"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        
        protected void Session_Start()
        {
            NguoiDungBUS.kiemTraCookie();
        }
    }
}
