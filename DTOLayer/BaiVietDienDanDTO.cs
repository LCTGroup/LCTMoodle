using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiVietDienDanDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public TapTinDTO tapTin;
        public DateTime? thoiDiemTao;
        public NguoiDungDTO nguoiTao;
        public KhoaHocDTO khoaHoc;
        public bool ghim;
        public int? diem;
        public string danhSachMaThanhVienDaXem;
    }
}
