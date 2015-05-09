using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class ChuDeDataDTO : DTO
    {
        public int ma;
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
            List<string> danhSachLoi = new List<string>();

            #region Bắt lỗi
            if (ten.Length == 0)
            {
                danhSachLoi.Add("Tên không được bỏ trống");
            }
            if (moTa.Length == 0)
            {
                danhSachLoi.Add("Mô tả không được bỏ trống");
            }
            if (maNguoiTao == 0)
            {
                danhSachLoi.Add("Người tạo không được bỏ trống");
            }
            if (maHinhDaiDien == 0)
            {
                danhSachLoi.Add("Hình đại diện không được bỏ trống");
            } 
            #endregion

            if (danhSachLoi.Count > 0)
            {
                ketQua.trangThai = 3;
                ketQua.ketQua = danhSachLoi;
            }
            else 
            {
                ketQua.trangThai = 0;
            }
            
            return ketQua;
        }
    }
}
