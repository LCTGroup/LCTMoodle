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
                name: "LayHinh",
                url: "LayHinh/{loai}/{ma}",
                defaults: new { controller = "TapTin", action = "LayHinh" },
                constraints: new { ma = @"\d+" }
            );

            routes.MapRoute(
                name: "DocTapTin",
                url: "DocTapTin/{loai}/{ma}",
                defaults: new { controller = "TapTin", action = "Doc" },
                constraints: new { ma = @"\d+" }
            );

            routes.MapRoute(
                name: "XemKhoaHoc",
                url: "KhoaHoc/{ma}",
                defaults: new { controller = "KhoaHoc", action = "Xem" },
                constraints: new { ma = @"\d+" }
            );

            routes.MapRoute(
                name: "KhoaHocCuaToi",
                url: "KhoaHocCuaToi",
                defaults: new { controller = "KhoaHoc", action = "DanhSachCuaToi" }
            );

            routes.MapRoute(
                name: "Xem_QuanLyChuongTrinh",
                url: "KhoaHoc/ChuongTrinh/{maKhoaHoc}",
                defaults: new { controller = "ChuongTrinh", action = "Index", maKhoaHoc = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BangDiem",
                url: "KhoaHoc/{action}-BangDiem/{maKhoaHoc}",
                defaults: new { controller = "BangDiem", action = "Index", maKhoaHoc = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChamDiemBaiTap",
                url: "KhoaHoc/Cham-BaiTap/{ma}",
                defaults: new { controller = "BaiVietBaiTap", action = "ChamDiem", ma = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "QuanLyQuyen",
                url: "QuanLyQuyen/{phamVi}/{maDoiTuong}",
                defaults: new { controller = "Quyen", action = "QuanLy", phamVi = UrlParameter.Optional, maDoiTuong = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "QuyenCuaToi",
                url: "QuyenCuaToi",
                defaults: new { controller = "Quyen", action = "QuyenCuaToi" }
            );

            routes.MapRoute(
                name: "XemCauHoi",
                url: "HoiDap/{ma}",
                defaults: new { controller = "HoiDap", action = "XemCauHoi", ma = UrlParameter.Optional },
                constraints: new { ma = @"\d+" }
            );

            //routes.MapRoute(
            //    name: "XemNguoiDung",
            //    url: "NguoiDung/{ma}",
            //    defaults: new { controller = "NguoiDung", action = "Xem", ma = UrlParameter.Optional },
            //    constraints: new { ma = @"\d+" }
            //);

            routes.MapRoute(
                name: "NguoiDung/ChiTietTinNhan/{tenTaiKhoanNguoiGui}",
                url: "{controller}/{action}/{tenTaiKhoanKhach}",
                defaults: new { controller = "NguoiDung", action = "ChiTietTinNhan", tenTaiKhoanNguoiGui = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "MacDinh",
                url: "{controller}/{action}/{ma}",
                defaults: new { controller = "TrangChu", action = "Index", ma = UrlParameter.Optional }
            );
        }
    }
}
