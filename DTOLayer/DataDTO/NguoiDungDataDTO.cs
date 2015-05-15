using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace DTOLayer
{
    public class NguoiDungDataDTO : DTO
    {
        public int ma;
        public string tenTaiKhoan;
        public string matKhau;
        public string email;
        public string hoTen;
        public DateTime? ngaySinh;
        public string diaChi;
        public string soDienThoai;
        public int maHinhDaiDien;

        /// <summary>
        /// Kiểm tra
        /// </summary>
        /// <returns>ketQua.TrangThai != 3 : dữ liệu hợp lệ</returns>
        public override KetQua kiemTra()
        {            
            List<string> thongBao = new List<string>();

            #region Kiểm tra Valid

            if (string.IsNullOrEmpty(tenTaiKhoan))
            {
                thongBao.Add("Tên tài khoản không được bỏ trống");
            }

            if (string.IsNullOrEmpty(matKhau))
            {
                thongBao.Add("Mật khẩu không được bỏ trống");
            }

            if (string.IsNullOrEmpty(email))
            {
                thongBao.Add("Email không được bỏ trống");
            }
            else if (!LCTHelper.laEmail(email))
            {
                thongBao.Add("Email không hợp lệ");
            }

            if (string.IsNullOrEmpty(hoTen))
            {
                thongBao.Add("Họ tên không được bỏ trống");
            }

            #endregion
           
            KetQua ketQua = new KetQua();
            ketQua.trangThai = (thongBao.Count > 0) ? 3 : 0;
            ketQua.ketQua = thongBao;
            return ketQua;
        }
    }
}
