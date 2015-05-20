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
	}
}