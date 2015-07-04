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

        public static string layString(Dictionary<string, object> duLieu, string key, string macDinh = null)
        {
            object item;
            if (duLieu.TryGetValue(key, out item))
            {
                return item.ToString();
            }

            return macDinh;
        }

        public static T layGiaTri<T>(Dictionary<string, object> duLieu, string key, T macDinh)
        {
            object item;
            if (duLieu.TryGetValue(key, out item))
            {
                return (T)Convert.ChangeType(item, typeof(T));
            }

            return macDinh;
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