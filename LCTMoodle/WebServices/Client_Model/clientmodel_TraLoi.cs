using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_TraLoi
    {
        public int Ma { get; set; }
        public string NoiDung { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string NgayCapNhat { get; set; }
        public bool Duyet { get; set; }
        public string HinhAnh { get; set; }
    }
}