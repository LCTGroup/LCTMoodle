﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class ChuDeDTO : DTO
    {
        public string ten;
        public string moTa;
        public NguoiDungDTO nguoiTao;
        public DateTime? thoiDiemTao;
        public ChuDeDTO cha;
        public TapTinDTO hinhDaiDien;
        public List<ChuDeDTO> con;
        public string cay;
    }
}
