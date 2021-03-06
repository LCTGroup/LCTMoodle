﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiVietBaiTapDTO : DTO
    {
        public string tieuDe;
        public string noiDung;
        public TapTinDTO tapTin;
        /// <summary>
        /// 0: Tham khảo --- 1: Điểm cộng --- 2: Điểm
        /// </summary>
        public int? loai;
        /// <summary>
        /// 0: Tập tin hoặc đường dẫn --- 1: Tập tin --- Đường dẫn
        /// </summary>
        public int? cachNop;
        public DateTime? thoiDiemHetHan;
        public DateTime? thoiDiemTao;
        public NguoiDungDTO nguoiTao;
        public KhoaHocDTO khoaHoc;
        public List<BaiTapNopDTO> danhSachBaiTapNop;
        public string danhSachMaThanhVienDaXem;
    }
}
