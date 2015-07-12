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
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value, lienKet["NguoiTao"])) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaCha":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            chuDe.cha = LienKet.co(lienKet, "Cha") ?
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
                            chuDe.hinhDaiDien = LienKet.co(lienKet, "HinhDaiDien") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("ChuDe_HinhDaiDien", maTam.Value, lienKet["HinhDaiDien"])) :
                                new TapTinDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "Cay":
                        chuDe.cay = layString(dong, i);
                        break;
                    default:
                        if (chuDe.duLieuThem == null)
                        {
                            chuDe.duLieuThem = new Dictionary<string, object>();
                        }
                        chuDe.duLieuThem.Add(dong.GetName(i), dong[i]);
                        break;
                }
            }

            if (LienKet.co(lienKet, "Con"))
            {
                chuDe.con = layDanhSachDTO<ChuDeDTO>(ChuDeDAO.layTheoMaCha(chuDe.ma));
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
                    layMa(chuDe.cha),
                    layMa(chuDe.hinhDaiDien)
                }
            );
        }

        public static KetQua layTheoMaCha(int? maCha, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layChuDeTheoMaCha",
                new object[] 
                {
                    maCha
                },
                lienKet
            );
        }
        
        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
            (
                "layChuDeTheoMa",
                new object[] 
                { 
                    ma
                },
                lienKet
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

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet)
        {
            return layDanhSachDong
                (
                    "layChuDe_TimKiem",
                    new object[]
                    {
                        tuKhoa
                    },
                    lienKet
                );
        }

        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
            (
                "capNhatChuDeTheoMa",
                new object[]
                {
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }

        public static KetQua capNhatTheoMa_MaCha(int? ma, int? maCha)
        {
            return khongTruyVan
                (
                    "capNhatChuDeTheoMa_MaCha",
                    new object[]
                    {
                        ma, 
                        maCha
                    }
                );
        }

        public static KetQua lay_TimKiemPhanTrang(string where = null, string orderBy = null, int? trang = null, int? soDongMoiTrang = null, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layChuDe_TimKiemPhanTrang",
                    new object[]
                    {
                        where,
                        orderBy,
                        trang,
                        soDongMoiTrang
                    },
                    lienKet
                );
        }
    }
}
