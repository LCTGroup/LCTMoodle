using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class NguoiDungDTO : DTO
    {
        public string tenTaiKhoan;
        public string matKhau;
        public string email;
        public string ho;
        public string ten;
        public DateTime? ngaySinh;
        public string diaChi;
        public string soDienThoai;
        public TapTinDTO hinhDaiDien;      
    }
}
