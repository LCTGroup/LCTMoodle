using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class KhoaHocDataDTO : DTO
    {
        public string ten;
        public string moTa;
        public int maChuDe;
        public int maHinhDaiDien;
        public int maNguoiTao;
        public DateTime? thoiDiemTao;
        public DateTime? han;
        public bool canDangKy;
        public DateTime? hanDangKy;
        public int phiThamGia;
        public string cheDoRiengTu;
        public bool coBangDiem;
        public bool coBangDiemDanh;
        public bool canDuyetBaiViet;

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
            if (maChuDe == 0)
            {
                loi.Add("Chủ đề không được bỏ trống");
            }
            if (maHinhDaiDien == 0)
            {
                loi.Add("Hình đại diện không được bỏ trống");
            }
            if (maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (cheDoRiengTu.Length == 0)
            {
                loi.Add("Chế độ riêng tư không được bỏ trống");
            }
            else if (CheDoRiengTu.lay(cheDoRiengTu) == null)
            {
                loi.Add("Chế độ riêng tư không hợp lệ");
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