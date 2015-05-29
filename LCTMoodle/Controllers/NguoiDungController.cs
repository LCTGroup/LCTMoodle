using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Data;

namespace LCTMoodle.Controllers
{
    public class NguoiDungController : LCTController
    {
        //
        // GET: /NguoiDung/
        public ActionResult Index()
        {
            return View(NguoiDungBUS.layTheoMa((int)Session["NguoiDung"]));
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <returns></returns>
        public ActionResult DangNhap()
        {
            //Tắt hiển thị cột trái, cột phải
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;                        

            return View();
        }
        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <returns></returns>
        public ActionResult DangKy()
        {
            //Tắt hiển thị cột trái, cột phải
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;
         
            return View();
        }

        [HttpPost]        
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            KetQua ketQua = NguoiDungBUS.them(chuyenDuLieuForm(formCollection));

            return Json(ketQua);
        }

        [HttpPost]
        public ActionResult XuLyKiemTraDangNhap(FormCollection formCollection)
        {
            KetQua ketQua = NguoiDungBUS.kiemTraDangNhap(chuyenDuLieuForm(formCollection));

            return Json(ketQua);
        }

        public ActionResult XuLyDangXuat()
        {
            NguoiDungBUS.xuLyDangXuat();

            return RedirectToAction("Index", "TrangChu");
        }
        
        [HttpGet]
        public ActionResult KiemTraTenTaiKhoan(string tenTaiKhoan)
        {
            return Json(NguoiDungBUS.kiemTraTenTaiKhoan(tenTaiKhoan), JsonRequestBehavior.AllowGet);
        }        
	}
}