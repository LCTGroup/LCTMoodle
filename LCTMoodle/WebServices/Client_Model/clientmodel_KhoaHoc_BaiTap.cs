using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_KhoaHoc_BaiTap
    {
        public int ma { get; set; }
        public string tieuDe { get; set; }
        public string noiDung { get; set; }
        public int maNguoiTao { get; set; }
        public string tenTaiKhoan { get; set; }
        public int maFile { get; set; }
        public string tenFile { get; set; }
        public DateTime ngayTao { get; set; }
        public DateTime ngayHetHan { get; set; }
        public string hinhAnh { get; set; }
    }
}