using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiVietDienDanDataDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public int maTapTin;
        public DateTime? thoiDiemTao;
        public int maNguoiTao;
        public int maKhoaHoc;

        public override KetQua kiemTra()
        {
            KetQua ketQua = new KetQua();
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (string.IsNullOrEmpty(noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (maKhoaHoc == 0)
            {
                loi.Add("Khóa học không được bỏ trống");
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
