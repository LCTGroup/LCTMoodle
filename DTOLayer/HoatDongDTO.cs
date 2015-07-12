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
        /// HT, ND, CD, HD, KH, Q
        /// </summary>
        public string loaiDoiTuongTacDong;
        public int? maDoiTuongTacDong;
        /// <summary>
        /// HT, ND, CD, HD, KH, Q
        /// </summary>
        public string loaiDoiTuongBiTacDong;
        public int? maDoiTuongBiTacDong;
        public int? maHanhDong;
        public string duongDan;
        public DateTime? thoiDiem;
        public List<GiaTriHoatDongDTO> giaTriHoatDong;
        public LoiNhanHanhDongDTO loiNhanHanhDong;
    }
}
