﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpers
{
    public static class TapTinHelper
    {
        public static string layDuongDanGoc()
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
        }

        public static string layDuongDan(string thuMuc, string ten)
        {
            return layDuongDanGoc() + thuMuc + "/" + ten;
        }

    }
}