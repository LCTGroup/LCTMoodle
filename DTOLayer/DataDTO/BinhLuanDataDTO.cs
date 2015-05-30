using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class BinhLuanDataDTO : DTO
    {
        public string noiDung;
        public int maNguoiTao;
        public int maDoiTuong;
        public int maTapTin;
        public DateTime thoiDiemTao;
        public string loaiDoiTuong;
    }
}