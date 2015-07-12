using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class TinNhanDTO : DTO
    {
        public NguoiDungDTO nguoiGui;
        public NguoiDungDTO nguoiNhan;
        public string noiDung;
        public DateTime? thoiDiemGui;
        public bool daDoc;
    }
}
