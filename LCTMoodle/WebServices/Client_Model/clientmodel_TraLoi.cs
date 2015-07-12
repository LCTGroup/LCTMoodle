using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_TraLoi
    {
        public int ma { get; set; }
        public string noiDung { get; set; }
        public string nguoiTao { get; set; }
        public DateTime ngayTao { get; set; }
        public DateTime ngayCapNhat { get; set; }
        public bool duyet { get; set; }
        public string hinhAnh { get; set; }
    }
}