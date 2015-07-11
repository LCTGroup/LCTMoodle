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
        /// <summary>
        /// Webservice kiểm tra đăng nhập đăng nhập
        ///  - 1: tên đăng nhập không tồn tại
        ///  - 2: mật khẩu không đúng
        ///  - 3: thành công
        /// </summary>
        /// <param name="_TenDN"></param>
        /// <param name="_MatKhau"></param>
        /// <returns>clientmodel_NguoiDung</returns>
        public int kiemTraDangNhap(string _TenDN, string _MatKhau)
        {
            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(_TenDN);
            NguoiDungDTO dto_NguoiDung = ketQua.ketQua as NguoiDungDTO;

            if (ketQua.trangThai != 0)
            {
                return 1; //Tên đăng nhập không tồn tại
            }
            else
            {
                if (_MatKhau == dto_NguoiDung.matKhau || NguoiDungHelper.layMaMD5(_MatKhau) == dto_NguoiDung.matKhau)
                {
                    return 3; //Đúng
                }
                else
                {
                    return 2; //Sai Mật Khẩu
                }
            }
        }


        clientmodel_NguoiDung dangKy(string tenDN, string matKhau, string email, string hoTen, DateTime ngaySinh, int maHinh)
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
            clientmodel_NguoiDung cm_NguoiDung = new clientmodel_NguoiDung();

            if(ketQua.trangThai == 0)
            {

            }
            else
            {
                
            }

            return cm_NguoiDung;
        }

        /// <summary>
        /// Webservice kiểm tra đăng nhập va trả vê thông báo
        /// </summary>
        /// <param name="tenDN"></param>
        /// <param name="matKhau"></param>
        /// <returns>clientmodel_NguoiDung</returns>
        public clientmodel_NguoiDung dangNhap(string tenDN, string matKhau)
        {
            KetQua ketQua = NguoiDungBUS.xuLyDangNhap(tenDN, matKhau);
            clientmodel_NguoiDung cm_NguoiDung = new clientmodel_NguoiDung();

            if(ketQua.trangThai == 0)
            {
                NguoiDungDTO dto_NguoiDung = ketQua.ketQua as NguoiDungDTO;
                cm_NguoiDung.ma = dto_NguoiDung.ma.Value;
                cm_NguoiDung.thongBao = "Đăng nhập thành công";
            }
            else
            {
                cm_NguoiDung.thongBao = ketQua.ketQua as string;
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


        public clientmodel_NguoiDung themNguoiDung()
        {
            throw new NotImplementedException();
        }


        clientmodel_NguoiDung Iwcf_NguoiDung.dangKy(string tenDN, string matKhau, string email, string hoTen, DateTime ngaySinh, int maHinh)
        {
            throw new NotImplementedException();
        }
    }
}
