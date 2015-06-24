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
        public static void gan(ref NguoiDungDTO nguoiDung, Form form)
        {
            if (nguoiDung == null)
            {
                nguoiDung = new NguoiDungDTO();
            }
            foreach(var key in form.Keys.ToArray())
            {
                switch(key)
                {
                    case "TenTaiKhoan":
                        nguoiDung.tenTaiKhoan = form.layString(key);
                        break;
                    case "MatKhau":
                        nguoiDung.matKhau = NguoiDungHelper.layMaMD5(form.layString(key));
                        break;
                    case "Email":
                        nguoiDung.email = form.layString(key);
                        break;
                    case "GioiTinh":
                        nguoiDung.gioiTinh = form.layInt(key);
                        break;
                    case "Ho":
                        nguoiDung.ho = form.layString(key);
                        break;
                    case "TenLot":
                        nguoiDung.tenLot = form.layString(key);
                        break;
                    case "Ten":
                        nguoiDung.ten = form.layString(key);
                        break;
                    case "NgaySinh":
                        nguoiDung.ngaySinh = form.layDate(key);
                        break;
                    case "DiaChi":
                        nguoiDung.diaChi = form.layString(key);
                        break;
                    case "SoDienThoai":
                        nguoiDung.soDienThoai = form.layString(key);
                        break;
                    case "MaHinhDaiDien":
                        nguoiDung.hinhDaiDien = TapTinBUS.chuyen("NguoiDung_HinhDaiDien", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "MatKhauCap2":
                        nguoiDung.matKhauCap2 = NguoiDungHelper.layMaMD5(form.layString(key));
                        break;
                    default: 
                        break;
                }
            }
        }

        public static KetQua kiemTra(NguoiDungDTO nguoiDung, string[] truong = null, bool kiemTra = true)
        {
            List<string> thongBao = new List<string>();

            #region Kiểm tra Valid

            if (coKiemTra("TenTaiKhoan", truong, kiemTra) && NguoiDungBUS.tonTaiTenTaiKhoan(nguoiDung.tenTaiKhoan))
            {
                thongBao.Add("Tên tài khoản bị trùng");
            }
            if (coKiemTra("TenTaiKhoan", truong, kiemTra) && string.IsNullOrEmpty(nguoiDung.tenTaiKhoan))
            {
                thongBao.Add("Tên tài khoản không được bỏ trống");
            }

            if (coKiemTra("MatKhau", truong, kiemTra) && string.IsNullOrEmpty(nguoiDung.matKhau))
            {
                thongBao.Add("Mật khẩu không được bỏ trống");
            }

            if (coKiemTra("Email", truong, kiemTra) && string.IsNullOrEmpty(nguoiDung.email))
            {
                thongBao.Add("Email không được bỏ trống");
            }
            else if (!LCTHelper.laEmail(nguoiDung.email))
            {
                thongBao.Add("Email không hợp lệ");
            }

            if (coKiemTra("Ho", truong, kiemTra) && string.IsNullOrEmpty(nguoiDung.ho))
            {
                thongBao.Add("Họ không được bỏ trống");
            }

            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrEmpty(nguoiDung.ho))
            {
                thongBao.Add("Tên không được bỏ trống");
            }

            if (coKiemTra("MatKhauCap2", truong, kiemTra) && string.IsNullOrEmpty(nguoiDung.matKhau))
            {
                thongBao.Add("Mật khẩu cấp 2 không được bỏ trống");
            }
            #endregion

            KetQua ketQua = new KetQua();
            ketQua.trangThai = (thongBao.Count > 0) ? 3 : 0;
            ketQua.ketQua = thongBao;
            return ketQua;
        }

        public static BangCapNhat layBangCapNhat(NguoiDungDTO nguoiDung, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "GioiTinh":
                        bangCapNhat.Add("GioiTinh", nguoiDung.gioiTinh.ToString(), 1);
                        break;
                    case "Ho":
                        bangCapNhat.Add("Ho", nguoiDung.ho, 2);
                        break;
                    case "TenLot":
                        bangCapNhat.Add("TenLot", nguoiDung.tenLot, 2);
                        break;
                    case "Ten":
                        bangCapNhat.Add("Ten", nguoiDung.ten, 2);
                        break;
                    case "NgaySinh":
                        bangCapNhat.Add("NgaySinh", nguoiDung.ngaySinh.HasValue ? nguoiDung.ngaySinh.Value.ToString("d/M/yyyy") : null, 3);
                        break;
                    case "DiaChi":
                        bangCapNhat.Add("DiaChi", nguoiDung.diaChi, 2);
                        break;
                    case "SoDienThoai":
                        bangCapNhat.Add("SoDienThoai", nguoiDung.soDienThoai, 2);
                        break;
                    case "MaHinhDaiDien":
                        bangCapNhat.Add("MaHinhDaiDien", layMa_String(nguoiDung.hinhDaiDien), 2);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua them(Form form)
        {
            bool chapNhanQuyDinh = form.layBool("ChapNhanQuyDinh");

            if (!chapNhanQuyDinh)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Chấp nhận quy định của LCTMoodle bạn mới có thể trở thành thành viên của hệ thống"
                };
            }

            string maKichHoat = NguoiDungHelper.PhatSinhMaKichHoat();
            NguoiDungDTO nguoiDung = new NguoiDungDTO() 
            {
                maKichHoat = maKichHoat
            };
            gan(ref nguoiDung, form);
            
            KetQua ketQua = kiemTra(nguoiDung);           

            if (ketQua.trangThai == 0)
            {
                string duongDanKichHoat = @"http://localhost:1711/NguoiDung/KichHoat?tenTaiKhoan=" + form.layString("TenTaiKhoan");
                ketQua = NguoiDungDAO.them(nguoiDung);
                if (ketQua.trangThai == 0)
                {
                    string mailDangKy = form.layString("Email");
                    string tieuDe = "Kích hoạt tài khoản LCTMoodle";
                    string noiDung = "Mã kích hoạt tài khoản của bạn là: <b>" + maKichHoat +
                        "</b><br><br>Đường dẫn kích hoạt: <a href=\"" + duongDanKichHoat + "\">Nhấp vào đây</a>";

                    NguoiDungHelper.guiEmail(tieuDe, noiDung, mailDangKy);
                }
            }
            return ketQua;
        }

        public static KetQua capNhat(Form form)
        {
            int? ma = form.layInt("Ma");

            KetQua ketQua = NguoiDungBUS.layTheoMa(ma.Value);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;            
            
            gan(ref nguoiDung, form);
            
            ketQua = NguoiDungBUS.kiemTra(nguoiDung, form.Keys.ToArray());
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return NguoiDungDAO.capNhat(ma, layBangCapNhat(nguoiDung, form.Keys.ToArray()));
        }
        
        public static KetQua kichHoatTaiKhoan(Form form)
        {
            string tenTaiKhoan = form.layString("TenTaiKhoan");
            string matKhau = form.layString("MatKhau");
            string maKichHoat = form.layString("MaKichHoat");
            
            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;
            if (nguoiDung.maKichHoat != maKichHoat)
            {
                ketQua.trangThai = 3;
                ketQua.ketQua = "Mã kích hoạt không đúng";
                return ketQua;
            }

            NguoiDungDAO.kichHoatTaiKhoanTheoTenTaiKhoan(tenTaiKhoan, null);

            ketQua = NguoiDungBUS.xuLyDangNhap(tenTaiKhoan, matKhau, true);
            if (ketQua.trangThai != 0)
            {
                //Cập nhật lại mã kích hoạt cũ để trạng thái tài khoản trở thành như ban đầu
                NguoiDungDAO.kichHoatTaiKhoanTheoTenTaiKhoan(tenTaiKhoan, maKichHoat);
                
                ketQua.trangThai = 3;
                ketQua.ketQua = "Sai Mật khẩu";
                return ketQua;
            }

            
            return ketQua;            
        }

        public static KetQua layTheoTenTaiKhoan(string tenTaiKhoan)
        {
            return NguoiDungDAO.layTheoTenTaiKhoan(tenTaiKhoan);
        }
        
        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return NguoiDungDAO.layTheoMa(ma, lienKet);
        }

        //Hàm cùng tên
        public static KetQua xuLyDangNhap(string tenTaiKhoan, string matKhau, bool ghiNho = false)
        {
            if (!tonTaiTenTaiKhoan(tenTaiKhoan))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Tài khoản không tồn tài"
                };
            }
            
            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            
            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;
            if (nguoiDung.maKichHoat != null)
            {
                return new KetQua()
                {
                    trangThai = 5,
                    ketQua = "Tài khoản chưa được kích hoạt. Bạn vui lòng kiểm tra email để kích hoạt tài khoản."
                };
            }

            if ((NguoiDungHelper.layMaMD5(matKhau) == nguoiDung.matKhau) || (matKhau == nguoiDung.matKhau))
            {
                //Lưu mã người dùng vào Session
                HttpContext.Current.Session["NguoiDung"] = nguoiDung.ma;

                if (ghiNho)
                {
                    //Lưu Cookie
                    HttpCookie cookieNguoiDung = new HttpCookie("NguoiDung");
                    cookieNguoiDung.Values.Add("TenTaiKhoan", nguoiDung.tenTaiKhoan);
                    cookieNguoiDung.Values.Add("MatKhau", nguoiDung.matKhau);
                    cookieNguoiDung.Expires = DateTime.Now.AddDays(7);

                    HttpContext.Current.Response.Cookies.Add(cookieNguoiDung);
                }

                ketQua.ketQua = nguoiDung;
                return ketQua;
            }
            
            ketQua.trangThai = 3;
            ketQua.ketQua = "Mật khẩu không đúng";
            
            return ketQua;
        }

        public static KetQua xuLyDangNhap(Form form)
        {
            string tenTaiKhoan = form.layString("TenTaiKhoan");
            string matKhau = form.layString("MatKhau");
            bool ghiNho = form.layBool("GhiNho");
            
            return NguoiDungBUS.xuLyDangNhap(tenTaiKhoan, matKhau, ghiNho);
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
            HttpContext.Current.Session.Clear();
        }
        
        public static bool tonTaiTenTaiKhoan(string tenTaiKhoan)
        {
            return NguoiDungDAO.layTheoTenTaiKhoan(tenTaiKhoan).trangThai == 0 ? true : false;
        }

        public static KetQua layTheoMaNhomNguoiDung(string phamVi, int maNhomNguoiDung)
        {
            return NguoiDungDAO.layTheoMaNhomNguoiDung(phamVi, maNhomNguoiDung);
        }

        public static KetQua kiemTraDangNhap()
        {
            if (Session["NguoiDung"] == null)
            {
                return new KetQua()
                {
                    trangThai = 4,
                    ketQua = "Chưa đăng nhập"
                };
            }
            return new KetQua()
            {
                trangThai = 0,
                ketQua = "Đã đăng nhập"
            };
        }

        public static KetQua kiemTraCookie()
        {
            HttpCookie ckNguoiDung = HttpContext.Current.Request.Cookies.Get("NguoiDung");
            
            if (ckNguoiDung == null)
            {
                return new KetQua()
                {
                    trangThai = 4,
                    ketQua = "Không tồn tại Cookie"
                };
            }
            
            Form formNguoiDung = new Form() 
            {
                { "TenTaiKhoan", ckNguoiDung["TenTaiKhoan"] },
                { "MatKhau", ckNguoiDung["MatKhau"] },
                { "GhiNho", "1" }
            };

            return NguoiDungBUS.xuLyDangNhap(formNguoiDung);
        }
    }
}