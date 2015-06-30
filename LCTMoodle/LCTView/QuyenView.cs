using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;
using BUSLayer;

namespace LCTMoodle.LCTView
{
    public class QuyenView
    {
        public static string[] layDSQuyen(string phamVi, int maDoiTuong = 0, int? maNguoiDung = null)
        {
            if (!maNguoiDung.HasValue)
            {
                maNguoiDung = HttpContext.Current.Session["NguoiDung"] as int?;
            }
            if (!maNguoiDung.HasValue)
            {
                return null;
            }

            var ketQua = QuyenBUS.layTheoMaNguoiDungVaMaDoiTuong_MangGiaTri(maNguoiDung.Value, phamVi, maDoiTuong);
            return ketQua.trangThai == 0 ? ketQua.ketQua as string[] : null;
        }
    }
}