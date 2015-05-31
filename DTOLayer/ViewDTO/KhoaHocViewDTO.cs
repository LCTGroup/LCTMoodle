using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class KhoaHocViewDTO : DTO
    {
        public string ten;
        public string moTa;
        public ChuDeViewDTO chuDe;
        public TapTinViewDTO hinhDaiDien;
        public NguoiDungViewDTO nguoiTao;
        public DateTime? thoiDiemTao;
        public bool canDangKy;
        public DateTime? hanDangKy;
        public int phiThamGia;
        public CheDoRiengTu cheDoRiengTu;
        public bool coBangDiem;
        public bool coBangDiemDanh;
        public bool canDuyetBaiViet;
    }
}