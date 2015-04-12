using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class KetQua
    {
        /// <summary>
        /// 0: Thành công ---
        /// 1: Không có dòng dữ liệu nào ---
        /// 2: Lỗi truy vấn / Lỗi xử lý
        /// </summary>
        public int trangThai;
        public object ketQua;
    }

    public class DTO
    {
        public virtual object[] kiemTra()
        {
            return new object[] 
            {
                true,
                null
            };
        }
        public virtual void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            return;
        }

        public string layString(System.Data.SqlClient.SqlDataReader dong, int index, string macDinh = null)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetString(index);
        }

        public int layInt(System.Data.SqlClient.SqlDataReader dong, int index, int macDinh = 0)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetInt32(index);
        }
        public DateTime layDateTime(System.Data.SqlClient.SqlDataReader dong, int index, DateTime macDinh)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetDateTime(index);
        }
    }
}
