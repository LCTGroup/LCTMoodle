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
    class DAOClass
    {
        static string chuoiKetNoi = System.Configuration.ConfigurationManager.ConnectionStrings["chuoiKetNoi_LCTMoodle"].ConnectionString;

        /// <summary>
        /// Thực thi stored procedure lấy về dòng đầu tiên
        /// </summary>
        /// <typeparam name="T">Đối tượng DTO</typeparam>
        /// <param name="tenStoredProcedure">Tên stored procedure</param>
        /// <param name="danhSachThamSo">Danh sách tham số (Cần truyền theo đúng thự tự của storedProcedure)</param>
        /// <returns>
        /// Mảng object gồm 2 chỉ số:
        ///     [0]: Trạng thái kết quả thực hiện
        ///         0: Thành công
        ///         1: Không có dòng dữ liệu nào được trả về
        ///         2: Lỗi
        ///     [1]: 
        ///         Thành công: Kết quả trả về của stored procedure - Dòng kết quả đầu tiên, được lưu bởi đối tượng T (T)
        ///         Thất bại: Tên lối
        /// </returns>
        protected static object[] layDong<T>(string tenStoredProcedure, object[] danhSachThamSo)
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
                    DTO.ganDTO(dong);
                    dong.Close();

                    return new object[]
                        {
                            0,
                            (T)Convert.ChangeType(DTO, typeof(T))
                        };
                }
                else
                {
                    dong.Close();
                    return new object[]
                        {
                            1
                        };
                }
            }
            catch (SqlException e)
            {
                return new object[]
                    {
                        2,
                        e.Message
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
        /// <returns>
        /// Mảng object gồm 2 chỉ số:
        ///     [0]: Trạng thái kết quả thực hiện
        ///         0: Thành công
        ///         1: Không có dòng dữ liệu nào được trả về
        ///         2: Lỗi
        ///     [1]: 
        ///         Thành công: Kết quả trả về của stored procedure - Toàn bộ kết quả, được lưu bởi danh sách đối tượng T (List<T>)
        ///         Thất bại: Tên lối
        /// </returns>
        protected static object[] layDanhSachDong<T>(string tenStoredProcedure, object[] danhSachThamSo)
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
                        DTOs[i].ganDTO(dr);
                    }
                    while (dr.Read());

                    dr.Close();

                    return new object[]
                        {
                            0,
                            DTOs.ConvertAll<T>(X => (T)Convert.ChangeType(X, typeof(T)))
                        };
                }
                else
                {
                    dr.Close();
                    return new object[]
                        {
                            1
                        };
                }
            }
            catch (SqlException e)
            {
                return new object[]
                    {
                        2,
                        e.Message
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
        /// <returns>
        /// Mảng object gồm 2 chỉ số:
        ///     [0]: Trạng thái kết quả thực hiện
        ///         0: Thành công
        ///         1: Thất bại
        ///     [1]:         
        ///         Thất bại: Tên lối
        /// </returns>
        protected static object[] khongTruyVan(string tenStoredProcedure, object[] danhSachThamSo)
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
                return new object[] 
                    {
                        0
                    };
            }
            catch (SqlException e)
            {
                return new object[]
                    {
                        1,
                        e.Message
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
        /// <returns>
        /// Mảng object gồm 2 chỉ số:
        ///     [0]: Trạng thái kết quả thực hiện
        ///         0: Thành công
        ///         1: Lỗi
        ///     [1]: 
        ///         Thành công: Giá trị kiểu T
        ///         Thất bại: Tên lối
        /// </returns>
        protected static object[] layGiaTri<T>(string tenStoredProcedure, object[] danhSachThamSo)
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

                return new object[] 
                    {
                        0,
                        (T)Convert.ChangeType(ketQua, typeof(T))
                    };
            }
            catch (SqlException e)
            {

                return new object[] 
                    {
                        1,
                        e.Message
                    };
            }
            finally
            {
                ketNoi.Close();
            }
        }
    }
}
