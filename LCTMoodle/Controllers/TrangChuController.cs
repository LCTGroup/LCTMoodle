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
    public class TrangChuController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/NguoiDung/DanhSachXacNhanThem.cshtml", NguoiDungBUS.docTapTin(Helpers.TapTinHelper.layDuongDan("Tam", "1.xls")).ketQua);
            return null;
            var ketQua = KhoaHocBUS.timKiemPhanTrang(1, 8, null, null, new LienKet() { "GiangVien" });
            if (ketQua.trangThai == 0)
            {
                ViewData["KhoaHoc"] = ketQua.ketQua as List<KhoaHocDTO>;
            }

            ketQua = CauHoiBUS.layDanhSach(10, new LienKet() { "NguoiTao", "HinhDaiDien" });
            if (ketQua.trangThai == 0)
            {
                ViewData["CauHoi"] = ketQua.ketQua as List<CauHoiDTO>;
            }

            ketQua = ChuDeBUS.timKiemPhanTrang(1, 20);

            return View();
        }

        public ActionResult FormMau()
        {
            return View();
        }

        public ActionResult CayMau()
        {
            return View();
        }

        public ActionResult TrangMau()
        {
            return View();
        }
	}
}