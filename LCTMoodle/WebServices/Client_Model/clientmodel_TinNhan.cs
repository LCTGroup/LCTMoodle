using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_TinNhan
    {
        public int ma { get; set; }
        public bool daDoc { get; set; }
        public string noiDung { get; set; }
        public DateTime ngayGui { get; set; }
        public int maNguoiGui { get; set; }
        public string hoTenNguoiGui { get; set; }
        public string tenTKNguoiGui { get; set; }
        public int gioiTinhNguoiGui { get; set; }
        public string hinhAnhNguoiGui { get; set; }
        public int maNguoiNhan { get; set; }
        public string hoTenNguoiNhan { get; set; }
        public string tenTKNguoiNhan { get; set; }
        public int gioiTinhNguoiNhan { get; set; }
        public string hinhAnhNguoiNhan { get; set; }
    }
}