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
    public class TrangChuController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult FormMau()
        {
            return View();
        }

        public ActionResult CayMau()
        {
            return View();
        }

        public ActionResult TrangMau()
        {
            return View();
        }

        public ActionResult Test()
        {
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = new object[]
                {
                    new 
                    {
                        ma = "1",
                        ten = "A1"
                    },
                    new 
                    {
                        ma = "2",
                        ten = "A2"
                    },
                    new 
                    {
                        ma = "3",
                        ten = "A3"
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }
	}
}