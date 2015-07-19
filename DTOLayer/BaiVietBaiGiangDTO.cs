using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiVietBaiGiangDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public string tomTat;
        public TapTinDTO tapTin;
        public DateTime? thoiDiemTao;
        public NguoiDungDTO nguoiTao;
        public KhoaHocDTO khoaHoc;
        public string danhSachMaThanhVienDaXem;
    }
}
