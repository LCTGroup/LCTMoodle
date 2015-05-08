using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Helpers
{
    public static class LCTHelper
    {
        public static void taoThuMuc(string duongDanThuMuc)
        {
            if (!Directory.Exists(duongDanThuMuc))
            {
                Directory.CreateDirectory(duongDanThuMuc);
            }
        }
    }
}