using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Helpers
{
    public static class TapTinHelper
    {
        public static string layDuongDanGoc()
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
        }

        public static string layDuongDan(string loai, string ten)
        {
            return layDuongDanGoc() + loai + "/" + ten;
        }

        public static string doc_string(string duongDan)
        {
            //Kiểm tra tồn tại
            if (!File.Exists(duongDan))
            {
                return null;
            }
            
            //Đọc tất cả
            return File.ReadAllText(duongDan);
        }

        public static bool coHoTroXem(string duoi)
        {
            duoi = duoi.Substring(1);
            return Array.IndexOf(new string[]
            { 
                "jpg",
                "jpge",
                "png",
                "txt", 
                "cs", 
                "css", 
                "js", 
                "rb", 
                "php",
                "cshtml",
                "cpp",
                "html",
                "haml"
            }, duoi) != -1;
        }
    }
}