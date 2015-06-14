using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Data;
using Helpers;

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
            ViewData["MaCauHoi"] = ma;
            KetQua ketQua = CauHoiBUS.layCauHoiTheoMa(ma);

            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            return View(ketQua.ketQua);
        }

        public ActionResult _Form_TraLoi(int ma = 0)
        {
            KetQua ketQua = TraLoiBUS.layTheoMa(ma);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua =
                        renderPartialViewToString(ControllerContext, "HoiDap/_Form_TraLoi.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyThemCauHoi(FormCollection form)
        {
            return Json(CauHoiBUS.them(chuyenForm(form)));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyThemTraLoi(FormCollection form)
        {
            //Kiểm tra đăng nhập
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn chưa đăng nhập"
                });
            }
            
            KetQua ketQua = TraLoiBUS.them(chuyenForm(form));
            
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

        public ActionResult XuLyXoaTraLoi(int ma)
        {
            return Json(
                TraLoiBUS.xoaTheoMa(ma), 
                JsonRequestBehavior.AllowGet    
            );
        }
	}
}