using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_KhoaHoc_BaiTap_Nop
    {
        public int ma { get; set; }
        public int  maNguoitao { get; set; }
        public string  tenTaiKhoan { get; set; }
        public string tenNguoiNop { get; set; }
        public string anhNguoiTao { get; set; }
        public string tenFile { get; set; }
        public int  maFile { get; set; }
        public DateTime ngayTao { get; set; }
        public double diem { get; set; }
        public string duongDan { get; set; }
        public string ghiChu { get; set; }
    }
}