using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class DTO
    {
        public virtual KetQua kiemTra()
        {
            //Duyết tất cả những thuộc tính có ràng buộc
            //Nếu có thuộc tính nào ko hợp lệ
            //(Đối tượng kết quả là 1 mảng lỗi)
            //=> Đưa 1 dòng message vào kết quả
            return null;
        }
        public virtual void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            return;
        }

        #region Lấy giá trị
        public string layString(System.Data.SqlClient.SqlDataReader dong, int index, string macDinh = null)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetString(index);
        }

        public int layInt(System.Data.SqlClient.SqlDataReader dong, int index, int macDinh = 0)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetInt32(index);
        }

        public DateTime? layDateTime(System.Data.SqlClient.SqlDataReader dong, int index, DateTime? macDinh = null)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetDateTime(index);
        }

        public bool layBool(System.Data.SqlClient.SqlDataReader dong, int index, bool macDinh = false)
        {
            return dong.IsDBNull(index) ? macDinh : dong.GetBoolean(index);
        }
		 
	    #endregion
    }

    public class KetQua
    {
        /// <summary>
        /// 0: Thành công ---
        /// 1: Không có dòng dữ liệu nào ---
        /// 2: Lỗi truy vấn / Lỗi xử lý
        /// 3: Lỗi ràng buộc
        /// </summary>
        public int trangThai;
        public object ketQua;
    }

    public class PhamVi
    {
        public string ma;
        public string ten;
        public string moTa;
        public string hinh;

        public static PhamVi lay(string ma)
        {
            var danhSach = layDanhSach();
            int soLuong = danhSach.Count;

            for (int i = 0; i < soLuong; i++)
            {
                if (danhSach[i].ma.Equals(ma))
                {
                    return danhSach[i];
                }
            }

            return null;
        }

        public static List<PhamVi> layDanhSach()
        {
            return new List<PhamVi>()
            {
                new PhamVi() 
                {
                    ma = "HeThong",
                    ten = "Hệ thống",
                    moTa = "Những chủ đề trong phạm vi toàn hệ thống",
                    hinh = "he-thong.png"
                },
                new PhamVi() 
                {
                    ma = "KhoaHoc",
                    ten = "Khóa học",
                    moTa = "Những chủ đề trong phạm vi khóa học",
                    hinh = "khoa-hoc.png"
                }
            };
        }
    }

    public class CheDoRiengTu
    {
        public string ma;
        public string ten;
        public bool macDinh;

        public static CheDoRiengTu lay(string ma)
        {
            var danhSach = layDanhSach();
            int soLuong = danhSach.Count;

            for (int i = 0; i < soLuong; i++)
            {
                if (danhSach[i].ma.Equals(ma))
                {
                    return danhSach[i];
                }
            }

            return null;
        }

        public static List<CheDoRiengTu> layDanhSach()
        {
            return new List<CheDoRiengTu>()
            {
                new CheDoRiengTu() 
                {
                    ma = "NoiBo",
                    ten = "Nội bộ",
                    macDinh = true
                },
                new CheDoRiengTu()
                {
                    ma = "CongKhai",
                    ten = "Công khai"
                }
            };
        }
    }
}
