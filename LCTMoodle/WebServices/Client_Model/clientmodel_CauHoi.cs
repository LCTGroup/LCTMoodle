using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_CauHoi
    {
        public int ma { get; set; }
        public string tieuDe { get; set; }
        public string noiDung { get; set; }
        public string nguoiTao { get; set; }
        public int soTraLoi { get; set; }
        public DateTime ngayTao { get; set; }
        public DateTime ngayCapNhat { get; set; }
        public string hinhAnh { get; set; }
    }
}