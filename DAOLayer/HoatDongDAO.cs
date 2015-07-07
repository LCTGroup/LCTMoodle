using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class HoatDongDAO : DAO<HoatDongDAO, HoatDongDTO>
    {
        public static HoatDongDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            HoatDongDTO hoatDong = new HoatDongDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        hoatDong.ma = layInt(dong, i); break;
                    case "MaNguoiTacDong":
                        hoatDong.maNguoiTacDong = layInt(dong, i); break;
                    case "LoaiDoiTuongTacDong":
                        hoatDong.loaiDoiTuongTacDong = layString(dong, i); break;
                    case "MaDoiTuongTacDong":
                        hoatDong.maDoiTuongTacDong = layInt(dong, i); break;
                    case "LoaiDoiTuongBiTacDong":
                        hoatDong.loaiDoiTuongBiTacDong = layString(dong, i); break;
                    case "MaDoiTuongBiTacDong":
                        hoatDong.maDoiTuongBiTacDong = layInt(dong, i); break;
                    case "MaHanhDong":
                        hoatDong.maHanhDong = layInt(dong, i); break;
                    case "ThoiDiem":
                        hoatDong.thoiDiem = layDateTime(dong, i); break;                        
                    default:
                        break;
                }                
            }

            if (LienKet.co(lienKet, "GiaTriHoatDong"))
            {
                var ketQua = GiaTriHanhDongDAO.layTheoMaHanhDong(hoatDong.maHanhDong);
                if (ketQua.trangThai != 0)
                {
                    hoatDong.giaTriHoatDong = null;
                }
                else
                {
                    List<GiaTriHanhDongDTO> giaTriHanhDong = ketQua.ketQua as List<GiaTriHanhDongDTO>;
                    hoatDong.giaTriHoatDong = giaTriHanhDong;
                }
            }
            return hoatDong;
        }

        public static KetQua themHoatDong(HoatDongDTO hoatDong)
        {
            return khongTruyVan
                (
                    "themHoatDong",
                    new object[]
                    {
                        hoatDong.maNguoiTacDong,
                        hoatDong.loaiDoiTuongTacDong,
                        hoatDong.maDoiTuongTacDong,
                        hoatDong.loaiDoiTuongBiTacDong,
                        hoatDong.maDoiTuongBiTacDong,
                        hoatDong.maHanhDong
                    }
                );
        }
    }
}
