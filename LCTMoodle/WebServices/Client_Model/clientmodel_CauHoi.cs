using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_CauHoi
    {
        public int Ma { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string NguoiTao { get; set; }
        public int SoTraLoi { get; set; }
        public string HinhAnh { get; set; }
    }
}