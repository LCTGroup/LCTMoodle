using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class CotDiem_NguoiDungDTO : DTO
    {
        public CotDiemDTO cotDiem;
        public NguoiDungDTO nguoiDung;
        public NguoiDungDTO nguoiTao;
        public double? diem;
    }
}
