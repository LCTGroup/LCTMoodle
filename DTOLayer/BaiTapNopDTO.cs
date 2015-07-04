using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiTapNopDTO : DTO
    {
        public TapTinDTO tapTin;
        public string duongDan;
        public DateTime? thoiDiemTao;
        public NguoiDungDTO nguoiTao;
        public BaiVietBaiTapDTO baiVietBaiTap;
        public bool daChuyenDiem;
        public double? diem;
        public string ghiChu;
        public bool daXoa;
    }
}
