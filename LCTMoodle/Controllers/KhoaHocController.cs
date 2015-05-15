using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DAOLayer;
using DTOLayer;

namespace LCTMoodle.Controllers
{
    public class KhoaHocController : LCTController
    {
        public ActionResult Xem(int ma)
        {
            KetQua ketQua = KhoaHocDAO.layTheoMa(ma);

            if (ketQua.trangThai == 0)
            {
                return View(ketQua.ketQua);
            }
            else
            {
                return RedirectToAction("Index", "TrangChu");
            }
        }

        public ActionResult Tao()
        {
            return View();
        }       

        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(KhoaHocBUS.them(chuyenDuLieuForm(formCollection)));
        }
	}
}