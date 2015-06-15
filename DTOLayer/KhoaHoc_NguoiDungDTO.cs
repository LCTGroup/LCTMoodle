using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class KhoaHoc_NguoiDungDTO : DTO
    {
        public NguoiDungDTO nguoiDung;
        public KhoaHocDTO khoaHoc;
        /// <summary>
        /// 0: Thành viên ---
        /// 1: Chờ xác nhận đăng ký ---
        /// 2: Bị chặn
        /// </summary>
        public int? trangThai;
        public DateTime? thoiDiemThamGia;
        public NguoiDungDTO nguoiThem;
    }
}