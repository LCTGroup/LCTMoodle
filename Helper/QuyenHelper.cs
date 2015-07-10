using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpers
{
    public static class QuyenHelper
    {
        public static bool co(string[] quyen, string giaTri)
        {
            return quyen != null && (Array.IndexOf(quyen, giaTri) != -1 || Array.IndexOf(quyen, "QLDB") != -1);
        }

        public static bool co(List<string> quyen, string giaTri)
        {
            return quyen != null && quyen.Exists(x => x == giaTri || x == "QLDB");
        }
    }
}