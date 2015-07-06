using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BinhLuanBaiVietDienDanDTO : DTO
    {
        public string noiDung;
        public NguoiDungDTO nguoiTao;
        public BaiVietDienDanDTO baiVietDienDan;
        public TapTinDTO tapTin;
        public DateTime? thoiDiemTao;
        public int? diem;
    }
}
