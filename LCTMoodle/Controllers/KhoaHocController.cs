using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;

namespace LCTMoodle.Controllers
{
    public class KhoaHocController : LCTController
    {
        public ActionResult Tao()
        {
            return View();
        }

        public ActionResult XuLyThem(FormCollection formCollection)
        {
            var form = chuyenDuLieuForm(formCollection);
            KhoaHocBUS.themKhoaHoc(form);
            return Json(1);
        }
	}
}