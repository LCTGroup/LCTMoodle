using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class NguoiDungViewDTO : DTO
    {
        public string tenTaiKhoan;
        public string matKhau;
        public string email;
        public string hoTen;
        public DateTime? ngaySinh;
        public string diaChi;
        public string soDienThoai;
        public TapTinViewDTO hinhDaiDien;      
    }
}
