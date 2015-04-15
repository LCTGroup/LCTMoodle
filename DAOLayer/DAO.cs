using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;

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
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            try
            {
                ketNoi.Open();

                SqlCommand lenh = new SqlCommand(tenStoredProcedure, ketNoi);
                lenh.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < danhSachThamSo.Length; i++)
                {
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i]);
                }
                SqlDataReader dong = lenh.ExecuteReader();

                DTO DTO = (DTO)Activator.CreateInstance(typeof(T));
                if (dong.Read())
                {
                    DTO.gan(dong);
                    dong.Close();

                    return new KetQua()
                        {
                            trangThai = 0,
                            ketQua = (T)Convert.ChangeType(DTO, typeof(T))
                        };
                }
                else
                {
                    dong.Close();
                    return new KetQua()
                        {
                            trangThai = 1,
                            ketQua = "Không có dòng dữ liệu nào"
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
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            try
            {
                ketNoi.Open();

                SqlCommand lenh = new SqlCommand(tenStoredProcedure, ketNoi);
                lenh.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < danhSachThamSo.Length; i++)
                {
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i]);
                }
                SqlDataReader dr = lenh.ExecuteReader();

                List<DTO> DTOs = ((List<T>)Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(T)))).ConvertAll<DTO>(X => (DTO)Convert.ChangeType(X, typeof(DTO)));
                                
                if (dr.Read())
                {
                    int i = 0;
                    do
                    {
                        DTOs.Add((DTO)Activator.CreateInstance(typeof(T)));
                        DTOs[i++].gan(dr);
                    }
                    while (dr.Read());

                    dr.Close();

                    return new KetQua()
                        {
                            trangThai = 0,
                            ketQua = DTOs.ConvertAll<T>(X => (T)Convert.ChangeType(X, typeof(T)))
                        };
                }
                else
                {
                    dr.Close();
                    return new KetQua()
                    {
                        trangThai = 1,
                        ketQua = "Không có dòng dữ liệu nào"
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
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i]);
                }
                lenh.ExecuteNonQuery();

                return new KetQua()
                {
                    trangThai = 0
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

        /// <summary>
        /// Thực hiện stored procedure lấy về 1 giá trị
        /// </summary>
        /// <typeparam name="T">Đối tượng DTO</typeparam>
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
                    lenh.Parameters.AddWithValue("@" + i, danhSachThamSo[i]);
                }

                object ketQua = lenh.ExecuteScalar();

                return new KetQua()
                    {
                        trangThai = 0,
                        ketQua = (T)Convert.ChangeType(ketQua, typeof(T))
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
