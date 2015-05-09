﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Helpers;

namespace LCTMoodle.Controllers
{
    public class ChuDeController : LCTController
    {
        public ActionResult QuanLy()
        {
            return View();
        }

        /// <summary>
        /// Lấy danh sách chủ đề theo mã chủ đề cha và phạm vi.
        /// </summary>
        public ActionResult XuLyLayDanhSach(int maChuDeCha, string phamVi)
        {
            return Json(ChuDeDAO.layChuDeTheoMaChuDeChaVaPhamVi(maChuDeCha, phamVi), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Form(int maChuDeCha, string phamVi = "HeThong")
        {
            ViewData["MaChuDeCha"] = maChuDeCha;
            ViewData["PhamVi"] = phamVi;

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "~/Views/ChuDe/_Form.cshtml", null, ViewData)
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(ChuDeBUS.themChuDe(chuyenDuLieuForm(formCollection)));
        }

        public ActionResult Chon(string phamVi = "HeThong")
        {
            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "~/Views/ChuDe/_Chon.cshtml")
                }, JsonRequestBehavior.AllowGet);
        }
	}
}