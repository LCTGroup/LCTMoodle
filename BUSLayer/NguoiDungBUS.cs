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
            KetQua ketQua = TapTinBUS.chuyen(layInt(form, "HinhDaiDien"), "NguoiDung_HinhDaiDien");
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            NguoiDungDataDTO nguoiDung = new NguoiDungDataDTO()
            {
                tenTaiKhoan = layString(form, "TenTaiKhoan"),
                matKhau = layString(form, "MatKhau"),
                email = layString(form, "Email"),
                hoTen = layString(form, "HoTen"),
                ngaySinh = layDateTime(form, "NgaySinh"),
                diaChi = layString(form, "DiaChi"),
                soDienThoai = layString(form, "SoDienThoai"),
                maHinhDaiDien = (ketQua.ketQua as TapTinViewDTO).ma
            };
            ketQua = nguoiDung.kiemTra();

            return ketQua.trangThai == 3 ?
                ketQua :
                NguoiDungDAO.them(nguoiDung);
        }
        public static KetQua kiemTraDangNhap(Dictionary<string, string> form)
        {
            NguoiDungDataDTO nguoiDungDangNhap = new NguoiDungDataDTO()
            {
                tenTaiKhoan = layString(form, "TenTaiKhoan"),
                matKhau = layString(form, "MatKhau")
            };
            
            KetQua ketQua = NguoiDungDAO.lay(nguoiDungDangNhap);
            if (ketQua.trangThai == 1)
            {
                ketQua.ketQua = "Tên tài khoản không tồn tại";
                return ketQua;
            }

            NguoiDungViewDTO nguoiDung = ketQua.ketQua as NguoiDungViewDTO;
            
            if (string.Equals(nguoiDung.matKhau, nguoiDungDangNhap.matKhau))
            {
                if (layBool(form, "GhiNho"))
                {            
                    //Lưu mã người dùng vào Session
                    System.Web.HttpContext.Current.Session["NguoiDung"] = nguoiDung.ma;                                       
                }
                ketQua.ketQua = "Đăng nhập thành công";
            }
            else
            {
                ketQua.trangThai = 1;
                ketQua.ketQua = "Mật khẩu không đúng";
            }
            return ketQua;            
        }

    }
}