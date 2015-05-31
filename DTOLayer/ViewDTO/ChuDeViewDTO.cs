using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class ChuDeViewDTO : DTO
    {
        public string ten;
        public string moTa;
        public int maNguoiTao;
        public DateTime? thoiDiemTao;
        public string phamVi;
        public ChuDeViewDTO chuDeCha;
        public TapTinViewDTO hinhDaiDien;
        public List<ChuDeViewDTO> danhSachChuDeCon;
    }
}
