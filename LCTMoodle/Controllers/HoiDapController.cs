﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;

namespace LCTMoodle.Controllers
{
    public class HoiDapController : LCTController
    {
        //
        // GET: /HoiDap/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CauHoi(string maCauHoi)
        {
            return View();
        }
        public PartialViewResult _itemTraLoi()
        {
            return PartialView();
        }
	}
}