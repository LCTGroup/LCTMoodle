using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LCTMoodle
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Content",
                url: "{tapTin}.{dinhDang}/{*thuMuc}",
                defaults: new { controller = "LCT", action = "LayContent" }
            );

            routes.MapRoute(
                name: "LayTapTin",
                url: "LayTapTin/{loai}/{ma}",
                defaults: new { controller = "TapTin", action = "Lay" },
                constraints: new { ma = @"\d+" }
            );

            routes.MapRoute(
                name: "QuanLyChuDe",
                url: "ChuDe/QuanLy/{phamVi}/{ma}",
                defaults: new { controller = "ChuDe", action = "QuanLy", phamVi = UrlParameter.Optional, ma = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "XemKhoaHoc",
                url: "KhoaHoc/{ma}",
                defaults: new { controller = "KhoaHoc", action = "Xem" },
                constraints: new { ma = @"\d+" }
            );

            routes.MapRoute(
                name: "Xem_QuanLyGiaotrinh",
                url: "KhoaHoc/GiaoTrinh/{ma}",
                defaults: new { controller = "GiaoTrinh", action = "Index" }
            );

            routes.MapRoute(
                name: "BangDiem",
                url: "KhoaHoc/BangDiem/{action}/{ma}",
                defaults: new { controller = "BangDiem", action = "Index" }
            );

            routes.MapRoute(
                name: "QuanLyQuyen",
                url: "QuanLyQuyen/{phamVi}/{maDoiTuong}",
                defaults: new { controller = "Quyen", action = "QuanLy", phamVi = UrlParameter.Optional, maDoiTuong = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "XemCauHoi",
                url: "HoiDap/{ma}",
                defaults: new { controller = "HoiDap", action = "XemCauHoi", ma = UrlParameter.Optional },
                constraints: new { ma = @"\d+" }
            );

            routes.MapRoute(
                name: "MacDinh",
                url: "{controller}/{action}/{ma}",
                defaults: new { controller = "TrangChu", action = "Index", ma = UrlParameter.Optional }
            );
        }
    }
}
