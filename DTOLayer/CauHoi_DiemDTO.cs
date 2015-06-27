using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class CauHoi_DiemDTO : DTO
    {
        public CauHoiDTO cauHoi;
        public NguoiDungDTO nguoiTao;
        public bool diem;
    }
}
