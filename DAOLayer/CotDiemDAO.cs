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
    public class CotDiemDAO : DAO<CotDiemDAO, CotDiemDTO>
    {
        public static CotDiemDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CotDiemDTO cotDiem = new CotDiemDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        cotDiem.ma = layInt(dong, i);
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cotDiem.khoaHoc = LienKet.co(lienKet, "KhoaHoc") ?
                                layDTO<KhoaHocDTO>(KhoaHocDAO.layTheoMa(maTam.Value, lienKet["KhoaHoc"])) :
                                new KhoaHocDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "Ten":
                        cotDiem.ten = layString(dong, i);
                        break;
                    case "MoTa":
                        cotDiem.moTa = layString(dong, i);
                        break;
                    case "HeSo":
                        cotDiem.heSo = layInt(dong, i);
                        break;
                    case "Ngay":
                        cotDiem.ngay = layDateTime(dong, i);
                        break;
                    case "LaDiemCong":
                        cotDiem.laDiemCong = layBool(dong, i);
                        break;
                    case "LoaiDoiTuong":
                        cotDiem.loaiDoiTuong = layString(dong, i);
                        break;
                    case "MaDoiTuong":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cotDiem.doiTuong = new DTO() { ma = maTam };
                        }
                        break;
                    default:
                        break;
                }
            }

            return cotDiem;
        }

        public static KetQua them(CotDiemDTO cotDiem, LienKet lienKet = null)
        {
            return layDong
            (
                "themCotDiem",
                new object[] 
                {
                    cotDiem.ten,
                    cotDiem.moTa,
                    cotDiem.heSo,
                    cotDiem.ngay,
                    layMa(cotDiem.khoaHoc),
                    cotDiem.laDiemCong,
                    cotDiem.loaiDoiTuong,
                    layMa(cotDiem.doiTuong)
                }
            );
        }

        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
            (
                "capNhatCotDiemTheoMa",
                new object[]
                {
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }

        public static KetQua layTheoMaKhoaHoc(int? maKhoaHoc, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layCotDiemTheoMaKhoaHoc",
                new object[] 
                { 
                    maKhoaHoc
                },
                lienKet
            );
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return khongTruyVan
            (
                "xoaCotDiemTheoMa",
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
                "capNhatCotDiem_ThuTu",
                new object[]
                {
                    thuTuCu,
                    thuTuMoi,
                    maKhoaHoc
                }
            );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layCotDiemTheoMa",
                    new object[]
                    {
                        ma
                    },
                    lienKet
                );
        }

        public static KetQua layTheoLoaiDoiTuongVaMaDoiTuong(string loaiDoiTuong, int? maDoiTuong, LienKet lienKet = null)
        {
            return layDong
                (
                    "layCotDiemTheoLoaiDoiTuongVaMaDoiTuong",
                    new object[] 
                    { 
                        loaiDoiTuong,
                        maDoiTuong
                    },
                    lienKet
                );
        }
    }
}
