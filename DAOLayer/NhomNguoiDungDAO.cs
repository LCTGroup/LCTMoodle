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
    public class NhomNguoiDungDAO : DAO<NhomNguoiDungDAO, NhomNguoiDungDTO>
    {
        public static NhomNguoiDungDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            NhomNguoiDungDTO nhomNguoiDung = new NhomNguoiDungDTO();
            
            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        nhomNguoiDung.ma = layInt(dong, i);
                        break;
                    case "Ten":
                        nhomNguoiDung.ten = layString(dong, i);
                        break;
                    case "MoTa":
                        nhomNguoiDung.moTa = layString(dong, i);
                        break;
                    case "PhamVi":
                        nhomNguoiDung.phamVi = layString(dong, i);
                        break;
                    case "MaDoiTuong":
                        maTam = layInt(dong, i);
                        
                        if (maTam.HasValue)
                        {
                            nhomNguoiDung.doiTuong = new DTO()
                            {
                                ma = maTam
                            };
                        }
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            nhomNguoiDung.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value, lienKet["NguoiTao"])) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "GiaTri":
                        nhomNguoiDung.giaTri = layString(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return nhomNguoiDung;
        }

        public static KetQua them(NhomNguoiDungDTO nhomNguoiDung)
        {
            return layDong
                (
                    "themNhomNguoiDung",
                    new object[]
                    {
                        nhomNguoiDung.ten,
                        nhomNguoiDung.moTa,
                        nhomNguoiDung.phamVi,
                        layMa(nhomNguoiDung.doiTuong),
                        layMa(nhomNguoiDung.nguoiTao)
                    }
                );
        }

        public static KetQua capNhatTheoMa(string phamVi, int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
                (
                    "capNhatNhomNguoiDungTheoMa",
                    new object[]
                    {
                        phamVi,
                        ma,
                        bangCapNhat.bang
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaDoiTuong(string phamVi, int? maDoiTuong)
        {
            return layDanhSachDong
                (
                    "layNhomNguoiDungTheoMaDoiTuong",
                    new object[]
                    {
                        phamVi,
                        maDoiTuong
                    }
                );
        }

        public static KetQua xoaTheoMa(string phamVi, int? ma)
        {
            return khongTruyVan
                (
                    "xoaNhomNguoiDungTheoMa",
                    new object[]
                    {
                        phamVi,
                        ma
                    }
                );
        }

        public static KetQua layTheoMa(string phamVi, int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layNhomNguoiDungTheoMa",
                    new object[]
                    {
                        phamVi,
                        ma
                    },
                    lienKet
                );
        }

        public static KetQua them_MacDinh(string phamVi, int? maDoiTuong)
        {
            return khongTruyVan
                (
                    "themNhomNguoiDung_MacDinh",
                    new object[]
                    {
                        phamVi,
                        maDoiTuong
                    }
                );
        }
    }
}
