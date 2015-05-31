using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

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
    }
}