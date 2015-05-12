using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using Helpers;

namespace LCTMoodle.Controllers
{
    public class TapTinController : LCTController
    {
        [Route("TapTin/{ma:int}/{loai}")]
        public ActionResult Lay(int ma, string loai)
        {
            KetQua ketQua = TapTinBUS.lay(ma, loai);

            if (ketQua.trangThai == 0)
            {
                TapTinViewDTO tapTin = ketQua.ketQua as TapTinViewDTO;
                return File(TapTinHelper.layDuongDan(loai, tapTin.ma + System.IO.Path.GetExtension(tapTin.ten)), tapTin.loai, tapTin.ten);
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