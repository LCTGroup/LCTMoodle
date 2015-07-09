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

        public clientmodel_NguoiDung themNguoiDung(Dictionary<string,string> nguoiDung)
        {
            Form form = new Form()
            {
                {"TenTaiKhoan",nguoiDung["TenTaiKhoan"]},
                {"MatKhau",nguoiDung["MatKhau"]},
                {"Email",nguoiDung["Email"]},
                {"Ho",nguoiDung["Ho"]},
                {"TenLot",nguoiDung["TenLot"]},
                {"Ten",nguoiDung["Ten"]},
                {"NgaySinh",nguoiDung["NgaySinh"]},
            };

            KetQua ketQua = NguoiDungBUS.them(form);
            clientmodel_NguoiDung cm_NguoiDung = new clientmodel_NguoiDung();

            if(ketQua.trangThai == 0)
            {
                cm_NguoiDung.TrangThai = 0;
                cm_NguoiDung.ThongBao = "Đăng ký thành công ! check mail để kích hoạt";
            }
            else
            {
                cm_NguoiDung.TrangThai = ketQua.trangThai;
                cm_NguoiDung.ThongBao = ketQua.ketQua as string;
            }

            return cm_NguoiDung;
        }
    }
}
