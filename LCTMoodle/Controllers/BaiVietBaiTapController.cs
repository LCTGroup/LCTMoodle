﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using Data;

namespace LCTMoodle.Controllers
{
    public class BaiVietBaiTapController : LCTController
    {
        public ActionResult _Khung(int maKhoaHoc)
        {
            #region Kiểm tra quyền
            #region Lấy khóa học
            KetQua ketQua = KhoaHocBUS.layTheoMa(maKhoaHoc);
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

            ketQua = BaiVietBaiTapBUS.layTheoMaKhoaHoc(maKhoaHoc);
            List<BaiVietBaiTapDTO> danhSachBaiViet = 
                ketQua.trangThai == 0 ?
                ketQua.ketQua as List<BaiVietBaiTapDTO> :
                null;

            ViewData["MaKhoaHoc"] = maKhoaHoc;

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua =
                        renderPartialViewToString(ControllerContext, "BaiVietBaiTap/_Khung.cshtml", danhSachBaiViet, ViewData)
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Form(int ma)
        {
            #region Kiểm tra điều kiện
            //Kiểm tra người dùng
            var maNguoiDung = Session["NguoiDung"] as int?;
            if (!maNguoiDung.HasValue)
            {
                return Json(new KetQua(4), JsonRequestBehavior.AllowGet);
            }

            //Lấy bài tập
            var ketQua = BaiVietBaiTapBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua(1, "Bài tập không tồn tại"), JsonRequestBehavior.AllowGet);
            }
            var baiTap = ketQua.ketQua as BaiVietBaiTapDTO;

            //Kiểm tra quyền
            if (maNguoiDung == baiTap.nguoiTao.ma && !BUS.coQuyen("BT_Sua", "KH", baiTap.khoaHoc.ma.Value, maNguoiDung))
            {
                return Json(new KetQua(3, "Bạn không có quyền sửa"));
            }
            #endregion

            if (baiTap.loai != 0)
            {
                //Lấy cột điểm
                ketQua = CotDiemBUS.layTheoLoaiDoiTuongVaMaDoiTuong("BaiTap", baiTap.ma.Value);
                if (ketQua.trangThai == 0)
                {
                    ViewData["CotDiem"] = ketQua.ketQua;
                }
            }

            ViewData["MaKhoaHoc"] = baiTap.khoaHoc.ma.Value;
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua =
                    renderPartialViewToString(ControllerContext, "BaiVietBaiTap/_Form.cshtml", baiTap, ViewData)
            }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            var form = chuyenForm(formCollection);
            form.Add("MaNguoiTao", Session["NguoiDung"].ToString());

            var ketQua = BaiVietBaiTapBUS.them(form);

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BaiVietBaiTap/_Item.cshtml", ketQua.ketQua)
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
            return Json(
                BaiVietBaiTapBUS.xoaTheoMa(ma),
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyCapNhat(FormCollection form)
        {
            KetQua ketQua = BaiVietBaiTapBUS.capNhatTheoMa(chuyenForm(form));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "BaiVietBaiTap/_Item.cshtml", ketQua.ketQua)
                });
            }
            else
            {
                return Json
                (
                    ketQua
                );
            }
        }

        public ActionResult ChamDiem(int ma)
        {
            #region Kiểm tra điều kiện
            var ketQua = BaiVietBaiTapBUS.layTheoMa(ma, new LienKet()
                {
                    { 
                        "BaiTapNop", 
                        new LienKet()
                        {
                            "NguoiTao",
                            "TapTin"
                        }
                    },
                    "KhoaHoc"
                });
            if (ketQua.trangThai != 0)
            {
                return Redirect("/");
            }

            var baiTap = ketQua.ketQua as BaiVietBaiTapDTO;

            if (baiTap.loai != 1 && baiTap.loai != 2)
            {
                return Redirect("/");
            }

            //Quản lý quyền
            if (!BUS.coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value))
            {
                return Redirect("/");
            }
            if (!BUS.coQuyen("QLBangDiem", "KH", baiTap.khoaHoc.ma.Value))
            {
                return Redirect("/");
            }
            #endregion

            return View(baiTap);
        }

        [HttpPost]
        public ActionResult XuLyChuyenDiem(int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(CotDiem_NguoiDungBUS.chuyenDiemBaiTapNop(ma, (int)Session["NguoiDung"]));
        }
	}
}