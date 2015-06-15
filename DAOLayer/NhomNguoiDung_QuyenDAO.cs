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
    public class NhomNguoiDung_QuyenDAO : DAO<NhomNguoiDung_QuyenDAO, NhomNguoiDung_QuyenDTO>
    {
        public static NhomNguoiDung_QuyenDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            NhomNguoiDung_QuyenDTO nhomNguoiDung_Quyen = new NhomNguoiDung_QuyenDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaQuyen":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            if (LienKet.co(lienKet, "Quyen"))
                            {
                                nhomNguoiDung_Quyen.quyen = layDTO<QuyenDTO>(QuyenDAO.layTheoMa(maTam));
                            }
                            else
                            {
                                nhomNguoiDung_Quyen.quyen = new QuyenDTO()
                                {
                                    ma = maTam
                                };
                            }
                        }
                        break;
                    case "MaNhomNguoiDung":
                        maTam = layInt(dong, i);
                        string phamViNhomNguoiDung = dong["PhamViNhomNguoiDung"] as string;

                        if (maTam.HasValue && phamViNhomNguoiDung != null)
                        {
                            if (LienKet.co(lienKet, "NhomNguoiDung"))
                            {
                                nhomNguoiDung_Quyen.nhomNguoiDung = layDTO<NhomNguoiDungDTO>(NhomNguoiDungDAO.layTheoMa(phamViNhomNguoiDung, maTam));
                            }
                            else
                            {
                                nhomNguoiDung_Quyen.nhomNguoiDung = new NhomNguoiDungDTO()
                                {
                                    ma = maTam,
                                    phamVi = phamViNhomNguoiDung
                                };
                            }
                        }
                        break;
                    case "MaDoiTuong":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue) {
                            nhomNguoiDung_Quyen.doiTuong = new DTO()
                            {
                                ma = maTam
                            };
                        }
                        break;
                    default:
                        break;
                }
            }

            return nhomNguoiDung_Quyen;
        }
        public static KetQua themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int? maNhomNguoiDung, int? maQuyen, int? maDoiTuong, bool them, bool la)
        {
            return khongTruyVan
                (
                    "themHoacXoaNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen",
                    new object[] 
                    { 
                        phamVi,
                        maNhomNguoiDung,
                        maQuyen,
                        maDoiTuong,
                        them,
                        la
                    }
                );
        }

        public static KetQua layTheoMaNhomNguoiDungVaMaDoiTuong(string phamVi, int? maNhomNguoiDung, int? maDoiTuong, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaDoiTuong",
                    new object[]
                    {
                        phamVi,
                        maNhomNguoiDung,
                        maDoiTuong
                    },
                    lienKet
                );
        }
    }
}