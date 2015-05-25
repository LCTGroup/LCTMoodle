using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Helpers;
using Data;

namespace LCTMoodle.Controllers
{
    public class TapTinController : LCTController
    {
        public ActionResult Lay(string loai, int ma)
        {
            KetQua ketQua = TapTinBUS.lay(loai, ma);

            if (ketQua.trangThai == 0)
            {
                TapTinViewDTO tapTin = ketQua.ketQua as TapTinViewDTO;

                string duongDan = TapTinHelper.layDuongDan(loai, tapTin.ma + tapTin.duoi);
                if (System.IO.File.Exists(duongDan))
                {
                    return File(duongDan, tapTin.loai, tapTin.ten);
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection form)
        {
            KetQua ketQua = TapTinBUS.them(Request.Files["TapTin"]);

            return Json(ketQua);
        }
	}
}