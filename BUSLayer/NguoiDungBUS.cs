using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

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
                    //Lưu Cookie
                    HttpCookie cookieNguoiDung = new HttpCookie("NguoiDung");                    
                    cookieNguoiDung.Values.Add("TenTaiKhoan", nguoiDung.tenTaiKhoan);
                    cookieNguoiDung.Values.Add("MatKhau", nguoiDung.matKhau);
                    cookieNguoiDung.Expires = DateTime.Now.AddDays(7);

                    HttpContext.Current.Response.Cookies.Add(cookieNguoiDung);
                }                
                //Lưu mã người dùng vào Session
                HttpContext.Current.Session["NguoiDung"] = nguoiDung.tenTaiKhoan;

                ketQua.ketQua = "Đăng nhập thành công";
            }
            else
            {
                ketQua.trangThai = 1;
                ketQua.ketQua = "Mật khẩu không đúng";
            }
            return ketQua;            
        }
        public static void xuLyDangXuat()
        {
            //Xóa Cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies["NguoiDung"];            
            cookie.Value = null;
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);            

            //Xóa session            
            HttpContext.Current.Session.Abandon();
        }
        public static void kiemTraCookie()
        {
            if (HttpContext.Current.Request.Cookies["NguoiDung"] != null && HttpContext.Current.Session["NguoiDung"] == null)
            {               
                Dictionary<string, string> formDangNhap = new Dictionary<string, string>();

                formDangNhap.Add("TenTaiKhoan", HttpContext.Current.Request.Cookies["NguoiDung"]["TenTaiKhoan"]);
                formDangNhap.Add("MatKhau", HttpContext.Current.Request.Cookies["NguoiDung"]["MatKhau"]);

                KetQua ketQua = NguoiDungBUS.kiemTraDangNhap(formDangNhap);

                if (ketQua.trangThai == 0)
                {
                    HttpContext.Current.Session["NguoiDung"] = formDangNhap["TenTaiKhoan"];
                }            
            }
        }
    }
}