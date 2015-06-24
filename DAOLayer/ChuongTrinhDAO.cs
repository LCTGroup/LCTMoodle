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
    public class ChuongTrinhDAO : DAO<ChuongTrinhDAO, ChuongTrinhDTO>
    {
        public static ChuongTrinhDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            ChuongTrinhDTO giaoTrinh = new ChuongTrinhDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        giaoTrinh.ma = layInt(dong, i);
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            giaoTrinh.khoaHoc = LienKet.co(lienKet, "KhoaHoc") ?
                                layDTO<KhoaHocDTO>(KhoaHocDAO.layTheoMa(maTam.Value)) :
                                new KhoaHocDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "CongViec":
                        giaoTrinh.congViec = layString(dong, i);
                        break;
                    case "MoTa":
                        giaoTrinh.moTa = layString(dong, i);
                        break;
                    case "ThoiGian":
                        giaoTrinh.thoiGian = layString(dong, i);
                        break;
                    case "ThuTu":
                        giaoTrinh.thuTu = layInt(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return giaoTrinh;
        }

        public static KetQua layTheoMaKhoaHoc(int? maKhoaHoc, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layChuongTrinhTheoMaKhoaHoc",
                new object[]
                {
                    maKhoaHoc
                },
                lienKet
            );
        }

        public static KetQua them(ChuongTrinhDTO giaoTrinh)
        {
            return layDong
            (
                "themChuongTrinh",
                new object[]
                {
                    layMa(giaoTrinh.khoaHoc),
                    giaoTrinh.congViec,
                    giaoTrinh.moTa,
                    giaoTrinh.thoiGian
                }
            );
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return khongTruyVan
            (
                "xoaChuongTrinhTheoMa",
                new object[]
                {
                    ma
                }
            );
        }

        public static KetQua capNhatThuTu(int? thuTuCu, int? thuTuMoi, int? maKhoaHoc)
        {
            return khongTruyVan
            (
                "capNhatChuongTrinh_ThuTu",
                new object[]
                {
                    thuTuCu,
                    thuTuMoi,
                    maKhoaHoc
                }
            );
        }
    }
}
