using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTOLayer;
using BUSLayer;
using DAOLayer;
using Helpers;

namespace LCTMoodle.Controllers
{
    public class BinhLuanController : LCTController
    {
        public ActionResult Them(FormCollection formCollection)
        {
            return Json(BinhLuanBUS.them(chuyenDuLieuForm(formCollection)));
        }

        public ActionResult Lay(string loaiDoiTuong, int maDoiTuong)
        {
            return Json(BinhLuanDAO.layTheoDoiTuong(loaiDoiTuong, maDoiTuong), JsonRequestBehavior.AllowGet);
        }
	}
}