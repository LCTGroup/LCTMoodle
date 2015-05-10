using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;

namespace BUSLayer
{
    public class NguoiDungBUS : BUS
    {
        public static KetQua them(Dictionary<string, string> form)
        {
            return NguoiDungDAO.them(new NguoiDungDataDTO() 
            { 
                tenTaiKhoan = form["TenTaiKhoan"],
                matKhau = form["MatKhau"],
                email = form["Email"],
                hoTen = form["HoTen"],
                ngaySinh = Convert.ToDateTime(form["NgaySinh"]),
                diaChi = form["DiaChi"],
                soDienThoai = form["SoDienThoai"]
            });
        }
    }
}
