using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class TraLoiViewDTO : DTO
    {
        public string noiDung;
        public DateTime? thoiDiemTao;
        public bool duyet;
        public NguoiDungViewDTO NguoiTao;
        public CauHoiViewDTO CauHoi;
    }
}
