using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_ChuDe
    {
        public int ma { get; set; }
        public string ten { get; set; }
        public string moTa { get; set; }
        public DateTime ngayTao { get; set; }
        public int soChuDeCon { get; set; }
        public int soKhoaHocCon { get; set; }
        public string hinhAnh { get; set; }
    }
}