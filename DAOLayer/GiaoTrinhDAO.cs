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
    public class GiaoTrinhDAO : DAO<GiaoTrinhDAO, GiaoTrinhViewDTO>
    {
        public static GiaoTrinhViewDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            GiaoTrinhViewDTO giaoTrinh = new GiaoTrinhViewDTO();

            int maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        giaoTrinh.ma = layInt(dong, i);
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);

                        if (maTam != 0)
                        {
                            if (LienKet.co(lienKet, "KhoaHoc"))
                            {
                                KetQua ketQua = KhoaHocDAO.layTheoMa(maTam);

                                if (ketQua.trangThai == 0)
                                {
                                    giaoTrinh.khoaHoc = ketQua.ketQua as KhoaHocViewDTO;
                                }
                            }
                            else
                            {
                                giaoTrinh.khoaHoc = new KhoaHocViewDTO()
                                {
                                    ma = maTam
                                };
                            }
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

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layGiaoTrinhTheoMaKhoaHoc",
                new object[]
                {
                    maKhoaHoc
                },
                lienKet
            );
        }

        public static KetQua them(GiaoTrinhDataDTO giaoTrinh)
        {
            return layDong
            (
                "themGiaoTrinh",
                new object[]
                {
                    giaoTrinh.maKhoaHoc,
                    giaoTrinh.congViec,
                    giaoTrinh.moTa,
                    giaoTrinh.thoiGian
                }
            );
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return khongTruyVan
            (
                "xoaGiaoTrinhTheoMa",
                new object[]
                {
                    ma
                }
            );
        }

        public static KetQua capNhatThuTu(int ma, int thuTu, int maKhoaHoc)
        {
            return khongTruyVan
            (
                "capNhatGiaoTrinh_ThuTu",
                new object[]
                {
                    ma,
                    thuTu,
                    maKhoaHoc
                }
            );
        }
    }
}
