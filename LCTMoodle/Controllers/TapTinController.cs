using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;

namespace LCTMoodle.Controllers
{
    public class TapTinController : Controller
    {
        public ActionResult LayTapTin(int ma)
        {
            KetQua ketQua = TapTinBUS.layTapTin(ma);

            if (ketQua.trangThai == 0)
            {
                TapTinViewDTO tapTin = ketQua.ketQua as TapTinViewDTO;
                return File(TapTinBUS.layDuongDan(tapTin.thuMuc, tapTin.ma + "_" + tapTin.ten), tapTin.loai, tapTin.ten);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult ThemTapTin(FormCollection form)
        {
            KetQua ketQua = TapTinBUS.themTapTin(Request.Files["TapTin"], form["ThuMuc"]);

            return Json(
                new KetQua()
                {
                    trangThai = ketQua.trangThai,
                    ketQua = (ketQua.ketQua as TapTinViewDTO).ma
                }
            );
        }
	}
}