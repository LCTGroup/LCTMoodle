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
            NguoiDungDataDTO nguoiDung = new NguoiDungDataDTO()
            { 
                tenTaiKhoan = layString(form, "TenTaiKhoan"),
                matKhau = layString(form, "MatKhau"),
                email = layString(form, "Email"),
                hoTen = layString(form, "HoTen"),
                ngaySinh = layDateTime(form, "NgaySinh"),
                diaChi = layString(form, "DiaChi"),
                soDienThoai = layString(form, "SoDienThoai")
            };
            KetQua ketQua = nguoiDung.kiemTra();
            
            return ketQua.trangThai == 3 ?
                ketQua : 
                NguoiDungDAO.them(nguoiDung);            
        }
    }
}
