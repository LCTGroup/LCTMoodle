﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class CauHoi_DiemDAO : DAO<CauHoi_DiemDAO, CauHoi_DiemDTO>
    {
        public static CauHoi_DiemDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CauHoi_DiemDTO cauHoi_Diem = new CauHoi_DiemDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaCauHoi":
                        maTam = layInt(dong, i);
                        
                        if (maTam.HasValue)
                        {
                            cauHoi_Diem.cauHoi = LienKet.co(lienKet, "CauHoi") ?
                                layDTO<CauHoiDTO>(CauHoiDAO.layTheoMa(maTam, lienKet["CauHoi"])) :
                                new CauHoiDTO() { ma = maTam };
                        }
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cauHoi_Diem.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam, lienKet["NguoiTao"])) :
                                new NguoiDungDTO() { ma = maTam };
                        }
                        break;
                    case "Diem":
                        cauHoi_Diem.diem = layBool(dong, i); 
                        break;
                    default:
                        break;
                }                
            }            

            return cauHoi_Diem;
        }  
        
        public static KetQua them(int? maCauHoi, int? maNguoiTao, bool diem)
        {
            return khongTruyVan
            (
                "themCauHoi_Diem",
                new object[] 
                {
                    maCauHoi,
                    maNguoiTao,
                    diem
                }
            );
        }

        public static KetQua xoaTheoMaCauHoiVaMaNguoiTao(int? maCauHoi, int? maNguoiTao)
        {
            return khongTruyVan
            (
                "xoaCauHoi_DiemTheoMaCauHoiVaMaNguoiTao",
                new object[] 
                {
                    maCauHoi,
                    maNguoiTao
                }
            );
        }        

        public static KetQua layTheoMaCauHoiVaMaNguoiTao_Diem(int? maCauHoi, int? maNguoiTao)
        {
            return layGiaTri<bool>
                (
                    "layCauHoi_DiemTheomMaCauHoiVaMaNguoiTao_Diem",
                    new object[]
                    {
                        maCauHoi,
                        maNguoiTao
                    }
                );
        }

        public static KetQua layTheoMaCauHoi_Diem(int? maCauHoi)
        {
            return layGiaTri<int>
                (
                    "layCauHoi_DiemTheoMaCauHoi_Diem",
                    new object[]
                    {
                        maCauHoi
                    }
                );
        }
    }
}
