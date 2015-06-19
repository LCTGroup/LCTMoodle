using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BUSLayer;
using DTOLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    public class wcf_NguoiDung : Iwcf_NguoiDung
    {
        /// <summary>
        /// Webservice kiểm tra đăng nhập đăng nhậ
        ///  - 1: tên đăng nhập không tồn tại
        ///  - 2: mật khẩu không đúng
        ///  - 3: thành công
        /// </summary>
        /// <param name="_TenDN"></param>
        /// <param name="_MatKhau"></param>
        /// <returns>int</returns>
        public int kiemTraDangNhap(string _TenDN, string _MatKhau)
        {
            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(_TenDN);
            NguoiDungDTO dto_NguoiDung = ketQua.ketQua as NguoiDungDTO;

            if(ketQua.trangThai != 0)
            {
                return 1; //Tên đăng nhập không tồn tại
            }
            else
            {
                if(_MatKhau == dto_NguoiDung.matKhau || NguoiDungHelper.layMaMD5(_MatKhau) == dto_NguoiDung.matKhau)
                {
                    return 3; //Đúng
                }
                else
                {
                    return 2; //Sai Mật Khẩu
                }
            }
        }
    }
}
