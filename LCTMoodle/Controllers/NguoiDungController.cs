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
    public class NguoiDungController : LCTController
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
            //Tắt hiển thị cột trái, cột phải
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
            //Tắt hiển thị cột trái, cột phải
            ViewData["CotTrai"] = false;
            ViewData["CotPhai"] = false;
         
            return View();
        }

        [HttpPost]        
        public ActionResult XuLyThemNguoiDung(FormCollection formCollection)
        {
            Dictionary<string, string> form = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);
            KetQua ketQua = NguoiDungBUS.themNguoiDung(form);

            return Json(ketQua);
        }
	}
}