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
    public class BaiTapNopController : LCTController
    {
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            Form form = chuyenForm(formCollection);
            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", ((int)Session["NguoiDung"]).ToString());
            }

            return Json(BaiTapNopBUS.themHoacCapNhat(form, new LienKet() { "TapTin" }));
        }

        public ActionResult _DanhSachNop(int maBaiTap)
        {
            #region Kiểm tra quyền
            //Lấy bài tập
            var ketQua = BaiVietBaiTapBUS.layTheoMa(maBaiTap);
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài tập không tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }

            var baiTap = ketQua.ketQua as BaiVietBaiTapDTO;
            if (!BUS.coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value))
            {
                return Json(new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xem bài nộp"
                }, JsonRequestBehavior.AllowGet);
            }
            #endregion

            ketQua = BaiTapNopBUS.layTheoMaBaiVietBaiTap(maBaiTap, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
            List<BaiTapNopDTO> danhSachBaiTapNop =
                ketQua.trangThai == 0 ?
                    ketQua.ketQua as List<BaiTapNopDTO> :
                    null;

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "BaiTapNop/_DanhSachNop.cshtml", danhSachBaiTapNop)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Xem(int ma)
        {
            #region MyRegion
            //Lấy bài nộp, bài tập
            var ketQua = BaiTapNopBUS.layTheoMa(ma, new LienKet() { "BaiVietBaiTap", "NguoiTao", "TapTin" });
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua(1, "Bài nộp không tồn tại"), JsonRequestBehavior.AllowGet);
            }
            var baiNop = ketQua.ketQua as BaiTapNopDTO;

            //Kiểm tra quyền
            if (!BUS.coQuyen("BT_QLBaiNop", "KH", baiNop.baiVietBaiTap.khoaHoc.ma.Value))
            {
                return Json(new KetQua(3, "Bạn không có quyền xem bài nộp"), JsonRequestBehavior.AllowGet);
            }
            #endregion

            return Json(new KetQua(renderPartialViewToString(ControllerContext, "BaiTapNop/_Xem.cshtml", baiNop)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyChamDiem(int ma, double diem)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            return Json(BaiTapNopBUS.chamDiem(ma, diem, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyGhiChu(int ma, string ghiChu)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            return Json(BaiTapNopBUS.ghiChu(ma, ghiChu, (int)Session["NguoiDung"]));
        }

        public ActionResult XuLyXoa_Mot(int ma, string lyDo)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            return Json(BaiTapNopBUS.xoa(ma, lyDo, (int)Session["NguoiDung"]));
        }

        public ActionResult XuLyXoa_Nhieu(string dsMa, string lyDo)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            return Json(BaiTapNopBUS.xoa(dsMa, lyDo, (int)Session["NguoiDung"]));
        }

        public ActionResult LayDanhSachFile(string ds)
        {
            if (Session["NguoiDung"] == null)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để sử dụng chức năng này"));
            }

            var ketQua = BaiTapNopBUS.nen(ds, (int)Session["NguoiDung"]);
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Có lỗi xảy ra khi lấy danh sách tập tin"));
            }

            string[] thongTin = ketQua.ketQua as string[];
            return File(thongTin[0], thongTin[1], thongTin[2]);
        }
	}
}