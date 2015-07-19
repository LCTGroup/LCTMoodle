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
    public class BaiVietDienDanController : LCTController
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

            List<BaiVietDienDanDTO> danhSachBaiViet;

            if (ma == 0)
            {
                ketQua = BaiVietDienDanBUS.layTheoMaKhoaHoc(maKhoaHoc, new LienKet() { "TapTin", "NguoiTao" });
                danhSachBaiViet =
                    ketQua.trangThai == 0 ?
                    ketQua.ketQua as List<BaiVietDienDanDTO> :
                    null;
            }
            else
            {
                ketQua = BaiVietDienDanBUS.layTheoMa(ma, new LienKet() { "TapTin", "NguoiTao" });
                danhSachBaiViet =
                    ketQua.trangThai == 0 ?
                    new List<BaiVietDienDanDTO>() { ketQua.ketQua as BaiVietDienDanDTO } :
                    null;
            }
            
            ViewData["MaKhoaHoc"] = maKhoaHoc;

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua =
                    renderPartialViewToString(ControllerContext, "BaiVietDienDan/_Khung.cshtml", danhSachBaiViet, ViewData)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Form(int ma = 0)
        {
            KetQua ketQua = BaiVietDienDanBUS.layTheoMa(ma);

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
                        renderPartialViewToString(ControllerContext, "BaiVietDienDan/_Form.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiTao", Session["NguoiDung"].ToString());
            KetQua ketQua = BaiVietDienDanBUS.them(form, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BaiVietDienDan/_Item.cshtml", ketQua.ketQua)
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

            return Json(BaiVietDienDanBUS.xoaTheoMa(ma, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiSua", Session["NguoiDung"].ToString());

            KetQua ketQua = BaiVietDienDanBUS.capNhatTheoMa(form, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "BaiVietDienDan/_Item.cshtml", ketQua.ketQua)
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
        public ActionResult XuLyGhim(int ma, bool ghim)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }

            return Json(BaiVietDienDanBUS.ghim(ma, ghim, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyChoDiem(int ma, int diem)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(BaiVietDienDanBUS.capNhatDiem(ma, diem, (int)Session["NguoiDung"]));
        }
	}
}