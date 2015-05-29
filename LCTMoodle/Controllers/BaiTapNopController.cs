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
        public ActionResult XuLyThem(FormCollection form)
        {
            KetQua ketQua = BaiTapNopBUS.them(chuyenDuLieuForm(form));
            
            return Json(ketQua);
        }

        public ActionResult _DanhSachNop(int maBaiTap)
        {
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "BaiTapNop/_DanhSachNop.cshtml")
            }, JsonRequestBehavior.AllowGet);
        }
	}
}