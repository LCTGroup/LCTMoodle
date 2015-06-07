using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class ChuDeDAO : DAO<ChuDeDAO, ChuDeDTO>
    {
        public static ChuDeDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            ChuDeDTO chuDe = new ChuDeDTO();

            int? maTam;
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
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            chuDe.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "PhamVi":
                        chuDe.phamVi = layString(dong, i); break;
                    case "MaChuDeCha":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            chuDe.chuDeCha = LienKet.co(lienKet, "ChuDeCha") ?
                                null :
                                new ChuDeDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaHinhDaiDien":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            chuDe.hinhDaiDien = LienKet.co(lienKet, "TapTin") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("ChuDe_HinhDaiDien", maTam.Value)) :
                                new TapTinDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    default:
                        break;
                }
            }

            return chuDe;
        }

        public static KetQua them(ChuDeDTO chuDe)
        {
            return layDong
            (
                "themChuDe",
                new object[] 
                {
                    chuDe.ten,
                    chuDe.moTa,
                    layMa(chuDe.nguoiTao),
                    chuDe.phamVi,
                    layMa(chuDe.chuDeCha),
                    layMa(chuDe.hinhDaiDien)
                }
            );
        }

        public static KetQua layTheoMaChuDeCha(string phamVi, int? maChuDeCha)
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
        
        public static KetQua layTheoMa(string phamVi, int? ma)
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

        public static KetQua xoaTheoMa(int? ma)
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
