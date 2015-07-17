using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Helpers;
using DTOLayer;
using BUSLayer;
using LCTMoodle.WebServices.Client_Model;
using System.Drawing;
using System.IO;

namespace LCTMoodle.WebServices
{
    public class wcf_NguoiDung : Iwcf_NguoiDung
    {
        List<clientmodel_NguoiDung> timKiemNguoiDung(string tuKhoa)
        {
            return null;
        }

        private const string _Loai = "NguoiDung_HinhDaiDien";

        /// <summary>
        /// Webservice lấy hình ảnh
        /// </summary>
        /// <param name="ten"></param>
        /// <returns>byte[] </returns>
        public byte[] layHinhAnh(string ten)
        {
            string _DuongDan = TapTinHelper.layDuongDan(_Loai, ten);

            if (File.Exists(@_DuongDan))
            {
                Image img = Image.FromFile(@_DuongDan);
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// Webservice kiểm tra đăng nhập đăng nhập
        ///  - 1: tên đăng nhập không tồn tại
        ///  - 2: mật khẩu không đúng
        ///  - 3: thành công
        /// </summary>
        /// <param name="tenDN"></param>
        /// <param name="matKhau"></param>
        /// <returns>int</returns>
        public int kiemTraDangNhap(string tenDN, string matKhau)
        {
            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenDN);
            NguoiDungDTO dto_NguoiDung = ketQua.ketQua as NguoiDungDTO;

            if (ketQua.trangThai != 0)
            {
                return 1; //Tên đăng nhập không tồn tại
            }
            else
            {
                if (matKhau == dto_NguoiDung.matKhau || NguoiDungHelper.layMaMD5(matKhau) == dto_NguoiDung.matKhau)
                {
                    return 3; //Đúng
                }
                else
                {
                    return 2; //Sai Mật Khẩu
                }
            }
        }

        /// <summary>
        /// Webservice lấy người dùng theo mã
        /// </summary>
        /// <param name="ma"></param>
        /// <returns>clientmodel_NguoiDung</returns>
        public clientmodel_NguoiDung layNguoiDungTheoMa(int ma)
        {
            KetQua ketQua = NguoiDungBUS.layTheoMa(ma, new LienKet { "HinhDaiDien" });
            NguoiDungDTO dto_NguoiDung = ketQua.ketQua as NguoiDungDTO;
            clientmodel_NguoiDung cm_NguoiDung = new clientmodel_NguoiDung();

            if (ketQua.trangThai == 0)
            {
                if (dto_NguoiDung.ma != null)
                {
                    cm_NguoiDung.ma = dto_NguoiDung.ma.Value;
                }

                if (dto_NguoiDung.ho != null && dto_NguoiDung.ten != null)
                {
                    if (dto_NguoiDung.tenLot != null)
                    {
                        cm_NguoiDung.hoTen = string.Format("{0} {1} {2}", dto_NguoiDung.ho, dto_NguoiDung.tenLot, dto_NguoiDung.ten);
                    }
                    else
                    {
                        cm_NguoiDung.hoTen = string.Format("{0} {1}", dto_NguoiDung.ho, dto_NguoiDung.ten);
                    }
                }

                if (dto_NguoiDung.tenTaiKhoan != null)
                {
                    cm_NguoiDung.tenDN = dto_NguoiDung.tenTaiKhoan;
                }

                if (dto_NguoiDung.ngaySinh != null)
                {
                    cm_NguoiDung.ngaySinh = dto_NguoiDung.ngaySinh.Value;
                }

                if (dto_NguoiDung.email != null)
                {
                    cm_NguoiDung.email = dto_NguoiDung.email;
                }

                if (dto_NguoiDung.soDienThoai != null)
                {
                    cm_NguoiDung.soDienThoai = dto_NguoiDung.soDienThoai;
                }

                if (dto_NguoiDung.diaChi != null)
                {
                    cm_NguoiDung.diaChi = dto_NguoiDung.diaChi;
                }

                if (dto_NguoiDung.hinhDaiDien.ma != null && dto_NguoiDung.hinhDaiDien.duoi != null)
                {
                    cm_NguoiDung.hinhAnh = dto_NguoiDung.hinhDaiDien.ma.Value + dto_NguoiDung.hinhDaiDien.duoi;
                }
            }

            return cm_NguoiDung;
        }

        /// <summary>
        /// Webservice đăng ký thành viên
        /// </summary>
        /// <param name="tenDN"></param>
        /// <param name="matKhau"></param>
        /// <param name="email"></param>
        /// <param name="hoTen"></param>
        /// <param name="ngaySinh"></param>
        /// <param name="chapNhanDieuKhoan">0,1</param>
        /// <param name="duLieuAnh"></param>
        /// <param name="tenAnh"></param>
        /// <param name="contenttype"></param>
        /// <returns></returns>
        public clientmodel_DangKy dangKy(string tenDN, string matKhau, string email, string hoTen, DateTime ngaySinh, int chapNhanDieuKhoan, byte[] duLieuAnh = null, string tenAnh = null, string contenttype = null)
        {
            int maHinh = -1;
            KetQua ketQuaLuuHinh = TapTinBUS.them(duLieuAnh, tenAnh, 0, contenttype);
            TapTinDTO dto_TapTin = ketQuaLuuHinh.ketQua as TapTinDTO;

            if (ketQuaLuuHinh.trangThai == 0)
            {
                maHinh = dto_TapTin.ma.Value;    
            }

            string ho = layHo(hoTen);
            string tenLot = layTenLot(hoTen);
            string ten = layTen(hoTen);

            Form form;
            if(maHinh != -1)
            {
                form = new Form()
                {
                    {"TenTaiKhoan",tenDN},
                    {"MatKhau",matKhau},
                    {"Email",email},
                    {"Ho",ho},
                    {"TenLot",tenLot},
                    {"Ten",ten},
                    {"NgaySinh",ngaySinh.ToShortDateString()},
                    {"ChapNhanQuyDinh",chapNhanDieuKhoan.ToString()},
                    {"MaHinhDaiDien",maHinh.ToString()},
                };
            }
            else
            {
                form = new Form()
                {
                    {"TenTaiKhoan",tenDN},
                    {"MatKhau",matKhau},
                    {"Email",email},
                    {"Ho",ho},
                    {"TenLot",tenLot},
                    {"Ten",ten},
                    {"NgaySinh",ngaySinh.ToShortDateString()},
                    {"ChapNhanQuyDinh",chapNhanDieuKhoan.ToString()},
                };
            }


            KetQua ketQua = NguoiDungBUS.them(form);
            clientmodel_DangKy cm_DangKy = new clientmodel_DangKy();

            if(ketQua.trangThai == 0)
            {
                cm_DangKy.ma = (int)ketQua.ketQua;
                cm_DangKy.trangThai = ketQua.trangThai;
            }
            else
            {
                cm_DangKy.trangThai = ketQua.trangThai;
                cm_DangKy.lst_ThongBao = ketQua.ketQua as List<string>;
            }

            return cm_DangKy;
        }

        /// <summary>
        /// Webservice kiểm tra đăng nhập va trả vê thông báo
        /// </summary>
        /// <param name="tenDN"></param>
        /// <param name="matKhau"></param>
        /// <returns>clientmodel_NguoiDung</returns>
        public clientmodel_DangNhap dangNhap(string tenDN, string matKhau)
        {
            KetQua ketQua = NguoiDungBUS.xuLyDangNhap(tenDN, matKhau);
            NguoiDungDTO dto_NguoiDung = ketQua.ketQua as NguoiDungDTO;
            clientmodel_DangNhap cm_NguoiDung = new clientmodel_DangNhap();

            if(ketQua.trangThai == 0)
            {
                cm_NguoiDung.ma = dto_NguoiDung.ma.Value;
                cm_NguoiDung.trangThai = ketQua.trangThai;
                cm_NguoiDung.thongBao = "Đăng nhập thành công";
            }
            else
            {
                cm_NguoiDung.thongBao = ketQua.ketQua as string;
                cm_NguoiDung.trangThai = ketQua.trangThai;
            }

            return cm_NguoiDung;
        }

        /// <summary>
        /// Webservice cập nhật thông tin người dùng
        /// </summary>
        /// <param name="maNguoiDung"></param>
        /// <param name="gioiTinh"></param>
        /// <param name="hoTen"></param>
        /// <param name="ngaySinh"></param>
        /// <param name="diaChi"></param>
        /// <param name="duLieuAnh"></param>
        /// <param name="tenAnh"></param>
        /// <param name="contenttype"></param>
        /// <returns>clientmodel_DangKy</returns>
        public clientmodel_DangKy capNhatNguoiDung(string maNguoiDung, int gioiTinh, string hoTen, DateTime ngaySinh, string soDienThoai, string diaChi, byte[] duLieuAnh = null, string tenAnh = null, string contenttype = null)
        {
            int maHinh = -1;
            KetQua ketQuaLuuHinh = TapTinBUS.them(duLieuAnh, tenAnh, 0, contenttype);
            TapTinDTO dto_TapTin = ketQuaLuuHinh.ketQua as TapTinDTO;

            if (ketQuaLuuHinh.trangThai == 0)
            {
                maHinh = dto_TapTin.ma.Value;
            }

            Form form;
            if(maHinh != -1)
            {
                form = new Form()
                {
                    {"Ma",maNguoiDung.ToString()},
                    {"GioiTinh",gioiTinh.ToString()},
                    {"Ho",layHo(hoTen)},
                    {"TenLot",layTenLot(hoTen)},
                    {"Ten",layTen(hoTen)},
                    {"NgaySinh",ngaySinh.ToShortDateString()},
                    {"SoDienThoai",soDienThoai},
                    {"DiaChi",diaChi},
                    {"MaHinhDaiDien",maHinh.ToString()},
                };
            }
            else
            {
                form = new Form()
                {
                    {"Ma",maNguoiDung.ToString()},
                    {"GioiTinh",gioiTinh.ToString()},
                    {"Ho",layHo(hoTen)},
                    {"TenLot",layTenLot(hoTen)},
                    {"Ten",layTen(hoTen)},
                    {"NgaySinh",ngaySinh.ToShortDateString()},
                    {"SoDienThoai",soDienThoai},
                    {"DiaChi",diaChi},
                };
            }

            KetQua ketQua = NguoiDungBUS.capNhat(form);
            clientmodel_DangKy cm_CapNhat = new clientmodel_DangKy();

            if (ketQua.trangThai == 0)
            {
                cm_CapNhat.trangThai = ketQua.trangThai;
            }
            else
            {
                cm_CapNhat.trangThai = ketQua.trangThai;
                cm_CapNhat.lst_ThongBao = ketQua.ketQua as List<string>;
            }

            return cm_CapNhat;
        }

        /// <summary>
        /// Webservice thay đổi mật khẩu
        /// </summary>
        /// <param name="tenTaiKhoan"></param>
        /// <param name="matKhauCu"></param>
        /// <param name="matKhauMoi"></param>
        /// <returns>clientmodel_DoiMatKhau</returns>
        public clientmodel_ThongBao thayDoiMatKhau(string tenTaiKhoan, string matKhauCu, string matKhauMoi)
        {
            KetQua ketQua = NguoiDungBUS.xuLyDoiMatKhau(tenTaiKhoan, matKhauCu, matKhauMoi);
            clientmodel_ThongBao cm_DoiMatKhau = new clientmodel_ThongBao();

            if(ketQua.trangThai == 0)
            {
                cm_DoiMatKhau.trangThai = ketQua.trangThai;
                cm_DoiMatKhau.thongBao = "Thành Công";
            }
            else
            {
                cm_DoiMatKhau.trangThai = ketQua.trangThai;
                cm_DoiMatKhau.thongBao = ketQua.ketQua as string;
            }

            return cm_DoiMatKhau;
        }

        /// <summary>
        /// Webservice kích hoạt tài khoản
        /// </summary>
        /// <param name="tenTaiKhoan"></param>
        /// <param name="matKhau"></param>
        /// <param name="maKichHoat"></param>
        /// <returns>clientmodel_ThongBao</returns>
        public clientmodel_ThongBao kichHoatTaiKhoan(string tenTaiKhoan, string matKhau, string maKichHoat)
        {
            Form form = new Form()
            {
                {"TenTaiKhoan",tenTaiKhoan},
                {"MatKhau",matKhau},
                {"MaKichHoat",maKichHoat},
            };

            KetQua ketQua = NguoiDungBUS.kichHoatTaiKhoan(form);
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();

            if(ketQua.trangThai == 0)
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = "Thành Công";
            }
            else
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = ketQua.ketQua as string;
            }

            return cm_ThongBao;
        }

        /// <summary>
        /// Webservice tìm kiếm người dùng theo từ khóa
        /// </summary>
        /// <param name="tuKhoa"></param>
        /// <returns>List<clientmodel_NguoiDung></returns>
        public List<clientmodel_NguoiDung> timKiemNguoiDung(string tuKhoa)
        {
            KetQua ketQua = NguoiDungBUS.timKiem(tuKhoa, new LienKet { "HinhDaiDien" });
            List<clientmodel_NguoiDung> lst_NguoiDung = new List<clientmodel_NguoiDung>();

            if(ketQua.trangThai == 0)
            {
                foreach(var nguoiDung in ketQua.ketQua as List<NguoiDungDTO>)
                {
                    if(nguoiDung.ma != null)
                    {
                        lst_NguoiDung.Add(new clientmodel_NguoiDung()
                        {
                            ma = nguoiDung.ma.Value,
                        });
                    }

                    if(nguoiDung.tenLot != null)
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].hoTen = string.Format("{0} {1} {2}", nguoiDung.ho, nguoiDung.tenLot, nguoiDung.ten);
                    }
                    else
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].hoTen = string.Format("{0} {1}", nguoiDung.ho, nguoiDung.ten);
                    }

                    if(nguoiDung.tenTaiKhoan != null)
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].tenDN = nguoiDung.tenTaiKhoan;
                    }

                    if(nguoiDung.ngaySinh != null)
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].ngaySinh = nguoiDung.ngaySinh.Value;
                    }

                    if(nguoiDung.email != null)
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].email = nguoiDung.email;
                    }

                    if(nguoiDung.soDienThoai != null)
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].soDienThoai = nguoiDung.soDienThoai;
                    }

                    if(nguoiDung.diaChi != null)
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].diaChi = nguoiDung.diaChi;
                    }

                    if(nguoiDung.hinhDaiDien != null)
                    {
                        lst_NguoiDung[lst_NguoiDung.Count - 1].hinhAnh = nguoiDung.hinhDaiDien.ma.Value + nguoiDung.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_NguoiDung;
        }

        public int themAnhDaiDien(byte[] hinhAnh, string tenAnh, int nguoiTao, string contenttype)
        {
            KetQua ketQua = TapTinBUS.them(hinhAnh, tenAnh, 0, contenttype);
            
            if(ketQua.trangThai == 0)
            {
                TapTinDTO dtoTapTin = ketQua.ketQua as TapTinDTO;
                return dtoTapTin.ma.Value;
            }

            return -1;
        }

        private string layTenLot(string hoTen)
        {
            if (hoTen.Split(' ').Count() > 2)
            {
                int dau = layHo(hoTen).Length + 1;
                int cuoi = hoTen.Length - layTen(hoTen).Length - layHo(hoTen).Length - 2;
                return hoTen.Substring(dau, cuoi);
            }
            return "";
        }

        private string layHo(string hoTen)
        {
            string[] chuoi = hoTen.Split(' ');
            return chuoi[0];
        }

        private string layTen(string hoTen)
        {
            string[] chuoi = hoTen.Split(' ');
            return chuoi[chuoi.Count() - 1];
        }
    }
}
