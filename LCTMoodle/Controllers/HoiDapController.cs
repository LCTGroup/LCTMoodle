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
    public class HoiDapController : LCTController
    {
        //
        // GET: /HoiDap/
        public ActionResult Index()
        {
            return View(CauHoiBUS.layToanBoCauHoi().ketQua);
        }
        public ActionResult TaoCauHoi()
        {
            return View();
        }
        public ActionResult XemCauHoi(int ma)
        {            
            return View(CauHoiBUS.layCauHoi(ma).ketQua as CauHoiDTO);
        }        
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyThemCauHoi(FormCollection form)
        {
            return Json(CauHoiBUS.them(chuyenDuLieuForm(form)));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyThemTraLoi(FormCollection form)
        {
            return Json(TraLoiBUS.them(chuyenDuLieuForm(form)).ketQua);
        }
	}
}