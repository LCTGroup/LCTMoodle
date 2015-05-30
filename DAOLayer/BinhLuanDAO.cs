using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BinhLuanDAO : DAO<BinhLuanDAO, BinhLuanViewDTO>
    {
        public static BinhLuanViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            BinhLuanViewDTO binhLuan = new BinhLuanViewDTO();

            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        binhLuan.ma = layInt(dong, i);
                        break;
                    case "NoiDung":
                        binhLuan.noiDung = layString(dong, i);
                        break;
                    case "MaNguoiTao":
                        binhLuan.nguoiTao = new NguoiDungViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaDoiTuong":
                        binhLuan.doiTuong = new DTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaTapTin":
                        binhLuan.tapTin = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "ThoiDiemTao":
                        binhLuan.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "LoaiDoiTuong":
                        binhLuan.loaiDoiTuong = layString(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return binhLuan;
        }

        public static KetQua them(BinhLuanDataDTO binhLuan)
        {
            return layDong
            (
                "themBinhLuan",
                new object[] { 
                    binhLuan.noiDung,
                    binhLuan.maTapTin,
                    binhLuan.maDoiTuong,
                    binhLuan.maNguoiTao,
                    binhLuan.loaiDoiTuong
                }
            );
        }

        public static KetQua layTheoDoiTuong(string loaiDoiTuong, int maDoiTuong)
        {
            return layDanhSachDong
            (
                "layBinhLuanTheoDoiTuong",
                new object[] {
                    loaiDoiTuong,
                    maDoiTuong
                }
            );
        }

        public static KetQua xoaTheoMa(string loaiDoiTuong, int ma)
        {
            return khongTruyVan(
                "xoaBinhLuanTheoMa",
                new object[] 
                { 
                    loaiDoiTuong,
                    ma
                }
            );
        }
    }
}
