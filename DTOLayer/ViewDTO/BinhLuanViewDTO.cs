using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BinhLuanViewDTO : DTO
    {
        public string noiDung;
        public NguoiDungViewDTO nguoiTao;
        public DTO doiTuong;
        public TapTinViewDTO tapTin;
        public DateTime? thoiDiemTao;
        public string loaiDoiTuong;
    }
}
