﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.WebServices.Client_Model
{
    public class clientmodel_KhoaHoc_DienDan_BinhLuan
    {
        public int ma { get; set; }
        public int diem { get; set; }
        public string noiDung { get; set; }
        public DateTime ngayTao { get; set; }
        public int maNguoiTao { get; set; }
        public string tenNguoiTao { get; set; }
        public string tenTKNguoiTao { get; set; }
        public string hinhAnh { get; set; }
        public int maTapTin { get; set; }
        public string tenTapTin { get; set; }
    }
}