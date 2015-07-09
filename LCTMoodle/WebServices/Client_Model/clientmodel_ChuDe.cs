using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_ChuDe
    {
        public int Ma { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public string NgayTao { get; set; }
        public int SoChuDeCon { get; set; }
        public int SoKhoaHocCon { get; set; }
        public string HinhAnh { get; set; }
    }
}