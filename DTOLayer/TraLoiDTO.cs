using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class TraLoiDTO : DTO
    {
        public string noiDung;
        public DateTime? thoiDiemTao;
        public DateTime? thoiDiemCapNhat;
        public bool duyet;
        public NguoiDungDTO nguoiTao;
        public CauHoiDTO cauHoi;
    }
}
