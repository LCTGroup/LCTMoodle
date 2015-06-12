﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class NhomNguoiDung_NguoiDungDAO : DAO<NhomNguoiDung_NguoiDungDAO, NhomNguoiDung_NguoiDungDTO>
    {
        public static NhomNguoiDung_NguoiDungDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            NhomNguoiDung_NguoiDungDTO nhomNguoiDung_NguoiDung = new NhomNguoiDung_NguoiDungDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaNhomNguoiDung":
                        maTam = layInt(dong, i);
                        string phamViNhomNguoiDung = dong["PhamViNhomNguoiDung"] as string;

                        if (maTam.HasValue && phamViNhomNguoiDung != null)
                        {
                            if (LienKet.co(lienKet, "NhomNguoiDung"))
                            {
                                nhomNguoiDung_NguoiDung.nhomNguoiDung = layDTO<NhomNguoiDungDTO>(NhomNguoiDungDAO.layTheoMa(phamViNhomNguoiDung, maTam));
                            }
                            else
                            {
                                nhomNguoiDung_NguoiDung.nhomNguoiDung = new NhomNguoiDungDTO()
                                {
                                    ma = maTam,
                                    phamVi = phamViNhomNguoiDung
                                };
                            }
                        }
                        break;
                    case "MaNguoiDung":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            if (LienKet.co(lienKet, "NguoiDung"))
                            {
                                nhomNguoiDung_NguoiDung.nguoiDung = layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam));
                            }
                            else
                            {
                                nhomNguoiDung_NguoiDung.nguoiDung = new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return nhomNguoiDung_NguoiDung;
        }

        public static KetQua layTheoMaNhomNguoiDungVaMaNguoiDung(string phamVi, int? maNhomNguoiDung, int? maNguoiDung, LienKet lienKet = null)
        {
            return layDong
                (
                    "layNhomNguoiDung_NguoiDungTheoMaNhomNguoiDungVaMaNguoiDung",
                    new object[] 
                    { 
                        phamVi, 
                        maNhomNguoiDung,
                        maNguoiDung
                    },
                    lienKet
                );            
        }

        public static KetQua them(string phamVi, int? maNhomNguoiDung, int? maNguoiDung)
        {
            return khongTruyVan
                (
                    "themNhomNguoiDung_NguoiDung",
                    new object[]
                    {
                        phamVi,
                        maNhomNguoiDung,
                        maNguoiDung
                    }
                );
        }

        public static KetQua xoaTheoMaNhomNguoiDungVaMaNguoiDung(string phamVi, int? maNhomNguoiDung, int? maNguoiDung)
        {
            return khongTruyVan
                (
                    "xoaNhomNguoiDung_NguoiDungTheoMaNhomNguoiDungVaMaNguoiDung",
                    new object[]
                    {
                        phamVi,
                        maNhomNguoiDung,
                        maNguoiDung
                    }
                );
        }
    }
}
