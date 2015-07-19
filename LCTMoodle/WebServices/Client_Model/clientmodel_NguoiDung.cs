using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_NguoiDung
    {
        public int ma { get; set; }
        public string hoTen { get; set; }
        public string tenDN { get; set; }
        public DateTime ngaySinh { get; set; }
        public string email { get; set; }
        public string soDienThoai { get; set; }
        public string diaChi { get; set; }
        public string hinhAnh { get; set; }
    }
}