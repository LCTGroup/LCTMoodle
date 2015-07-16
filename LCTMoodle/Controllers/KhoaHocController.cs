using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DAOLayer;
using DTOLayer;
using Data;
using Helpers;

namespace LCTMoodle.Controllers
{
    public class KhoaHocController : LCTController
    {
        public ActionResult Xem(int ma)
        {
            #region Lấy khóa học
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Khóa học không tồn tại."));
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
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn đã bị chặn."));
            } 
            #endregion

            ViewData["ThanhVien"] = thanhVien;

            #region Kiểm tra trường hợp khóa học nội bộ
            if (
                    (khoaHoc.cheDoRiengTu == "NoiBo" &&
                    (thanhVien == null || thanhVien.trangThai != 0)) ||
                    thanhVien != null && thanhVien.trangThai == 3)
            {
                return View("DangKyThamGia", khoaHoc);
            } 
            #endregion

            return View(khoaHoc);
        }

        public ActionResult ThongTin(int ma)
        {
            #region Kiểm tra điều kiện
            //Lấy khóa học
            var ketQua = KhoaHocBUS.layTheoMa(ma, new LienKet() { "GiangVien", "ChuDe" });
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Khóa học không tồn tại"));
            }
            var khoaHoc = ketQua.ketQua as KhoaHocDTO;
            #endregion

            return View(khoaHoc);
        }

        public ActionResult Tao(int ma = 0)
        {
            if (Session["NguoiDung"] == null)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Cần đăng nhập để sử dụng chức năng này."));
            }
            int maNguoiDung = (int)Session["NguoiDung"];

            KetQua ketQua;
            #region Tạo
            if (ma == 0)
            {
                //Lấy chủ đề mà người dùng có thể tạo
                //Ở hệ thống
                ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("HT", maNguoiDung, "KH", "QLNoiDung");
                if (ketQua.trangThai != 0)
                {
                    //Ở chủ đề
                    ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("CD", maNguoiDung, "KH", "QLNoiDung");

                    if (ketQua.trangThai != 0)
                    {
                        //Nếu không tìm thấy ở 2 phạm vi tren => ko có quyền
                        return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn không có quyền tạo khóa học."));
                    }
                }

                ViewData["ChuDe"] = ketQua.ketQua as string;

                return View();
            } 
            #endregion

            #region Sửa
            ketQua = KhoaHocBUS.layTheoMa(ma, new LienKet()
                {
                    "ChuDe"
                });
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Khóa học không tồn tại"));
            }
            var khoaHoc = ketQua.ketQua as KhoaHocDTO;

            //Lấy chủ đề mà người dùng có thể sửa
            //Ở hệ thống
            ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("HT", maNguoiDung, "KH", "QLNoiDung");
            if (ketQua.trangThai != 0)
            {
                //Ở chủ đề
                ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("CD", maNguoiDung, "KH", "QLNoiDung");

                if (ketQua.trangThai != 0)
                {
                    //Nếu không tìm thấy ở 2 phạm vi trên => ko có quyền
                    if (!BUS.coQuyen("QLThongTin", "KH", khoaHoc.ma.Value, maNguoiDung))
                    {
                        return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn không có quyền sửa khóa học."));
                    }
                    return View(khoaHoc);
                }
            }
            string chuoiMaChuDe = ketQua.ketQua as string;

            //Kiểm tra người dùng có thể sửa khóa học này không
            foreach(var maChuDe in chuoiMaChuDe.Split('|').Select(int.Parse))
            {
                ketQua = ChuDeBUS.thuocCay(khoaHoc.chuDe, maChuDe);
                if (ketQua.trangThai == 0 && (bool)ketQua.ketQua)
                {
                    //Trường hợp có quyền tạo trên chủ đề
                    ViewData["ChuDe"] = chuoiMaChuDe;
                    return View(khoaHoc);
                }
            }

            if (!BUS.coQuyen("QLThongTin", "KH", khoaHoc.ma.Value, maNguoiDung))
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn không có quyền sửa khóa học."));
            }
            return View(khoaHoc);
            #endregion
        }       

        public ActionResult DanhSach()
        {
            var ketQua = KhoaHocBUS.timKiemPhanTrang(1, Data.GiaTri.soLuongKhoaHocMoiTrang, null, null, new LienKet() { "GiangVien" });

            return View(ketQua.trangThai == 0 ? ketQua.ketQua : null);
        }

        public ActionResult DanhSachCuaToi()
        {
            //Nếu người dùng chưa đăng nhập => chuyển về trang danh sách
            if (Session["NguoiDung"] == null)
            {
                return RedirectToAction("DanhSach", "KhoaHoc");
            } 

            //Lấy danh sách khóa học của người dùng
            KetQua ketQua = KhoaHocBUS.layTheoMaNguoiDung((int)Session["NguoiDung"], new LienKet() { "GiangVien" });
            if (ketQua.trangThai > 1)
            {
                //Nếu lấy thất bại => chuyển về trang danh sách
                return RedirectToAction("DanhSach", "KhoaHoc");
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

        public ActionResult ThanhVien(int ma)
        {
            #region Lấy khóa học
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Khóa học không tồn tại."));
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
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn đã bị chặn khỏi khóa học."));
            }
            #endregion

            ViewData["ThanhVien"] = thanhVien;

            #region Kiểm tra trường hợp khóa học nội bộ
            if (
                    (khoaHoc.cheDoRiengTu == "NoiBo" &&
                    (thanhVien == null || thanhVien.trangThai != 0)) ||
                    thanhVien != null && thanhVien.trangThai == 3)
            {
                return View("DangKyThamGia", khoaHoc);
            }
            #endregion

            #region Lấy danh sách thành viên của nhóm
            ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaTrangThai(ma, 0, new LienKet() { "NguoiDung" });
            if (ketQua.trangThai == 0)
            {
                ViewData["ThanhVien"] = ketQua.ketQua;
            }
            #endregion

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

            #region Lấy danh sách chặn
            ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaTrangThai(ma, 3, new LienKet() { "NguoiDung" });
            if (ketQua.trangThai == 0)
            {
                ViewData["DanhSachBiChan"] = ketQua.ketQua;
            }
            #endregion

            return View(khoaHoc);
        }

        public ActionResult ThemThanhVien(int ma)
        {
            #region Kiểm tra điều kiện
            if (Session["NguoiDung"] == null)
            {
                return Redirect("/NguoiDung/DangNhap/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để sử dụng chức năng này"));
            }

            var ketQua = KhoaHocBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Khóa học không tồn tại"));
            }
            var khoaHoc = ketQua.ketQua as KhoaHocDTO;

            if (!BUS.coQuyen("QLThanhVien", "KH", ma, Session["NguoiDung"] as int?))
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần có quyền quản lý thành viên để thực hiện chức năng này"));
            }
            #endregion

            return View(khoaHoc);
        }

        public ActionResult _Khung(int ma)
        {
            #region Kiểm tra quyền
            #region Lấy khóa học
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Khóa học không tồn tại"
                }, JsonRequestBehavior.AllowGet);
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
                return Json(new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bạn đã bị chặn"
                }, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region Kiểm tra trường hợp khóa học nội bộ
            if (
                    (khoaHoc.cheDoRiengTu == "NoiBo" &&
                    (thanhVien == null || thanhVien.trangThai != 0)) ||
                    thanhVien != null && thanhVien.trangThai == 3)
            {
                return Json(new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Đây là khóa học nội bộ, bạn cần tham gia để xem nội dung"
                }, JsonRequestBehavior.AllowGet);
            }
            #endregion
            #endregion

            #region Lấy dữ liệu bài viết
            ketQua = DAOLayer.BaiVietBaiGiangDAO.layTheoMaKhoaHoc(ma, new LienKet() { "NguoiTao" });
            ViewData["BaiGiang"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

            ketQua = DAOLayer.BaiVietBaiTapDAO.layTheoMaKhoaHoc(ma, new LienKet() { "NguoiTao" });
            ViewData["BaiTap"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

            ketQua = DAOLayer.BaiVietDienDanDAO.layTheoMaKhoaHoc(ma, new LienKet() { "NguoiTao" });
            ViewData["DienDan"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "KhoaHoc/_Khung.cshtml", khoaHoc, ViewData)
            }, JsonRequestBehavior.AllowGet);
            #endregion
        }
        
        public ActionResult _DanhSach_Tim(string tuKhoa = "", int maChuDe = 0, int trang = 1)
        {
            //Sửa số dòng mỗi trang nhớ sửa ở view
            var ketQua = KhoaHocBUS.timKiemPhanTrang(trang, Data.GiaTri.soLuongKhoaHocMoiTrang,
                "Ten LIKE '%" + tuKhoa + "%'" +
                (maChuDe != 0 ?
                "AND MaChuDe = " + maChuDe :
                null),
                null,
                new LienKet() { "GiangVien" });

            if (ketQua.trangThai == 0)
            {
                ketQua.ketQua = new
                    {
                        danhSach = renderPartialViewToString(ControllerContext, "KhoaHoc/_DanhSach.cshtml", ketQua.ketQua),
                        phanTrang = renderPartialViewToString(ControllerContext, "LCT/_PhanTrang.cshtml", null, new ViewDataDictionary()
                        {
                            { "TongSoLuong", LCTHelper.layGiaTri<int>((ketQua.ketQua as List<KhoaHocDTO>)[0].duLieuThem, "TongSoDong", 0) },
                            { "SoLuongMoiTrang", Data.GiaTri.soLuongKhoaHocMoiTrang }
                        })
                    };
            }

            return Json(ketQua, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiTao", Session["NguoiDung"].ToString());

            return Json(KhoaHocBUS.them(form));
        }

        [HttpPost]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiSua", Session["NguoiDung"].ToString());

            return Json(KhoaHocBUS.capNhat(form));
        }

        [HttpPost]
        public ActionResult XuLyDangKyThamGia(int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            return Json(KhoaHoc_NguoiDungBUS.dangKyThamGia(ma, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyHuyDangKy(int ma)
        {
            return Json(KhoaHoc_NguoiDungBUS.huyDangKy(ma));
        }

        [HttpPost]
        public ActionResult XuLyChapNhanDangKy(int ma, int maNguoiDung)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(KhoaHoc_NguoiDungBUS.chapNhanDangKy(ma, maNguoiDung, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyTuChoiDangKy(int ma, int maNguoiDung)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(KhoaHoc_NguoiDungBUS.tuChoiDangKy(ma, maNguoiDung, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyChanNguoiDung(int ma, int maNguoiDung)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(KhoaHoc_NguoiDungBUS.chanNguoiDung(ma, maNguoiDung, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyHuyChanNguoiDung(int ma, int maNguoiDung)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(KhoaHoc_NguoiDungBUS.huyChanNguoiDung(ma, maNguoiDung, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyXoaThanhVien(int ma, int maNguoiDung)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(KhoaHoc_NguoiDungBUS.xoaThanhVien(ma, maNguoiDung, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyRoiKhoaHoc(int ma)
        {
            return Json(KhoaHoc_NguoiDungBUS.roiKhoaHoc(ma));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatHocVien(int ma, int maNguoiDung, bool laHocVien)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(KhoaHoc_NguoiDungBUS.capNhatHocVien(ma, maNguoiDung, laHocVien, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyXoa(int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(KhoaHocBUS.xoaTheoMa(ma, (int)Session["NguoiDung"]));
        }
	}
}