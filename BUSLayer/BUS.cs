using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;

namespace BUSLayer
{
    public class BUS
    {
        protected static System.Web.SessionState.HttpSessionState Session = System.Web.HttpContext.Current.Session;

        #region Lấy giá trị
        protected static bool coKiemTra(string ten, string[] truong, bool kiemTra)
        {
            return 
                truong == null ? kiemTra :
                Array.Exists(truong, x => x == ten) == kiemTra;
        }

        protected static T layDTO<T>(Dictionary<string, string> form, string key, T macDinh = null)
            where T : DTOLayer.DTO, new()
        {
            try
            {
                int? maTam = layInt(form, key);

                return maTam.HasValue ?
                    new T()
                    {
                        ma = maTam
                    } :
                    macDinh;
            }
            catch
            {
                return macDinh;                
            }
        }

        protected static T layDTO<T>(int? ma, T macDinh = null)
            where T : DTOLayer.DTO, new()
        {
            try
            {
                return ma.HasValue ?
                    new T()
                    {
                        ma = ma
                    } :
                    macDinh;
            }
            catch
            {
                return macDinh;
            }
        }

        protected static T layDTO<T>(KetQua ketQua)
            where T : DTO
        {
            return ketQua.trangThai == 0 ? (T)ketQua.ketQua : null;
        }

        protected static List<T> layDanhSachDTO<T>(KetQua ketQua)
            where T : DTO
        {
            return ketQua.trangThai == 0 ? (List<T>)ketQua.ketQua : null;
        }

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

        protected static int? layInt(Dictionary<string, string> form, string key, int? macDinh = null)
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

        protected static DateTime? layDate(Dictionary<string, string> form, string key, DateTime? macDinh = null)
        {
            try
            {
                return DateTime.ParseExact(form[key], "d/M/yyyy", null);
            }
            catch
            {
                return macDinh;
            }
        }

        protected static DateTime? layTime(Dictionary<string, string> form, string key, DateTime? macDinh = null)
        {
            try
            {
                return DateTime.ParseExact(form[key], "H:mm", null);
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
                return DateTime.ParseExact(form[key], "H:mm d/M/yyyy", null);
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
