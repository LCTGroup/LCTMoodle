using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiTapNopViewDTO : DTO
    {
        public TapTinViewDTO tapTin;
        public string duongDan;
        public DateTime? thoiDiemTao;
        public NguoiDungViewDTO nguoiTao;
        public BaiVietBaiTapViewDTO baiVietBaiTap;
    }
}
