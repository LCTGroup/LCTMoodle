using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class ChuongTrinhDTO : DTO
    {
        public KhoaHocDTO khoaHoc;
        public string congViec;
        public string moTa;
        public string thoiGian;
        public int? thuTu;
    }
}
