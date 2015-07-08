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
        public string loaiDoiTuongTacDong;
        public int? maDoiTuongTacDong;
        public string loaiDoiTuongBiTacDong;
        public int? maDoiTuongBiTacDong;
        public int? maHanhDong;
        public DateTime? thoiDiem;
        public List<GiaTriHoatDongDTO> giaTriHoatDong;
        public LoiNhanHanhDongDTO loiNhanHanhDong;
    }
}
