
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_KhoaHoc
    {
        public int ma { get; set; }
        public string ten { get; set; }
        public string moTa { get; set; }
        public string nguoiTao { get; set; }
        public DateTime ngayTao { get; set; }
        public DateTime ngayHetHan { get; set; }
        public string hinhAnh { get; set; }
    }
}