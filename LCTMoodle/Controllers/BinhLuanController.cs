﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTOLayer;
using BUSLayer;
using DAOLayer;
using Helpers;

namespace LCTMoodle.Controllers
{
    public class BinhLuanController : LCTController
    {
        public ActionResult Them(FormCollection formCollection)
        {
            KetQua ketQua = BinhLuanBUS.them(chuyenDuLieuForm(formCollection));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BinhLuan/_Muc.cshtml", ketQua.ketQua)
                    });
            }
            else
            {
                return Json(ketQua);
            }
        }
	}
}