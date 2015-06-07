using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class CotDiemKHBUS : BUS
    {
        public static KetQua kiemTra(CotDiemDTO cotDiem)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (cotDiem.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (string.IsNullOrEmpty(cotDiem.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (cotDiem.heSo == 0)
            {
                loi.Add("Hệ số không được bỏ trống");
            }
            if (cotDiem.thoiDiem == null)
            {
                loi.Add("Thời điểm không được bỏ trống");
            }
            #endregion

            if (loi.Count > 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = loi
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
    }
}
