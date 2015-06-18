﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class CauHoiDAO : DAO<CauHoiDAO, CauHoiDTO>
    {
        public static CauHoiDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CauHoiDTO cauHoi = new CauHoiDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        cauHoi.ma = layInt(dong, i); break;
                    case "TieuDe":
                        cauHoi.tieuDe = layString(dong, i); break;
                    case "NoiDung":
                        cauHoi.noiDung = layString(dong, i); break;
                    case "ThoiDiemTao":
                        cauHoi.thoiDiemTao = layDateTime(dong, i); break;
                    case "ThoiDiemCapNhat":
                        cauHoi.thoiDiemCapNhat = layDateTime(dong, i); break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cauHoi.nguoiTao = LienKet.co(lienKet, "MaNguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaChuDe":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cauHoi.chuDe = LienKet.co(lienKet, "MaChuDe") ?
                                layDTO<ChuDeDTO>(ChuDeDAO.layTheoMa(maTam.Value)) :
                                new ChuDeDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    default:
                        break;
                }                
            }

            if (LienKet.co(lienKet, "TraLoi"))
            {
                cauHoi.danhSachTraLoi = layDanhSachDTO<TraLoiDTO>(TraLoiDAO.layTheoMaCauHoi(cauHoi.ma, lienKet["TraLoi"]));
            }

            return cauHoi;
        }  
        
        public static KetQua them(CauHoiDTO cauHoi)
        {
            return layGiaTri<int>
            (
                "themCauHoi",
                new object[] 
                {
                    cauHoi.tieuDe,
                    cauHoi.noiDung,
                    layMa(cauHoi.nguoiTao),
                    layMa(cauHoi.chuDe)
                }
            );
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return khongTruyVan
            (
                "xoaCauHoiTheoMa",
                new object[] 
                {
                    ma
                }
            );
        }

        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong(
                "capNhatCauHoiTheoMa",
                new object[] 
                {
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }
   
        public static KetQua layTheoMa(int? maCauHoi, LienKet lienKet = null)
        {
            return layDong
            (
                "layCauHoiTheoMa",
                new object[]
                {
                    maCauHoi
                },
                lienKet
            );
        }
        
        public static KetQua layDanhSachCauHoi(LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layToanBoCauHoi",
                new object[] 
                {
                },
                lienKet
            );
        }

        public static KetQua layTheoChuDe(int? ma, LienKet lienKet = null)
        {
            return layDong(
                "layCauHoiTheoChuDe",
                new object[] 
                {
                    ma
                },
                lienKet
            );
        }
    }
}
