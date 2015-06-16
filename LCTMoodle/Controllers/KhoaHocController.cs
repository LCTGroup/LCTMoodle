using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DAOLayer;
using DTOLayer;
using Data;

namespace LCTMoodle.Controllers
{
    public class KhoaHocController : LCTController
    {
        public ActionResult Xem(int ma)
        {
            //Lấy khóa học
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            KhoaHocDTO khoaHoc = ketQua.ketQua as KhoaHocDTO;

            //Lấy thành viên
            KhoaHoc_NguoiDungDTO thanhVien = null;
            if (Session["NguoiDung"] != null)
            {
                ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaMaNguoiDung(khoaHoc.ma.Value, (int)Session["NguoiDung"]);
                thanhVien = ketQua.trangThai == 0 ? ketQua.ketQua as KhoaHoc_NguoiDungDTO : null;
            }

            //Thành viên bị chặn
            if (thanhVien != null && thanhVien.trangThai == 3)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            ViewData["ThanhVien"] = thanhVien;

            //Khóa học nội bộ & người dùng không phải là thành viên
            if (
                (khoaHoc.cheDoRiengTu == "NoiBo" && 
                (thanhVien == null || thanhVien.trangThai != 0)) ||
                thanhVien != null && thanhVien.trangThai == 3)
            {
                return View("DangKyThamGia", khoaHoc);
            }

            return View(khoaHoc);
        }

        public ActionResult Tao()
        {
            return View();
        }       

        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(KhoaHocBUS.them(chuyenDuLieuForm(formCollection)));
        }

        public ActionResult _Khung(int ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);

            if (ketQua.trangThai == 0)
            {
                var khoaHoc = ketQua.ketQua;

                ketQua = DAOLayer.BaiVietBaiGiangDAO.layTheoMaKhoaHoc(ma);
                ViewData["BaiGiang"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

                ketQua = DAOLayer.BaiVietBaiTapDAO.layTheoMaKhoaHoc(ma);
                ViewData["BaiTap"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

                ketQua = DAOLayer.BaiVietDienDanDAO.layTheoMaKhoaHoc(ma);
                ViewData["DienDan"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "KhoaHoc/_Khung.cshtml", khoaHoc, ViewData)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ketQua);
            }
        }

        [HttpPost]
        public ActionResult XuLyDangKyThamGia(int ma)
        {
            return Json(KhoaHoc_NguoiDungBUS.dangKyThamGia(ma));
        }

        [HttpPost]
        public ActionResult XuLyHuyDangKy(int ma)
        {
            return Json(KhoaHoc_NguoiDungBUS.huyDangKy(ma));
        }
	}
}