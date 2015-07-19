using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiVietTaiLieuDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public TapTinDTO tapTin;
        public DateTime? thoiDiemTao;
        public NguoiDungDTO nguoiTao;
        public KhoaHocDTO khoaHoc;
        public string danhSachMaThanhVienDaXem;
    }
}
