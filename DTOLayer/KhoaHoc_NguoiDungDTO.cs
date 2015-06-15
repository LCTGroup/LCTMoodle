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
        public KhoaHocDTO khoaHoc;
        public NguoiDungDTO nguoiDung;
        /// <summary>
        /// 1: Thành viên ---
        /// 2: Chờ xác nhận đăng ký ---
        /// 3: Được mời, chờ xác nhận lời mời ---
        /// 4: Bị chặn
        /// </summary>
        public int? trangThai;
        public DateTime? thoiDiemThamGia;
        public NguoiDungDTO nguoiThem;
    }
}