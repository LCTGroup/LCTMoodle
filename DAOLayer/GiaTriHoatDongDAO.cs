using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class GiaTriHoatDongDAO : DAO<GiaTriHoatDongDAO, GiaTriHoatDongDTO>
    {
        public static GiaTriHoatDongDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            GiaTriHoatDongDTO giaTriHoatDong = new GiaTriHoatDongDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaHoatDong":
                        giaTriHoatDong.maHoatDong = layInt(dong, i); break;
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

        public static KetQua them(GiaTriHoatDongDTO giaTriHoatDong)
        {
            return khongTruyVan
                (
                    "ThemGiaTriHoatDong",
                    new object[] 
                    {
                        giaTriHoatDong.maHoatDong,
                        giaTriHoatDong.giaTriCu,
                        giaTriHoatDong.giaTriMoi,
                        giaTriHoatDong.giaTri
                    }
                );
        }

        public static KetQua layTheoMaHoatDong(int? maHoatDong)
        {
            return layDanhSachDong
                (
                    "layGiaTriHoatDongTheoMaHoatDong",
                    new object[]
                    {
                        maHoatDong
                    }
                );
        }
        
    }
}
