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
        public ActionResult Xem(int? ma)
        {
            if (ma != null)
            {
                return View(NguoiDungBUS.layTheoMa(ma).ketQua);
            }
            return RedirectToAction("Index", "TrangChu");
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

        public ActionResult Sua(int? ma)
        {
            KetQua ketQua = NguoiDungBUS.layTheoMa(ma, new LienKet() { 
                "HinhDaiDien"
            });
            return View(ketQua.ketQua);
        }

        [HttpPost]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            return Json(NguoiDungBUS.capNhat(chuyenForm(formCollection)));
        }

        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(NguoiDungBUS.them(chuyenForm(formCollection)));
        }

        [HttpPost]
        public ActionResult XuLyDangNhap(FormCollection formCollection)
        {
            KetQua ketQua = NguoiDungBUS.xuLyDangNhap(chuyenForm(formCollection));

            return Json(ketQua);
        }

        public ActionResult XuLyDangXuat()
        {
            NguoiDungBUS.xuLyDangXuat();

            return RedirectToAction("DangNhap", "NguoiDung");
        }
        
        [HttpGet]
        public ActionResult KiemTraTenTaiKhoan(string tenTaiKhoan)
        {
            return Json(NguoiDungBUS.tonTaiTenTaiKhoan(tenTaiKhoan), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GoiY_QuanLyKhoaHoc(string input)
        {
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = new object[]
                { 
                    new 
                    {
                        ma = 1,
                        ten = "Lê Bình Chiêu"
                    },
                    new 
                    {
                        ma = 1,
                        ten = "Lê Bình Chiêu"
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }
	}
}