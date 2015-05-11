using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class ChuDeDataDTO : DTO
    {
        public string ten;
        public string moTa;
        public int maNguoiTao;
        public DateTime? thoiDiemTao;
        public string phamVi;
        public int maChuDeCha;
        public int maHinhDaiDien;

        public override KetQua kiemTra()
        {
            KetQua ketQua = new KetQua();
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (string.IsNullOrEmpty(moTa))
            {
                loi.Add("Mô tả không được bỏ trống");
            }
            if (maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (maHinhDaiDien == 0)
            {
                loi.Add("Hình đại diện không được bỏ trống");
            } 
            #endregion

            if (loi.Count > 0)
            {
                ketQua.trangThai = 3;
                ketQua.ketQua = loi;
            }
            else 
            {
                ketQua.trangThai = 0;
            }
            
            return ketQua;
        }
    }
}
