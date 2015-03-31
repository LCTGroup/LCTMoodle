using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCTMoodle.Controllers
{
    public class NguoiDungController : Controller
    {
        //
        // GET: /NguoiDung/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <returns></returns>
        public ActionResult DangNhap()
        {
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;
            return View();
        }
        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <returns></returns>
        public ActionResult DangKy()
        {
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;
            return View();
        }
	}
}