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


        clientmodel_DangNhap dangKy(string tenDN, string matKhau, string email, string hoTen, DateTime ngaySinh, int maHinh)
        {
            Form form = new Form()
            {
                {"TenTaiKhoan","hp94"},
                {"MatKhau","huyphong"},
                {"Email","benny9407@yahoo.com.vn"},
                {"Ho","Nguyen"},
                {"TenLot","Huy"},
                {"Ten","Phong"},
                {"NgaySinh","14/02/1994"},
                {"ChapNhanQuyDinh","1"},
            };

            

            KetQua ketQua = NguoiDungBUS.them(form);
            clientmodel_DangNhap cm_NguoiDung = new clientmodel_DangNhap();

            if(ketQua.trangThai == 0)
            {

            }
            else
            {
                
            }

            return cm_NguoiDung;
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

            if(ketQua.trangThai == 0)
            {
                if(dto_NguoiDung.ma != null)
                {
                    cm_NguoiDung.ma = dto_NguoiDung.ma.Value;
                }

                if(dto_NguoiDung.ho != null && dto_NguoiDung.ten != null)
                {
                    if(dto_NguoiDung.tenLot != null)
                    {
                        cm_NguoiDung.hoTen = string.Format("{0} {1} {2}", dto_NguoiDung.ho, dto_NguoiDung.tenLot, dto_NguoiDung.ten);
                    }
                    else
                    {
                        cm_NguoiDung.hoTen = string.Format("{0} {1}", dto_NguoiDung.ho, dto_NguoiDung.ten);
                    }
                }

                if(dto_NguoiDung.tenTaiKhoan != null)
                {
                    cm_NguoiDung.tenDN = dto_NguoiDung.tenTaiKhoan;
                }

                if(dto_NguoiDung.ngaySinh != null)
                {
                    cm_NguoiDung.ngaySinh = dto_NguoiDung.ngaySinh.Value;
                }

                if(dto_NguoiDung.email != null)
                {
                    cm_NguoiDung.email = dto_NguoiDung.email;
                }

                if(dto_NguoiDung.soDienThoai != null)
                {
                    cm_NguoiDung.soDienThoai = dto_NguoiDung.soDienThoai;
                }

                if(dto_NguoiDung.diaChi != null)
                {
                    cm_NguoiDung.diaChi = dto_NguoiDung.diaChi;
                }

                if(dto_NguoiDung.hinhDaiDien.ma != null && dto_NguoiDung.hinhDaiDien.duoi != null)
                {
                    cm_NguoiDung.hinhAnh = dto_NguoiDung.hinhDaiDien.ma.Value + dto_NguoiDung.hinhDaiDien.duoi;
                }
            }

            return cm_NguoiDung;
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


        public clientmodel_DangNhap themNguoiDung()
        {
            throw new NotImplementedException();
        }


        clientmodel_DangNhap Iwcf_NguoiDung.dangKy(string tenDN, string matKhau, string email, string hoTen, DateTime ngaySinh, int maHinh)
        {
            throw new NotImplementedException();
        }

        
    }
}
