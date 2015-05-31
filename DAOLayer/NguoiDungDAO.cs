using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class NguoiDungDAO : DAO<NguoiDungDAO, NguoiDungViewDTO>
    {
        public static NguoiDungViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            NguoiDungViewDTO nguoiDung = new NguoiDungViewDTO();

            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        nguoiDung.ma = layInt(dong, i); break;
                    case "TenTaiKhoan":
                        nguoiDung.tenTaiKhoan = layString(dong, i); break;
                    case "MatKhau":
                        nguoiDung.matKhau = layString(dong, i); break;
                    case "Email":
                        nguoiDung.email = layString(dong, i); break;
                    case "HoTen":
                        nguoiDung.hoTen = layString(dong, i); break;
                    case "NgaySinh":
                        nguoiDung.ngaySinh = layDateTime(dong, i); break;
                    case "DiaChi":
                        nguoiDung.diaChi = layString(dong, i); break;
                    case "SoDienThoai":
                        nguoiDung.soDienThoai = layString(dong, i); break;
                    case "MaHinhDaiDien":
                        nguoiDung.hinhDaiDien = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }

            return nguoiDung;
        }  

        public static KetQua them(NguoiDungDataDTO nguoiDung)
        {
            return layGiaTri
            (
                "themNguoiDung",
                new object[] 
                {
                    nguoiDung.tenTaiKhoan,
                    nguoiDung.matKhau,
                    nguoiDung.email,
                    nguoiDung.hoTen,
                    nguoiDung.ngaySinh,
                    nguoiDung.diaChi,
                    nguoiDung.soDienThoai,
                    nguoiDung.maHinhDaiDien
                }
            );
        }
        public static KetQua layTheoTenTaiKhoan(string tenTaiKhoan)
        {
            return layDong
            (
                "layNguoiDungTheoTenTaiKhoan",
                new object[]
                {
                    tenTaiKhoan
                }
            );
        }
        public static KetQua layTheoMa(int ma)
        {
            return layDong
            (
                "layNguoiDungTheoMa",
                new object[]
                {
                    ma
                }
            );
        }        
    }
}
