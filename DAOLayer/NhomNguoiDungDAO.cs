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
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
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

        public static KetQua capNhatQuyenTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int? maNhomNguoiDung, int? maQuyen, bool co)
        {
            return khongTruyVan
                (
                    "capNhatNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen",
                    new object[] 
                    { 
                        phamVi,
                        maNhomNguoiDung,
                        maQuyen,
                        co
                    }
                );
        }
    }
}
