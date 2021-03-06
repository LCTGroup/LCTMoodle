﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;
using System.Reflection;

namespace DAOLayer
{
    public class DAO<DAOClass, DTOClass>
        where DTOClass : DTO
    {
        #region Xử lý truy vấn dữ liệu
        static string chuoiKetNoi = System.Configuration.ConfigurationManager.ConnectionStrings["chuoiKetNoi_LCTMoodle"].ConnectionString;

        /// <summary>
        /// Thực thi stored procedure lấy về dòng đầu tiên
        /// </summary>
        /// <param name="tenStoredProcedure">Tên stored procedure</param>
        /// <param name="danhSachThamSo">Danh sách tham số (Cần truyền theo đúng thự tự của storedProcedure)</param>
        protected static KetQua layDong(string tenStoredProcedure, object[] danhSachThamSo, LienKet lienKet = null)
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            try
            {
                ketNoi.Open();

                SqlCommand lenh = new SqlCommand(tenStoredProcedure, ketNoi);
                lenh.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < danhSachThamSo.Length; i++)
                {
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i] == null ? DBNull.Value : danhSachThamSo[i]);
                }
                SqlDataReader dong = lenh.ExecuteReader();

                MethodInfo method = typeof(DAOClass).GetMethod("gan");

                DTOClass dto;

                if (dong.Read())
                {
                    dto = method.Invoke(null, new object[] { dong, lienKet }) as DTOClass;

                    dong.Close();

                    return new KetQua()
                    {
                        trangThai = 0,
                        ketQua = dto
                    };
                }
                else
                {
                    dong.Close();
                    return new KetQua()
                    {
                        trangThai = 1
                    };
                }
            }
            catch (SqlException e)
            {
                return new KetQua()
                {
                    trangThai = 2,
                    ketQua = "Lỗi truy vấn: " + e.Message
                };
            }
            finally
            {
                ketNoi.Close();
            }
        }

        /// <summary>
        /// Thực thi stored procedure lầy về danh sách dòng
        /// </summary>
        /// <param name="tenStoredProcedure">Tên stored procedure</param>
        /// <param name="danhSachThamSo">Danh sách tham số (Cần truyền theo đúng thự tự của storedProcedure)</param>
        protected static KetQua layDanhSachDong(string tenStoredProcedure, object[] danhSachThamSo, LienKet lienKet = null)
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            try
            {
                ketNoi.Open();

                SqlCommand lenh = new SqlCommand(tenStoredProcedure, ketNoi);
                lenh.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < danhSachThamSo.Length; i++)
                {
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i] == null ? DBNull.Value : danhSachThamSo[i]);
                }
                SqlDataReader dong = lenh.ExecuteReader();

                MethodInfo method = typeof(DAOClass).GetMethod("gan");

                List<DTOClass> dtos = new List<DTOClass>();

                if (dong.Read())
                {
                    do
                    {
                        dtos.Add(method.Invoke(null, new object[] { dong, lienKet }) as DTOClass);
                    }
                    while (dong.Read());

                    dong.Close();

                    return new KetQua()
                    {
                        trangThai = 0,
                        ketQua = dtos
                    };
                }
                else
                {
                    dong.Close();
                    return new KetQua()
                    {
                        trangThai = 1
                    };
                }
            }
            catch (SqlException e)
            {
                return new KetQua()
                {
                    trangThai = 2,
                    ketQua = "Lỗi truy vấn\r\n" + e.Message
                };
            }
            finally
            {
                ketNoi.Close();
            }
        }

        /// <summary>
        /// Thực hiện procedure không truy vấn
        /// </summary>
        /// <param name="tenStoredProcedure">Tên stored procedure</param>
        /// <param name="danhSachThamSo">Danh sách tham số (Cần truyền theo đúng thự tự của storedProcedure)</param>
        protected static KetQua khongTruyVan(string tenStoredProcedure, object[] danhSachThamSo)
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            try
            {
                ketNoi.Open();

                SqlCommand lenh = new SqlCommand(tenStoredProcedure, ketNoi);
                lenh.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < danhSachThamSo.Length; i++)
                {
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i] == null ? DBNull.Value : danhSachThamSo[i]);
                }
                if (lenh.ExecuteNonQuery() == 0)
                {
                    return new KetQua()
                    {
                        trangThai = 1
                    };
                }
                else
                {
                    return new KetQua()
                    {
                        trangThai = 0
                    };
                }
            }
            catch (SqlException e)
            {
                return new KetQua()
                {
                    trangThai = 2,
                    ketQua = "Lỗi truy vấn: " + e.Message
                };
            }
            finally
            {
                ketNoi.Close();
            }
        }

        /// <summary>
        /// Thực hiện stored procedure lấy về 1 giá trị
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tenStoredProcedure">Tên stored procedure</param>
        /// <param name="danhSachThamSo">Danh sách tham số (Cần truyền theo đúng thự tự của storedProcedure)</param>
        protected static KetQua layGiaTri<T>(string tenStoredProcedure, object[] danhSachThamSo)
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            try
            {
                ketNoi.Open();

                SqlCommand lenh = new SqlCommand(tenStoredProcedure, ketNoi);
                lenh.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < danhSachThamSo.Length; i++)
                {
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i] == null ? DBNull.Value : danhSachThamSo[i]);
                }

                object ketQua = lenh.ExecuteScalar();

                if (ketQua != null)
                {
                    return new KetQua()
                    {
                        trangThai = 0,
                        ketQua = Convert.ChangeType(ketQua, typeof(T))
                    };
                }
                else
                {
                    return new KetQua()
                    {
                        trangThai = 1
                    };
                }
            }
            catch (SqlException e)
            {

                return new KetQua()
                {
                    trangThai = 2,
                    ketQua = "Lỗi truy vấn: " + e.Message
                };
            }
            finally
            {
                ketNoi.Close();
            }
        }
        #endregion

        #region Lấy giá trị
        protected static object layMa(DTO dto)
        {
            return dto != null ? dto.ma : null;
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

        protected static string layString(System.Data.SqlClient.SqlDataReader dong, int index, string macDinh = null)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetString(index);
        }

        protected static int? layInt(System.Data.SqlClient.SqlDataReader dong, int index, int? macDinh = null)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetInt32(index);
        }

        protected static double? layDouble(System.Data.SqlClient.SqlDataReader dong, int index, double? macDinh = null)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetDouble(index);
        }

        protected static DateTime? layDateTime(System.Data.SqlClient.SqlDataReader dong, int index, DateTime? macDinh = null)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetDateTime(index);
        }

        protected static bool layBool(System.Data.SqlClient.SqlDataReader dong, int index, bool macDinh = false)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetBoolean(index);
        }

        #endregion
    }
}
