using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class NguoiDungViewDTO : DTO
    {
        public string tenTaiKhoan;
        public string matKhau;
        public string email;
        public string hoTen;
        public DateTime? ngaySinh;
        public string diaChi;
        public string soDienThoai;
        public TapTinViewDTO hinhDaiDien;
 
        public override void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        ma = layInt(dong, i); break;
                    case "TenTaiKhoan":
                        tenTaiKhoan = layString(dong, i); break;
                    case "MatKhau":
                        matKhau = layString(dong, i); break;
                    case "Email":
                        email = layString(dong, i); break;
                    case "HoTen":
                        hoTen = layString(dong, i); break;
                    case "NgaySinh":
                        ngaySinh = layDateTime(dong, i); break;
                    case "DiaChi":
                        diaChi = layString(dong, i); break;
                    case "SoDienThoai":
                        soDienThoai = layString(dong, i); break;
                    case "MaHinhDaiDien":
                        hinhDaiDien = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }
        }        
    }
}
