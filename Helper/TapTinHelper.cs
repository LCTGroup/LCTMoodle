using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Ionic.Zip;

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
                "jpeg",
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

        public static string nen(string[] dsDuongDan, string duongDanGoc)
        {
            ZipFile zip = new ZipFile();

            int sl = dsDuongDan.Length;
            for (int i = 0; i < sl; i += 2)
            {
                zip.AddFile(dsDuongDan[i], duongDanGoc).FileName = dsDuongDan[i + 1];
            }

            string duongDanLuu = TapTinHelper.layDuongDanGoc() + "Tam/" + DateTime.Now.ToString("yyMMddhhmmss") + ".zip";
            zip.Save(duongDanLuu);

            return duongDanLuu;
        }
    }
}