using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class KhoaHocDTO : DTO
    {
        public string ten;
        public string moTa;
        public ChuDeDTO chuDe;
        public TapTinDTO hinhDaiDien;
        public NguoiDungDTO nguoiTao;
        public DateTime? thoiDiemTao;
        public DateTime? thoiDiemHetHan;
        public bool canDangKy;
        public DateTime? hanDangKy;
        public int? phiThamGia;
        public string cheDoRiengTu;
        public bool coBangDiem;
        public bool coBangDiemDanh;
        public bool canDuyetBaiViet;
    }
}