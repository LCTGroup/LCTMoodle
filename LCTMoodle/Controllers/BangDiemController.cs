using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTOLayer;
using BUSLayer;

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
	}
}