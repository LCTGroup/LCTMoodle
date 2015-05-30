using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class BaiTapNopDataDTO : DTO
    {
        public int maTapTin;
        public string duongDan;
        public DateTime? thoiDiemTao;
        public int maNguoiTao;
        public int maBaiVietBaiTap;
    }
}
