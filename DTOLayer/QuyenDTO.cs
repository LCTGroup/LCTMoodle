using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class QuyenDTO : DTO
    {
        public string ten;
        public string moTa;
        public string giaTri;
        public string phamVi;
        public QuyenDTO cha;
        public List<QuyenDTO> con;
    }
}
