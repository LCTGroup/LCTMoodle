using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class ChuDeDAO : DAO<ChuDeDAO, ChuDeViewDTO>
    {
        public static ChuDeViewDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            ChuDeViewDTO chuDe = new ChuDeViewDTO();

            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        chuDe.ma = layInt(dong, i); break;
                    case "Ten":
                        chuDe.ten = layString(dong, i); break;
                    case "MoTa":
                        chuDe.moTa = layString(dong, i); break;
                    case "ThoiDiemTao":
                        chuDe.thoiDiemTao = layDateTime(dong, i); break;
                    case "MaNguoiTao":
                        chuDe.maNguoiTao = layInt(dong, i); break;
                    case "PhamVi":
                        chuDe.phamVi = layString(dong, i); break;
                    case "MaChuDeCha":
                        int maChuDeCha = layInt(dong, i);
                        if (maChuDeCha != 0)
                        {
                            chuDe.chuDeCha = new ChuDeViewDTO()
                            {
                                ma = maChuDeCha
                            };
                        }
                        break;
                    case "MaHinhDaiDien":
                        chuDe.hinhDaiDien = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }

            return chuDe;
        }

        public static KetQua them(ChuDeDataDTO chuDe)
        {
            return layDong
            (
                "themChuDe",
                new object[] 
                {
                    chuDe.ten,
                    chuDe.moTa,
                    chuDe.maNguoiTao,
                    chuDe.phamVi,
                    chuDe.maChuDeCha,
                    chuDe.maHinhDaiDien
                }
            );
        }

        public static KetQua layTheoMaChuDeCha(string phamVi, int maChuDeCha)
        {
            return layDanhSachDong
            (
                "layChuDeTheoMaChuDeCha",
                new object[] 
                { 
                    phamVi,
                    maChuDeCha
                }
            );
        }
        
        public static KetQua layTheoMa(string phamVi, int ma)
        {
            return layDong
            (
                "layChuDeTheoMa",
                new object[] 
                { 
                    phamVi,
                    ma
                }
            );
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return khongTruyVan
            (
                "xoaChuDeTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }
    }
}
