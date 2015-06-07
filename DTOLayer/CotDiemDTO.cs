using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class CotDiemDTO : DTO
    {
        public KhoaHocDTO khoaHoc;
        public string ten;
        public string moTa;
        public int? heSo;
        public DateTime? thoiDiem;
    }
}
