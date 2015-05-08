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
        public DateTime thoiDiemTao;
        public bool canDangKy;
        public DateTime hanDangKy;
        public int phiThamGia;
        public int cheDoRiengTu;
        public bool coBangDiem;
        public bool coBangDiemDanh;
        public bool canDuyetBaiViet;

        //public override KetQua kiemTra()
        //{

        //}
    }
}