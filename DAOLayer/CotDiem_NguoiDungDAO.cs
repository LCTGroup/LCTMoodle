using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;
using System.Reflection;

namespace DAOLayer
{
    public class CotDiem_NguoiDungDAO : DAO<CotDiem_NguoiDungDAO, CotDiem_NguoiDungDTO>
    {
        public static CotDiem_NguoiDungDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CotDiem_NguoiDungDTO diem = new CotDiem_NguoiDungDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaCotDiem":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            diem.cotDiem = LienKet.co(lienKet, "CotDiem") ?
                                layDTO<CotDiemDTO>(CotDiemDAO.layTheoMa(maTam, lienKet["CotDiem"])) :
                                new CotDiemDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaNguoiDung":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            diem.nguoiDung = LienKet.co(lienKet, "NguoiDung") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam, lienKet["NguoiDung"])) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "Diem":
                        diem.diem = layDouble(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            diem.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam, lienKet["NguoiTao"])) :
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

            return diem;
        }

        public static KetQua layTheoMaKhoaHoc_ChuoiNguoiDung_Diem(int? maKhoaHoc)
        {
            return layGiaTri<string>
                (
                    "layCotDiem_NguoiDungTheoMaKhoaHoc_ChuoiNguoiDung_Diem",
                    new object[]
                    {
                        maKhoaHoc
                    }
                );
        }

        public static KetQua capNhat_Nhieu(System.Data.DataTable bang)
        {
            return khongTruyVan
                (
                    "capNhatCotDiem_NguoiDung_Nhieu",
                    new object[]
                    {
                        bang
                    }
                );
        }

        public static KetQua capNhat_Mot(CotDiem_NguoiDungDTO diem)
        {
            return khongTruyVan
                (
                    "capNhatCotDiem_NguoiDung_Mot",
                    new object[]
                    {
                        layMa(diem.cotDiem),
                        layMa(diem.nguoiDung),
                        diem.diem,
                        layMa(diem.nguoiTao)
                    }
                );
        }
    }
}
