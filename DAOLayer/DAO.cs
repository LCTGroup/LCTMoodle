using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class DAO
    {
        static string chuoiKetNoi = System.Configuration.ConfigurationManager.ConnectionStrings["chuoiKetNoi_LCTMoodle"].ConnectionString;

        /// <summary>
        /// Thực thi stored procedure lấy về dòng đầu tiên
        /// </summary>
        /// <typeparam name="T">Đối tượng DTO</typeparam>
        /// <param name="tenStoredProcedure">Tên stored procedure</param>
        /// <param name="danhSachThamSo">Danh sách tham số (Cần truyền theo đúng thự tự của storedProcedure)</param>
        protected static KetQua layDong<T>(string tenStoredProcedure, object[] danhSachThamSo)
            where T : DTO, new()
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

                T DTO = new T();
                if (dong.Read())
                {
                    DTO.gan(dong);
                    dong.Close();

                    return new KetQua()
                        {
                            trangThai = 0,
                            ketQua = DTO
                        };
                }
                else
                {
                    dong.Close();
                    return new KetQua()
                        {
                            trangThai = 1,
                            ketQua = DTO
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
        /// Thực thi stored procedure lầy về danh sách dòng
        /// </summary>
        /// <typeparam name="T">Đối tượng DTO</typeparam>
        /// <param name="tenStoredProcedure">Tên stored procedure</param>
        /// <param name="danhSachThamSo">Danh sách tham số (Cần truyền theo đúng thự tự của storedProcedure)</param>
        protected static KetQua layDanhSachDong<T>(string tenStoredProcedure, object[] danhSachThamSo)
            where T : DTO, new()
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
                SqlDataReader dr = lenh.ExecuteReader();

                List<T> DTOs = new List<T>();
                                
                if (dr.Read())
                {
                    int i = 0;
                    do
                    {
                        DTOs.Add(new T());
                        DTOs[i++].gan(dr);
                    }
                    while (dr.Read());

                    dr.Close();

                    return new KetQua()
                        {
                            trangThai = 0,
                            ketQua = DTOs
                        };
                }
                else
                {
                    dr.Close();
                    return new KetQua()
                    {
                        trangThai = 1,
                        ketQua = DTOs
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
                    ketQua = "Lỗi truy vấn\r\n" + e.Message
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
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
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

                return new KetQua()
                    {
                        trangThai = 0,
                        ketQua = Convert.ChangeType(ketQua, typeof(T))
                    };
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
    }
}
