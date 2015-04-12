using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;

namespace LCTMoodle.Controllers
{
    public class ChuDeController : LCTController
    {
        public ActionResult QuanLy()
        {
            return View();
        }

        public ActionResult _Form(int maChuDeCha, string phamVi = "Hệ thống")
        {
            ViewData["MaChuDeCha"] = maChuDeCha;
            ViewData["PhamVi"] = phamVi;

            return Json(new
            {
                trangThai = 1,
                ketQua = renderPartialViewToString(ControllerContext, "~/Views/ChuDe/_Form.cshtml",null, ViewData)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThemChuDe(FormCollection form)
        {
            KetQua ketQua = ChuDeBUS.themChuDe(form);

            return Json(new KetQua()
            {
                trangThai = ketQua.trangThai,
                ketQua = ketQua.ketQua
            });
        }
	}
}