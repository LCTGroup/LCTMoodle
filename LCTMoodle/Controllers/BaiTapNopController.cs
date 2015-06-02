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
            return Json(BaiTapNopBUS.themHoacCapNhat(chuyenDuLieuForm(form)));
        }

        public ActionResult _DanhSachNop(int maBaiTap)
        {
            KetQua ketQua = BaiTapNopBUS.layTheoMaBaiVietBaiTap(maBaiTap);
            List<BaiTapNopViewDTO> danhSachBaiTapNop =
                ketQua.trangThai == 0 ?
                    ketQua.ketQua as List<BaiTapNopViewDTO> :
                    null;

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "BaiTapNop/_DanhSachNop.cshtml", danhSachBaiTapNop)
            }, JsonRequestBehavior.AllowGet);
        }
	}
}