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
        public ActionResult Xem(int? ma)
        {
            if (ma != null)
            {
                KetQua ketQua = CauHoiDAO.layTheoMaNguoiTao(ma, new LienKet() { 
                    "NguoiTao",
                    "HinhDaiDien"
                });
                if (ketQua.trangThai == 0)
                {
                    ViewData["DanhSachCauHoi"] = ketQua.ketQua as List<CauHoiDTO>;
                }
                else
                {
                    ViewData["DanhSachCauHoi"] = null;
                }
                return View(NguoiDungBUS.layTheoMa(ma).ketQua);
            }
            return RedirectToAction("Index", "TrangChu");
        }

        public ActionResult DangNhap()
        {
            //Tắt hiển thị cột trái, cột phải
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;                        

            return View();
        }

        public ActionResult _FormDangNhap()
        {
            return Json(new KetQua() { 
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "NguoiDung/_FormDangNhap.cshtml")
            }, JsonRequestBehavior.AllowGet);
        }

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

        public ActionResult DoiMatKhau()
        {
            NguoiDungDTO nguoiDung = NguoiDungBUS.layTheoMa((int?)Session["NguoiDung"]).ketQua as NguoiDungDTO;
            
            return View(nguoiDung); 
        }

        public ActionResult QuenMatKhau()
        {
            ViewData["CotPhai"] = false;

            return View();
        }
        
        public ActionResult KichHoat(string tenTaiKhoan)
        {
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;

            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoan);  
            if (ketQua.trangThai == 0)
            {
                NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;
                if (nguoiDung.maKichHoat == null)
                {
                    return RedirectToAction("DangNhap", "NguoiDung");
                }
            }
            return View(ketQua.ketQua);
        }

        [HttpPost]
        public ActionResult XuLyPhucHoiMatKhau(FormCollection form)
        {
            return Json(NguoiDungBUS.phucHoiMatKhau(chuyenForm(form)));
        }

        [HttpPost]
        public ActionResult XuLyKichHoatTaiKhoan(FormCollection form)
        {
            return Json(NguoiDungBUS.kichHoatTaiKhoan(chuyenForm(form)));
        }

        [HttpPost]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            return Json(NguoiDungBUS.capNhat(chuyenForm(formCollection)));
        }

        [HttpPost]
        public ActionResult XuLyDoiMatKhau(FormCollection formCollection)
        {            
            return Json(NguoiDungBUS.xuLyDoiMatKhau(chuyenForm(formCollection)));
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