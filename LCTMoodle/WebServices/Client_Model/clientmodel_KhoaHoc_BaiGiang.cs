using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_KhoaHoc_BaiGiang
    {
        public int ma { get; set; }
        public string nguoiTao { get; set; }
        public string tieuDe { get; set; }
        public string noiDung { get; set; }
        public string tomTat { get; set; }
        public DateTime ngayTao { get; set; }
    }
}