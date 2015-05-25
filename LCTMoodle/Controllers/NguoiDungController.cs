using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;

namespace LCTMoodle.Controllers
{
    public class NguoiDungController : LCTController
    {
        //
        // GET: /NguoiDung/
        public ActionResult Index()
        {
            return View(NguoiDungBUS.lay(Session["NguoiDung"] as string));
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
            Dictionary<string, string> form = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);
            KetQua ketQua = NguoiDungBUS.them(form);

            return Json(ketQua);
        }

        [HttpPost]
        public ActionResult XuLyKiemTraDangNhap(FormCollection formCollection)
        {
            Dictionary<string,string> form = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);
            KetQua ketQua = NguoiDungBUS.kiemTraDangNhap(form);

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
            KetQua ketQua = NguoiDungBUS.kiemTraTenTaiKhoan(tenTaiKhoan);
            
            return Json(ketQua, JsonRequestBehavior.AllowGet);
        }        
	}
}