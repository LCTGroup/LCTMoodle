﻿using System;
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
    public class BaiVietBaiGiangController : LCTController
    {
        public ActionResult _Khung(int maKhoaHoc, int ma = 0)
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

            List<BaiVietBaiGiangDTO> danhSachBaiViet;
            if (ma == 0)
            {
                ketQua = BaiVietBaiGiangBUS.layTheoMaKhoaHoc(maKhoaHoc, new LienKet() { "TapTin" });
                danhSachBaiViet =
                    ketQua.trangThai == 0 ?
                    ketQua.ketQua as List<BaiVietBaiGiangDTO> :
                    null;
            }
            else
            {
                ketQua = BaiVietBaiGiangBUS.layTheoMa(ma, new LienKet() { "TapTin" });
                danhSachBaiViet =
                    ketQua.trangThai == 0 ?
                    new List<BaiVietBaiGiangDTO>() { ketQua.ketQua as BaiVietBaiGiangDTO } :
                    null;
            }

            ViewData["MaKhoaHoc"] = maKhoaHoc;

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua =
                    renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Khung.cshtml", danhSachBaiViet, ViewData)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Form(int ma = 0)
        {
            KetQua ketQua = BaiVietBaiGiangBUS.layTheoMa(ma);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua =
                        renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Form.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
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

            KetQua ketQua = BaiVietBaiGiangBUS.them(form, new LienKet()
            {
                "TapTin"
            });

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Item.cshtml", ketQua.ketQua)
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

            return Json(BaiVietBaiGiangBUS.xoaTheoMa(ma, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                    {
                        trangThai = 4
                    });
            }

            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiSua", Session["NguoiDung"].ToString());

            KetQua ketQua = BaiVietBaiGiangBUS.capNhatTheoMa(form, new LienKet()
            {
                "TapTin"
            });

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Item.cshtml", ketQua.ketQua)
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

        [HttpPost]
        public ActionResult XuLyCapNhatDaXem(int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            return Json(BaiVietBaiGiangBUS.capNhatTheoMa_Xem(ma, (int)Session["NguoiDung"]));
        }
	}
}