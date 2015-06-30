using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using BUSLayer;
using DAOLayer;
using DTOLayer;

namespace LCTMoodle.Controllers
{
    public class ChuongTrinhController : LCTController
    {
        public ActionResult Index(int maKhoaHoc)
        {
            #region Kiểm tra quyền
            #region Lấy khóa học
            KetQua ketQua = KhoaHocBUS.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return Redirect("/");
            }
            var khoaHoc = ketQua.ketQua as KhoaHocDTO;
            #endregion

            #region Lấy thành viên
            KhoaHoc_NguoiDungDTO thanhVien = null;
            if (Session["NguoiDung"] != null)
            {
                ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaMaNguoiDung(khoaHoc.ma.Value, (int)Session["NguoiDung"]);
                thanhVien = ketQua.trangThai == 0 ? ketQua.ketQua as KhoaHoc_NguoiDungDTO : null;
            }
            #endregion

            #region Kiểm tra nếu thành viên bị chặn
            if (thanhVien != null && thanhVien.trangThai == 3)
            {
                return Redirect("/");
            }
            #endregion

            #region Kiểm tra trường hợp khóa học nội bộ
            if (
                    (khoaHoc.cheDoRiengTu == "NoiBo" &&
                    (thanhVien == null || thanhVien.trangThai != 0)) ||
                    thanhVien != null && thanhVien.trangThai == 3)
            {
                ViewData["ThanhVien"] = thanhVien;
                return View("DangKyThamGia", khoaHoc);
            }
            #endregion
            #endregion

            ViewData["KhoaHoc"] = khoaHoc;

            ketQua = ChuongTrinhBUS.layTheoMaKhoaHoc(maKhoaHoc);

            return View(model: ketQua.trangThai == 0 ? ketQua.ketQua : null);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                    {
                        trangThai = 4
                    });
            }
            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiTao", Session["NguoiDung"].ToString());

            KetQua ketQua = ChuongTrinhBUS.them(form);

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "ChuongTrinh/_Item.cshtml", ketQua.ketQua)
                });
            }
            else
            {
                return Json(ketQua);   
            }
        }

        [HttpPost]
        public ActionResult XuLyXoa(int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }
            return Json(ChuongTrinhBUS.xoaTheoMa(ma, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatThuTu(int thuTuCu, int thuTuMoi, int maKhoaHoc)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }
            return Json(ChuongTrinhBUS.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc, (int)Session["NguoiDung"]));
        }
	}
}