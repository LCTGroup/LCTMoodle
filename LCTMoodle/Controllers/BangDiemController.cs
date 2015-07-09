using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTOLayer;
using BUSLayer;
using Newtonsoft.Json;

namespace LCTMoodle.Controllers
{
    public class BangDiemController : LCTController
    {
        public ActionResult Tao(int maKhoaHoc)
        {
            #region Kiểm tra quyền
            var ketQua = KhoaHocBUS.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Khóa học không tồn tại."));
            }
            var khoaHoc = ketQua.ketQua as KhoaHocDTO;

            if (!BUS.coQuyen("QLBangDiem", "KH", maKhoaHoc))
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần có quyền quản lý bảng điểm để thực hiện chức năng này."));
            }
            #endregion

            ViewData["KhoaHoc"] = khoaHoc;
            ketQua = CotDiemBUS.layTheoMaKhoaHoc(maKhoaHoc);

            return View(model: ketQua.trangThai == 0 ? ketQua.ketQua : null);
        }

        public ActionResult _Form(int maKhoaHoc)
        {
            ViewData["MaKhoaHoc"] = maKhoaHoc;

            return Json(
                new KetQua(renderPartialViewToString(ControllerContext, "BangDiem/_Form.cshtml", null, ViewData)),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Form_Sua(int ma)
        {
            #region Kiểm tra quyền
            //Lấy cột điểm
            var ketQua = CotDiemBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua(1), JsonRequestBehavior.AllowGet);
            }
            var cotDiem = ketQua.ketQua as CotDiemDTO;

            //Kiểm tra quyền
            if (!BUS.coQuyen("QLBangDiem", "KH", cotDiem.khoaHoc.ma.Value))
            {
                return Json(new KetQua(3, "Bạn không có quyền sửa cột điểm"), JsonRequestBehavior.AllowGet);
            }
            #endregion

            return Json
                (
                    new KetQua(renderPartialViewToString(ControllerContext, "BangDiem/_Form.cshtml", cotDiem)),
                    JsonRequestBehavior.AllowGet
                );
        }

        [HttpPost]
        public ActionResult XuLyThemCotDiem(FormCollection formCollection)
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

            var ketQua = CotDiemBUS.them(form);

            if (ketQua.trangThai == 0)
            {
                ketQua.ketQua = renderPartialViewToString(ControllerContext, "BangDiem/_Item.cshtml", ketQua.ketQua);
            }

            return Json(ketQua);
        }

        [HttpPost]
        public ActionResult XuLyCapNhatCotDiem(FormCollection formCollection)
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

            return Json(CotDiemBUS.capNhat(form));
        }

        [HttpPost]
        public ActionResult XuLyXoaCotDiem(int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }

            return Json(CotDiemBUS.xoaTheoMa(ma, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatThuTuCotDiem(int thuTuCu, int thuTuMoi, int maKhoaHoc)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }

            return Json(CotDiemBUS.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc, (int)Session["NguoiDung"]));
        }

        public ActionResult Xem(int maKhoaHoc)
        {
            #region Kiểm tra quyền
            #region Lấy khóa học
            KetQua ketQua = KhoaHocBUS.layTheoMa(maKhoaHoc);
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
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn đã bị chặn vào khóa học này."));
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

            ketQua = CotDiem_NguoiDungBUS.layTheoMaKhoaHoc(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Xem", "KhoaHoc", new { ma = maKhoaHoc });
            }

            object[] mangKetQua = ketQua.ketQua as object[];
            ViewData["CotDiem"] = mangKetQua[0];
            ViewData["NguoiDung"] = mangKetQua[1];
            ViewData["Diem"] = mangKetQua[2];

            return View(khoaHoc);
        }

        [HttpPost]
        public ActionResult XuLyCapNhatBangDiem(string jsonDiem)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }

            return Json(CotDiem_NguoiDungBUS.capNhat(JsonConvert.DeserializeObject<List<dynamic>>(jsonDiem), (int)Session["NguoiDung"]));
        }
	}
}