using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class HoatDongDTO : DTO
    {
        public int? maNguoiTacDong;
        /// <summary>
        /// Hệ thống: HT ---
        /// Người dùng: ND ---
        /// Chủ đề: CD ---
        /// Câu hỏi: CH ---
        /// Trả lời: TL ---
        /// Khóa học: KH ---
        /// Quyền: Q
        /// </summary>
        public string loaiDoiTuongTacDong;
        public int? maDoiTuongTacDong;
        /// <summary>
        /// Hệ thống: HT ---
        /// Người dùng: ND ---
        /// Chủ đề: CD ---
        /// Câu hỏi: CH ---
        /// Trả lời: TL ---
        /// Khóa học: KH ---
        /// Quyền: Q
        /// </summary>
        public string loaiDoiTuongBiTacDong;
        public int? maDoiTuongBiTacDong;
        public string duongDan;
        public DateTime? thoiDiem;
        public List<GiaTriHoatDongDTO> giaTriHoatDong;
        public HanhDongDTO hanhDong;
    }
}
