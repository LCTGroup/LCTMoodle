using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class CauHoiDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public DateTime? thoiDiemTao;
        public DateTime? thoiDiemCapNhat;
        public NguoiDungDTO nguoiTao;
        public ChuDeDTO chuDe;
        public List<TraLoiDTO> danhSachTraLoi;
    }
}
