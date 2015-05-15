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
        //public static KetQua kiemTraDangNhap(Dictionary<string, string> form)
        //{
        //    NguoiDungViewDTO nguoiDungDangNhap = new NguoiDungViewDTO()
        //    {
        //        tenTaiKhoan = layString(form, "TenTaiKhoan"),
        //        matKhau = layString(form, "MatKhau")
        //    };

        //    KetQua ketQua = NguoiDungDAO.lay(nguoiDungDangNhap);
        //}

    }
}