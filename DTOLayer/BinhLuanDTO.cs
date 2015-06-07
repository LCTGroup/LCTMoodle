using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BinhLuanDTO : DTO
    {
        public string noiDung;
        public NguoiDungDTO nguoiTao;
        public DTO doiTuong;
        public TapTinDTO tapTin;
        public DateTime? thoiDiemTao;
        public string loaiDoiTuong;
    }
}
