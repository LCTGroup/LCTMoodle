using System;
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
                            nhomNguoiDung_Quyen.quyen = LienKet.co(lienKet, "Quyen") ?
                                layDTO<QuyenDTO>(QuyenDAO.layTheoMa(maTam, lienKet["Quyen"])) : 
                                new QuyenDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaNhomNguoiDung":
                        maTam = layInt(dong, i);
                        string phamViNhomNguoiDung = dong["PhamViNhomNguoiDung"] as string;

                        if (maTam.HasValue && phamViNhomNguoiDung != null)
                        {
                            nhomNguoiDung_Quyen.nhomNguoiDung = LienKet.co(lienKet, "NhomNguoiDung") ?
                                layDTO<NhomNguoiDungDTO>(NhomNguoiDungDAO.layTheoMa(phamViNhomNguoiDung, maTam, lienKet["NhomNguoiDung"])) : 
                                new NhomNguoiDungDTO()
                                {
                                    ma = maTam,
                                    phamVi = phamViNhomNguoiDung
                                };
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

        public static KetQua themTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int? maNhomNguoiDung, int? maQuyen, int? maDoiTuong, bool la)
        {
            return khongTruyVan
                (
                    "themNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen",
                    new object[] 
                    { 
                        phamVi,
                        maNhomNguoiDung,
                        maQuyen,
                        maDoiTuong,
                        la
                    }
                );
        }

        public static KetQua xoaTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int? maNhomNguoiDung, int? maQuyen, int? maDoiTuong, bool la)
        {
            return khongTruyVan
                (
                    "xoaNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen",
                    new object[] 
                    { 
                        phamVi,
                        maNhomNguoiDung,
                        maQuyen,
                        maDoiTuong,
                        la
                    }
                );
        }

        public static KetQua layTheoMaNhomNguoiDung(string phamVi, int? maNhomNguoiDung, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layNhomNguoiDung_QuyenTheoMaNhomNguoiDung",
                    new object[]
                    {
                        phamVi,
                        maNhomNguoiDung
                    },
                    lienKet
                );
        }
    }
}
