using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class CauHoiViewDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public DateTime? thoiDiemTao;
        public NguoiDungViewDTO nguoiTao; 
    }
}
