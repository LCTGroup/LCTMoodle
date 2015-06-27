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
        public ActionResult Tao(int ma) //Để tạm, sau này sửa route, đổi thành mã khóa học
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);

            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            ViewData["KhoaHoc"] = ketQua.ketQua as KhoaHocDTO;

            ketQua = CotDiemBUS.layTheoMaKhoaHoc(ma);

            return View(model: ketQua.trangThai == 0 ? ketQua.ketQua : null);
        }

        [HttpPost]
        public ActionResult XuLyThemCotDiem(FormCollection form)
        {
            KetQua ketQua = CotDiemBUS.them(chuyenDuLieuForm(form));

            if (ketQua.trangThai == 0)
            {
                ketQua.ketQua = renderPartialViewToString(ControllerContext, "BangDiem/_Item.cshtml", ketQua.ketQua);
            }

            return Json(ketQua);
        }

        [HttpPost]
        public ActionResult XuLyXoaCotDiem(int ma)
        {
            return Json(CotDiemBUS.xoaTheoMa(ma));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatThuTuCotDiem(int thuTuCu, int thuTuMoi, int maKhoaHoc)
        {
            return Json(CotDiemBUS.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc));
        }

        public ActionResult Xem(int ma)
        {
            var ketQua = KhoaHocBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                RedirectToAction("Index", "TrangChu");
            }
            var khoaHoc = ketQua.ketQua;

            ketQua = CotDiem_NguoiDungBUS.layTheoMaKhoaHoc(ma);
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Xem", "KhoaHoc", new { ma = ma });
            }
            object[] mangKetQua = ketQua.ketQua as object[];
            ViewData["CotDiem"] = mangKetQua[0];
            ViewData["NguoiDung"] = mangKetQua[1];
            ViewData["Diem"] = mangKetQua[2];

            return View(khoaHoc);
        }

        [HttpPost]
        public ActionResult CapNhatBangDiem(string jsonDiem)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }

            return Json(CotDiem_NguoiDungBUS.capNhat(JsonConvert.DeserializeObject<List<dynamic>>(jsonDiem)));
        }
	}
}