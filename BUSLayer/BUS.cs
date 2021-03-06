﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DTOLayer;

namespace BUSLayer
{
    public class BUS
    {
        #region Lấy giá trị
        public static bool coQuyen(string giaTri, string phamVi, int maDoiTuong = 0, int? maNguoiDung = null)
        {
            if (!maNguoiDung.HasValue)
            {
                maNguoiDung = System.Web.HttpContext.Current.Session["NguoiDung"] as int?;
            }

            if (!maNguoiDung.HasValue)
            {
                return false;
            }

            var ketQua = QuyenBUS.kiemTraQuyenNguoiDung(maNguoiDung.Value, giaTri, phamVi, maDoiTuong);
            return ketQua.trangThai == 0 && (bool)ketQua.ketQua;
        }

        protected static bool coKiemTra(string ten, string[] truong, bool kiemTra)
        {
            return 
                truong == null ? kiemTra :
                Array.Exists(truong, x => x == ten) == kiemTra;
        }

        protected static object layMa(DTO dto)
        {
            return dto != null ? dto.ma : null;
        }

        protected static string layMa_String(DTO dto)
        {
            return dto != null ? dto.ma.ToString() : null;
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

        /// <summary>
        /// Lấy bảng mã
        /// </summary>
        /// <param name="dsMa">Danh sách mã (1,2,3,4,5,6)</param>
        /// <returns>DataTable Mã</returns>
        protected static DataTable layBangMa(string dsMa)
        {
            var bang = new DataTable();
            bang.Columns.Add("Ma");

            string[] mangMa = dsMa.Split(',');
            foreach(var ma in mangMa)
            {
                bang.Rows.Add(new object[] { ma });
            }
            return bang;
        }
        #endregion
    }
}
