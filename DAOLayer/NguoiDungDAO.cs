using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;

namespace DAOLayer
{
    public class NguoiDungDAO : DAO
    {       
        public static KetQua them(NguoiDungDataDTO nguoiDung)
        {
            return layGiaTri<int>
            (
                "themNguoiDung",
                new object[] 
                {
                    nguoiDung.tenTaiKhoan,
                    nguoiDung.matKhau,
                    nguoiDung.email,
                    nguoiDung.hoTen,
                    nguoiDung.ngaySinh,
                    nguoiDung.diaChi,
                    nguoiDung.soDienThoai,
                    nguoiDung.maHinhDaiDien
                }
            );
        }
        public static KetQua lay(NguoiDungViewDTO nguoiDung)
        {
            return layDong<NguoiDungDataDTO>
            (
                "layNguoiDung",
                new object[]
                {
                    nguoiDung.tenTaiKhoan
                }
            );
        }
    }
}
