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

        public ActionResult DanhSach()
        {
            KetQua ketQua = KhoaHocBUS.lay();

            return View(ketQua.trangThai == 0 ? ketQua.ketQua : null);
        }

        public ActionResult DanhSachCuaToi()
        {
            if (Session["NguoiDung"] == null)
            {
                return RedirectToAction("DanhSach", "KhoaHoc");
            }

            KetQua ketQua = KhoaHocBUS.layTheoMaNguoiDung((int)Session["NguoiDung"]);
            if (ketQua.trangThai > 1)
            {
                RedirectToAction("Index", "NguoiDung");
            }
            if (ketQua.trangThai == 0)
            {
                var danhSachKhoaHoc = ketQua.ketQua as List<KhoaHocDTO>[];

                ViewData["ThamGia"] = danhSachKhoaHoc[0];
                ViewData["DangKy"] = danhSachKhoaHoc[1];
                ViewData["DuocMoi"] = danhSachKhoaHoc[2];
                ViewData["BiChan"] = danhSachKhoaHoc[3];
            }

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

        public ActionResult DanhSachThanhVien(int ma)
        {
            //Lấy khóa học & kiểm tra tồn tại hay không
            #region Lấy khóa học & kiểm tra tồn tại
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            KhoaHocDTO khoaHoc = ketQua.ketQua as KhoaHocDTO; 
            #endregion

            //Lấy danh sách thành viên của nhóm
            #region Lấy danh sách thành viên của nhóm
            ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaTrangThai(ma, 0, new LienKet() { "NguoiDung" });
            if (ketQua.trangThai == 0)
            {
                ViewData["ThanhVien"] = ketQua.ketQua;
            }
	        #endregion

            //Lấy danh sách người dùng đăng ký
            #region Lấy danh sách người dùng đăng ký
            if (khoaHoc.canDangKy)
            {
                ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaTrangThai(ma, 1, new LienKet() { "NguoiDung" });
                if (ketQua.trangThai == 0)
                {
                    ViewData["DanhSachDangKy"] = ketQua.ketQua;
                }
            }
            #endregion

            //Lấy danh sách người dùng bị chặn
            #region Lấy danh sách chặn
            ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaTrangThai(ma, 3, new LienKet() { "NguoiDung" });
            if (ketQua.trangThai == 0)
            {
                ViewData["DanhSachBiChan"] = ketQua.ketQua;
            }
            #endregion

            return View(khoaHoc);
        }

        [HttpPost]
        public ActionResult XuLyChapNhanDangKy(int ma, int maNguoiDung)
        {
            return Json(KhoaHoc_NguoiDungBUS.chapNhanDangKy(ma, maNguoiDung));
        }

        [HttpPost]
        public ActionResult XuLyTuChoiDangKy(int ma, int maNguoiDung)
        {
            return Json(KhoaHoc_NguoiDungBUS.tuChoiDangKy(ma, maNguoiDung));
        }

        [HttpPost]
        public ActionResult XuLyChanNguoiDung(int ma, int maNguoiDung)
        {
            return Json(KhoaHoc_NguoiDungBUS.chanNguoiDung(ma, maNguoiDung));
        }

        [HttpPost]
        public ActionResult XuLyHuyChanNguoiDung(int ma, int maNguoiDung)
        {
            return Json(KhoaHoc_NguoiDungBUS.huyChanNguoiDung(ma, maNguoiDung));
        }

        [HttpPost]
        public ActionResult XuLyXoaThanhVien(int ma, int maNguoiDung)
        {
            return Json(KhoaHoc_NguoiDungBUS.xoaThanhVien(ma, maNguoiDung));
        }

        [HttpPost]
        public ActionResult XuLyRoiKhoaHoc(int ma)
        {
            return Json(KhoaHoc_NguoiDungBUS.roiKhoaHoc(ma));
        }
	}
}