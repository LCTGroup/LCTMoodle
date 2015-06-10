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
            return View((CauHoiBUS.layToanBoCauHoi().ketQua) as List<CauHoiDTO>);
        }
        public ActionResult TaoCauHoi()
        {
            return View();
        }
        public ActionResult XemCauHoi(int ma)
        {
            KetQua ketQua = CauHoiBUS.layCauHoi(ma);

            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            return View(ketQua.ketQua);
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
            KetQua ketQua = TraLoiBUS.them(chuyenDuLieuForm(form));
            if (ketQua.trangThai == 0) 
            {
                return Json(new KetQua() 
                { 
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_TraLoi.cshtml", ketQua.ketQua)
                });
            }
            return Json(ketQua);
        }
	}
}