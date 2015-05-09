using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUSLayer
{
    public class BUS
    {
        #region Lấy giá trị
        protected static string layString(Dictionary<string, string> form, string key, string macDinh = null)
        {
            try
            {
                return form[key];
            }
            catch
            {
                return macDinh;
            }
        }

        protected static int layInt(Dictionary<string, string> form, string key, int macDinh = 0)
        {
            try
            {
                return int.Parse(form[key]);
            }
            catch
            {
                return macDinh;
            }
        }

        protected static DateTime? layDateTime(Dictionary<string, string> form, string key, DateTime? macDinh = null)
        {
            try
            {
                return DateTime.Parse(form[key]);
            }
            catch
            {
                return macDinh;
            }
        }

        protected static DateTime? layDateTime_Full(Dictionary<string, string> form, string key_ngay, string key_gio, DateTime? macDinh = null)
        {
            try
            {
                DateTime?
                    ngay = layDateTime(form, key_ngay),
                    gio = layDateTime(form, key_gio);

                return DateTime.Parse
                (
                    (ngay.HasValue ? ngay.Value.ToShortDateString() : null) +
                    " " +
                    (gio.HasValue ? gio.Value.ToShortTimeString() : null)
                );
            }
            catch
            {
                return macDinh;
            }
        }

        protected static bool layBool(Dictionary<string, string> form, string key, bool macDinh = false)
        {
            try
            {
                return form.ContainsKey(key);
            }
            catch
            {
                return macDinh;
            }
        } 
        #endregion
    }
}
