using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiVietDienDanViewDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public TapTinViewDTO tapTin;
        public DateTime? thoiDiemTao;
        public NguoiDungViewDTO nguoiTao;
        public KhoaHocViewDTO khoaHoc;
    }
}
