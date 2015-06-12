using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class NhomNguoiDungDTO : DTO
    {
        public string ten;
        public string moTa;
        public string phamVi;
        public DTO doiTuong;
        public NguoiDungDTO nguoiTao;
    }
}
