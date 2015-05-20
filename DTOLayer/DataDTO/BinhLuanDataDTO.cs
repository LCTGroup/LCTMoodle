using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BinhLuanDataDTO : DTO
    {
        public string noiDung;
        public int maNguoiTao;
        public int maDoiTuong;
        public int maTapTin;
        public DateTime thoiDiemTao;
        public string loaiDoiTuong;

        public override KetQua kiemTra()
        {
            KetQua ketQua = new KetQua();
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (maDoiTuong == 0)
            {
                loi.Add("Đối tượng bình luận không được bỏ trống");
            }
            if (!new string[] { "BaiVietDienDan" }.Contains(loaiDoiTuong))
            {
                loi.Add("Loại đối tượng không hợp lệ");
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