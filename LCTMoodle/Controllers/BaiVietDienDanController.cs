using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;

namespace LCTMoodle.Controllers
{
    public class BaiVietDienDanController : LCTController
    {
        [ValidateInput(false)]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(BaiVietDienDanBUS.them(chuyenDuLieuForm(formCollection)));
        }

        public ActionResult LayDanhSach(int maKhoaHoc)
        {
            return Json(BaiVietDienDanDAO.layTheoMaKhoaHoc(maKhoaHoc), JsonRequestBehavior.AllowGet);
        }
	}
}