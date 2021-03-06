﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Data;
using Newtonsoft.Json;

namespace LCTMoodle.Controllers
{
    public class NguoiDungController : LCTController
    {

        #region View

        public ActionResult Xem(int ma)
        {
            #region Kiểm tra điều kiện

            int? maNguoiDung = Session["NguoiDung"] as int?;
            bool coQuyenXemKhoaHoc, coQuyenXemHoiDap;

            if (maNguoiDung.HasValue)
            {
                if (maNguoiDung == ma)
                {
                    coQuyenXemHoiDap = coQuyenXemKhoaHoc = true;
                }
                else
                {
                    coQuyenXemHoiDap = coQuyenXemKhoaHoc = false;
                }
            }
            else
            {
                coQuyenXemHoiDap = coQuyenXemKhoaHoc = false;
            }

            #endregion

            if (coQuyenXemHoiDap)
            {
                ViewData["DSLichSuCauHoi"] = null;

                string dsMa = CauHoiDAO.layCauHoi_DanhSachMaLienQuan(ma).ketQua as string;
                KetQua ketQua = HoatDongBUS.lay_CuaDanhSachDoiTuong("CH", dsMa, 1, 5, new LienKet() { "HanhDong" });
                if (ketQua.trangThai == 0)
                {
                    ViewData["DSLichSuCauHoi"] = ketQua.ketQua;
                }
            }
            return View(NguoiDungBUS.layTheoMa(ma, new LienKet() 
                { 
                    "HinhDaiDien"
                }).ketQua);
        }        

        public ActionResult DangNhap()
        {
            if (Session["NguoiDung"] != null)
            {
                return Redirect("/");
            }

            //Tắt hiển thị cột trái, cột phải
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;

            return View();
        }

        public ActionResult DangKy()
        {
            if (Session["NguoiDung"] != null)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng xuất trước khi đăng ký tài khoản mới."));
            }

            //Tắt hiển thị cột trái, cột phải
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;

            return View();
        }

        public ActionResult TinNhan(int? ma)
        {

            #region Kiểm tra điều kiện

            if (ma != Session["NguoiDung"] as int?)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn không có quyền xem tin nhắn người khác."));
            }

            #endregion

            var ketQua = TinNhanBUS.layDanhSachTinNhanTheoMaNguoiDung(ma.Value, new LienKet() { "NguoiDung" });
            if (ketQua.trangThai != 0 && ketQua.trangThai != 1)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Lấy tin nhắn lỗi."));
            }
            return View(ketQua.ketQua);
        }

        public ActionResult ChiTietTinNhan(string tenTaiKhoanKhach)
        {

            #region Kiểm tra điều kiện

            int? maNguoiDung = Session["NguoiDung"] as int?;
            if (!maNguoiDung.HasValue)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn chưa đăng nhập."));
            }

            #endregion

            var ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoanKhach);
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Người dùng không tồn tại."));
            }
            var khach = ketQua.ketQua as NguoiDungDTO;

            //Chuyển trạng thái tin nhắn sang đã đọc
            ketQua = TinNhanBUS.capNhatTrangThaiDaDocTinNhan(khach.ma.Value, maNguoiDung.Value, true);

            ketQua = TinNhanBUS.lay(khach.ma.Value, maNguoiDung.Value, new LienKet() { "NguoiDung" });
            if (ketQua.trangThai != 0 && ketQua.trangThai != 1)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Không lấy được ."));
            }

            ViewData["TenTaiKhoanKhach"] = tenTaiKhoanKhach;
            ViewData["MaNguoiDungKhach"] = khach.ma;

            return View(ketQua.ketQua);
        }

        public ActionResult Sua(int ma)
        {

            #region Kiểm tra điều kiện

            int? maNguoiDung = Session["NguoiDung"] as int?;
            bool coQuyenSuaThongTin = false;

            if (maNguoiDung.HasValue)
            {
                coQuyenSuaThongTin = maNguoiDung == ma;
            }

            #endregion

            if (coQuyenSuaThongTin)
            {
                KetQua ketQua = NguoiDungBUS.layTheoMa(ma, new LienKet() { 
                    "HinhDaiDien"
                });
                return View(ketQua.ketQua);
            }
            return Redirect("/NguoiDung/Xem/" + ma);
        }

        public ActionResult DoiMatKhau()
        {

            #region Kiểm tra điều kiện

            if (Session["NguoiDung"] == null)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn chưa đăng nhập."));
            }

            #endregion

            NguoiDungDTO nguoiDung = NguoiDungBUS.layTheoMa((int)Session["NguoiDung"]).ketQua as NguoiDungDTO;

            return View(nguoiDung);
        }

        public ActionResult QuenMatKhau()
        {
            ViewData["CotPhai"] = false;

            if (Session["NguoiDung"] != null)
            {
                return Redirect("/");
            }

            return View();
        }

        public ActionResult KichHoat(string tenTaiKhoan)
        {
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;

            #region Kiểm tra điều kiện

            if (Session["NguoiDung"] != null)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Tài khoản đang đăng nhập đã được kích hoạt. Để kích hoạt tài khoản khác xin vui lòng đăng xuất."));
            }

            #endregion

            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai == 0)
            {
                NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;
                if (nguoiDung.maKichHoat == null)
                {
                    return RedirectToAction("DangNhap", "NguoiDung");
                }
            }
            return View((object)tenTaiKhoan);
        }

        public ActionResult QuanLyNguoiDung()
        {

            #region Kiểm tra

            int? maNguoiDung = Session["NguoiDung"] as int?;
            bool coQuyenQuanLyNguoiDung = false;

            if (!maNguoiDung.HasValue)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để thực hiện chức năng này."));
            }
            else
            {
                coQuyenQuanLyNguoiDung = QuyenBUS.coQuyen("QLNguoiDung", "ND", 0, maNguoiDung);
            }

            if (!coQuyenQuanLyNguoiDung)
            {
                return Redirect("/NguoiDung/Xem/" + maNguoiDung);
            }

            #endregion

            var ketQua = NguoiDungDAO.lay();
            if (ketQua.trangThai != 0)
            {
                return Redirect("/NguoiDung/Xem/" + maNguoiDung);
            }

            List<NguoiDungDTO> nguoiDung = ketQua.ketQua as List<NguoiDungDTO>;

            return View(nguoiDung);
        }

        public ActionResult DanhSachNguoiDungBiChan()
        {

            #region Kiểm tra

            int? maNguoiDung = Session["NguoiDung"] as int?;
            bool coQuyenQuanLyNguoiDung = false;

            if (!maNguoiDung.HasValue)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để thực hiện chức năng này."));
            }
            else
            {
                coQuyenQuanLyNguoiDung = QuyenBUS.coQuyen("QLNguoiDung", "ND", 0, maNguoiDung);
            }

            if (!coQuyenQuanLyNguoiDung)
            {
                return Redirect("/NguoiDung/Xem/" + maNguoiDung);
            }

            #endregion

            var ketQua = NguoiDungDAO.layNguoiDungBiChan();
            if (ketQua.trangThai != 0 && ketQua.trangThai != 1)
            {
                return Redirect("/NguoiDung/Xem/" + maNguoiDung);
            }

            List<NguoiDungDTO> nguoiDung = ketQua.ketQua as List<NguoiDungDTO>;

            return View(nguoiDung);
        }

        #endregion

        #region Partial View

        public ActionResult _FormDangNhap()
        {
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "NguoiDung/_FormDangNhap.cshtml")
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DieuKhoan()
        {
            return Json(new KetQua(0, renderPartialViewToString(ControllerContext, "NguoiDung/_DieuKhoan.cshtml")), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DanhSachXacNhanThem(int maTapTin)
        {
            var ketQua = NguoiDungBUS.docTapTin_xls(Helpers.TapTinHelper.layDuongDan("Tam", maTapTin + ".xls"));
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }

            return Json(new KetQua(renderPartialViewToString(ControllerContext, "NguoiDung/_DanhSachXacNhanThem.cshtml", ketQua.ketQua)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _NhanTin()
        {
            return Json(new KetQua(0, renderPartialViewToString(ControllerContext, "NguoiDung/_NhanTin.cshtml")), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Xử lý

        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(NguoiDungBUS.them(chuyenForm(formCollection)));
        }

        [HttpPost]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            return Json(NguoiDungBUS.capNhat(chuyenForm(formCollection)));
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

        [HttpPost]
        public ActionResult XuLyChanNguoiDung(int maNguoiDung, bool trangThai)
        {
            return Json(NguoiDungBUS.chanNguoiDung(maNguoiDung, trangThai, Session["NguoiDung"] as int?));
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
        public ActionResult XuLyDoiMatKhau(FormCollection formCollection)
        {
            return Json(NguoiDungBUS.xuLyDoiMatKhau(chuyenForm(formCollection)));
        }

        [HttpPost]
        public ActionResult XuLyThemTinNhan(FormCollection form)
        {
            var ketQua = TinNhanBUS.them(chuyenForm(form), new LienKet() { "NguoiDung" });
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "/NguoiDung/_Item_TinNhan.cshtml", ketQua.ketQua)
            });
        }

        public ActionResult XuLyTaiHoatDongHoiDap(string loaiDTBiTacDong)
        {
            #region Kiểm tra điều kiện

            int? maNguoiDung = Session["NguoiDung"] as int?;
            if (!maNguoiDung.HasValue)
            {
                return Json(new KetQua(3, "Bạn chưa đăng nhập"));
            }

            #endregion

            KetQua ketQua = new KetQua();

            switch (loaiDTBiTacDong)
            {
                case "CH":
                    {
                        string dsMa = CauHoiBUS.layCauHoi_DanhSachMaLienQuan(maNguoiDung).ketQua as string;
                        ketQua = HoatDongBUS.lay_CuaDanhSachDoiTuong("CH", dsMa, 1, 5, new LienKet() { "HanhDong" });
                        break;
                    }

                case "TL":
                    {
                        string dsMa = TraLoiBUS.layTraLoi_DanhSachMaLienQuan(maNguoiDung).ketQua as string;
                        ketQua = HoatDongBUS.lay_CuaDanhSachDoiTuong("TL", dsMa, 1, 5, new LienKet() { "HanhDong" });
                        break;
                    }
                default:
                    return Json(new KetQua(1), JsonRequestBehavior.AllowGet);
            }

            if (ketQua.trangThai != 0 && ketQua.trangThai != 1)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }

            return Json(new KetQua(0, renderPartialViewToString(ControllerContext, "HoiDap/_LichSuCauHoi.cshtml", ketQua.ketQua)), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Kiểm tra

        public ActionResult KiemTraTenTaiKhoan(string tenTaiKhoan)
        {
            return Json(NguoiDungBUS.tonTaiTenTaiKhoan(tenTaiKhoan), JsonRequestBehavior.AllowGet);
        }

        public ActionResult KiemTraEmail(string email)
        {
            return Json(NguoiDungBUS.tonTaiEmail(email), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Điều hướng khác

        public ActionResult _GoiY_QuanLyKhoaHoc(string tuKhoa)
        {
            var ketQua = NguoiDungBUS.timKiem(tuKhoa);

            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua(1), JsonRequestBehavior.AllowGet);
            }

            List<object> dsNguoiDung = new List<object>();
            foreach (var nguoiDung in ketQua.ketQua as List<NguoiDungDTO>)
            {
                dsNguoiDung.Add(new
                {
                    ma = nguoiDung.ma.Value,
                    ten = nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten,
                    moTa = "Tài khoản: " + nguoiDung.tenTaiKhoan + (nguoiDung.ngaySinh.HasValue ? ("\r\nNgày sinh: " + nguoiDung.ngaySinh.Value.ToString("d/M/yyyy")) : null)
                });
            }


            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = dsNguoiDung
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GoiY_NhanTin(string tuKhoa)
        {
            var ketQua = NguoiDungBUS.timKiem(tuKhoa);

            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua(1), JsonRequestBehavior.AllowGet);
            }

            List<object> dsNguoiDung = new List<object>();
            foreach (var nguoiDung in ketQua.ketQua as List<NguoiDungDTO>)
            {
                dsNguoiDung.Add(new
                {
                    ma = nguoiDung.tenTaiKhoan,
                    ten = nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten,
                    moTa = "Tài khoản: " + nguoiDung.tenTaiKhoan + (nguoiDung.ngaySinh.HasValue ? ("\r\nNgày sinh: " + nguoiDung.ngaySinh.Value.ToString("d/M/yyyy")) : null)
                });
            }


            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = dsNguoiDung
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
	}
}