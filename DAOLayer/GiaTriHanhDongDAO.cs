using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class GiaTriHanhDongDAO : DAO<GiaTriHanhDongDAO, GiaTriHanhDongDTO>
    {
        public static GiaTriHanhDongDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            GiaTriHanhDongDTO giaTriHoatDong = new GiaTriHanhDongDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaHoatDong":
                        giaTriHoatDong.maHanhDong = layInt(dong, i); break;
                    case "GiaTriCu":
                        giaTriHoatDong.giaTriCu = layString(dong, i); break;
                    case "GiaTriMoi":
                        giaTriHoatDong.giaTriMoi = layString(dong, i); break;
                    case "GiaTri":
                        giaTriHoatDong.giaTriMoi = layString(dong, i); break;
                    default:
                        break;
                }
            }
            return giaTriHoatDong;
        }

        public static KetQua themGiaTriHanhDong(GiaTriHanhDongDTO giaTriHanhDong)
        {
            return khongTruyVan
                (
                    "ThemGiaTriHanhDong",
                    new object[] 
                    {
                        giaTriHanhDong.maHanhDong,
                        giaTriHanhDong.giaTriCu,
                        giaTriHanhDong.giaTriMoi,
                        giaTriHanhDong.giaTri
                    }
                );
        }

        public static KetQua layTheoMaHanhDong(int? maHanhDong)
        {
            return layDanhSachDong
                (
                    "layGiaTriHanhDongTheoMaHanhDong",
                    new object[]
                    {
                        maHanhDong
                    }
                );
        }
    }
}
