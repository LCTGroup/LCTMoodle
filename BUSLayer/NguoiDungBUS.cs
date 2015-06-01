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
using Helpers;

namespace BUSLayer
{
    public class NguoiDungBUS : BUS
    {
        public static KetQua kiemTra(NguoiDungDataDTO nguoiDung)
        {
            List<string> thongBao = new List<string>();

            #region Kiểm tra Valid

            if (string.IsNullOrEmpty(nguoiDung.tenTaiKhoan))
            {
                thongBao.Add("Tên tài khoản không được bỏ trống");
            }

            if (string.IsNullOrEmpty(nguoiDung.matKhau))
            {
                thongBao.Add("Mật khẩu không được bỏ trống");
            }

            if (string.IsNullOrEmpty(nguoiDung.email))
            {
                thongBao.Add("Email không được bỏ trống");
            }
            else if (!LCTHelper.laEmail(nguoiDung.email))
            {
                thongBao.Add("Email không hợp lệ");
            }

            if (string.IsNullOrEmpty(nguoiDung.hoTen))
            {
                thongBao.Add("Họ tên không được bỏ trống");
            }

            #endregion

            KetQua ketQua = new KetQua();
            ketQua.trangThai = (thongBao.Count > 0) ? 3 : 0;
            ketQua.ketQua = thongBao;
            return ketQua;
        }

        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("NguoiDung_HinhDaiDien", layInt(form, "HinhDaiDien"));
            
            if (ketQua.trangThai != 0)
            {
                return ketQua;                
            }

            NguoiDungDataDTO nguoiDung = new NguoiDungDataDTO()
            {
                tenTaiKhoan = layString(form, "TenTaiKhoan"),
                matKhau = Helpers.NguoiDungHelper.layMaMD5(layString(form, "MatKhau")),
                email = layString(form, "Email"),
                hoTen = layString(form, "HoTen"),
                ngaySinh = layDateTime(form, "NgaySinh"),
                diaChi = layString(form, "DiaChi"),
                soDienThoai = layString(form, "SoDienThoai"),
                maHinhDaiDien = (ketQua.ketQua as TapTinViewDTO).ma
            };
            
            ketQua = kiemTra(nguoiDung);           

            //Kiểm tra Tên tài khoản có trùng hay ko, nếu trùng thì thêm thất bại
            

            if (ketQua.trangThai == 0)
            {
                ketQua = NguoiDungBUS.kiemTraTenTaiKhoan(nguoiDung.tenTaiKhoan);
                if (ketQua.trangThai == 1)
                {
                    ketQua = NguoiDungDAO.them(nguoiDung);
                    if (ketQua.trangThai == 0)
                    {
                        HttpContext.Current.Session["NguoiDung"] = (int)ketQua.ketQua;
                    }
                }
            }
            return ketQua;
        }
        public static NguoiDungViewDTO layTheoTenTaiKhoan(string tenTaiKhoan)
        {
            return (NguoiDungDAO.layTheoTenTaiKhoan(tenTaiKhoan)).ketQua as NguoiDungViewDTO;
        }
        public static NguoiDungViewDTO layTheoMa(int ma)
        {
            return (NguoiDungDAO.layTheoMa(ma)).ketQua as NguoiDungViewDTO;
        }
        public static KetQua xuLyDangNhap(Dictionary<string,string> form)
        {
            string tenTaiKhoan = layString(form, "TenTaiKhoan");
            string matKhau = layString(form, "MatKhau");
            bool ghiNho = layBool(form, "GhiNho");

            KetQua ketQua = NguoiDungDAO.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai != 0)
            {
                ketQua.ketQua = "Tên tài khoản không tồn tại";
                return ketQua;
            }

            NguoiDungViewDTO nguoiDung = ketQua.ketQua as NguoiDungViewDTO;
            
            if ((matKhau == nguoiDung.matKhau) || (NguoiDungHelper.layMaMD5(matKhau) == nguoiDung.matKhau))
            {
                if (ghiNho)
                {
                    //Lưu Cookie
                    HttpCookie cookieNguoiDung = new HttpCookie("NguoiDung");                    
                    cookieNguoiDung.Values.Add("TenTaiKhoan", nguoiDung.tenTaiKhoan);
                    cookieNguoiDung.Values.Add("MatKhau", nguoiDung.matKhau);
                    cookieNguoiDung.Expires = DateTime.Now.AddDays(7);

                    HttpContext.Current.Response.Cookies.Add(cookieNguoiDung);                    
                }                
                
                //Lưu mã người dùng vào Session
                HttpContext.Current.Session["NguoiDung"] = nguoiDung.ma;

                ketQua.ketQua = nguoiDung;
            }
            else
            {
                ketQua.trangThai = 1; //Mặc dù lấy được người dùng nhưng sai password nên phải cập nhật trạng thái về 1 để ajax thông báo.
                ketQua.ketQua = "Mật khẩu không đúng";
            }
            return ketQua;            
        }
        public static void xuLyDangXuat()
        {
            //Xóa Cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies["NguoiDung"];

            if (cookie != null)
            {
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);                
            }
                       
            //Xóa session            
            HttpContext.Current.Session.Abandon();
        }
        public static KetQua kiemTraTenTaiKhoan(string tenTaiKhoan)
        {
            return NguoiDungDAO.layTheoTenTaiKhoan(tenTaiKhoan);
        }
    }
}