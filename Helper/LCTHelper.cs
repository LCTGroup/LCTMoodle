using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

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

        #region Kiểm tra email
        /// <summary>
        /// Kiểm tra email
        /// </summary>
        /// <param name="email">Truyền vào chuỗi email cần kiểm tra</param>
        /// <returns>true: nếu đúng định dạng email, ngược lại là false</returns>
        public static bool laEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
        #endregion
    }
}