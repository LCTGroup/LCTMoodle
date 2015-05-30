using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class BaiVietBaiTapDataDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public int maTapTin;
        public DateTime? thoiDiemHetHan;
        public DateTime? thoiDiemTao;
        public int maNguoiTao;
        public int maKhoaHoc;
    }
}
