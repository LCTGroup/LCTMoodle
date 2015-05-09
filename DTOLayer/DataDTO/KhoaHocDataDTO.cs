using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class KhoaHocDataDTO : DTO
    {
        public int ma;
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
            if (maChuDe == 0)
            {
                danhSachLoi.Add("Chủ đề không được bỏ trống");
            }
            if (maHinhDaiDien == 0)
            {
                danhSachLoi.Add("Hình đại diện không được bỏ trống");
            }
            if (maNguoiTao == 0)
            {
                danhSachLoi.Add("Người tạo không được bỏ trống");
            }
            if (cheDoRiengTu.Length == 0)
            {
                danhSachLoi.Add("Chế độ riêng tư không được bỏ trống");
            }
            else if (CheDoRiengTu.lay(cheDoRiengTu) == null)
            {
                danhSachLoi.Add("Chế độ riêng tư không hợp lệ");
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