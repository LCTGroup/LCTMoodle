using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietBaiGiangDAO : DAO<BaiVietBaiGiangDAO, BaiVietBaiGiangDTO>
    {
        public static BaiVietBaiGiangDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BaiVietBaiGiangDTO baiViet = new BaiVietBaiGiangDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        baiViet.ma = layInt(dong, i);
                        break;
                    case "TieuDe":
                        baiViet.tieuDe = layString(dong, i);
                        break;
                    case "NoiDung":
                        baiViet.noiDung = layString(dong, i);
                        break;
                    case "TomTat":
                        baiViet.tomTat = layString(dong, i);
                        break;
                    case "MaTapTin":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiViet.tapTin = LienKet.co(lienKet, "TapTin") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("BaiVietBaiGiang_TapTin", maTam.Value, lienKet["TapTin"])) :
                                new TapTinDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "ThoiDiemTao":
                        baiViet.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiViet.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value, lienKet["NguoiTao"])) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiViet.khoaHoc = LienKet.co(lienKet, "KhoaHoc") ?
                                layDTO<KhoaHocDTO>(KhoaHocDAO.layTheoMa(maTam.Value, lienKet["KhoaHoc"])) :
                                new KhoaHocDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "DanhSachMaThanhVienDaXem":
                        baiViet.danhSachMaThanhVienDaXem = layString(dong, i, "|");
                        break;
                    default:
                        break;
                }
            }

            return baiViet;
        }

        public static KetQua them(BaiVietBaiGiangDTO baiViet, LienKet lienKet = null)
        {
            return layDong
                (
                    "themBaiVietBaiGiang",
                    new object[] 
                    {
                        baiViet.tieuDe,
                        baiViet.noiDung,
                        baiViet.tomTat,
                        layMa(baiViet.tapTin),
                        layMa(baiViet.nguoiTao),
                        layMa(baiViet.khoaHoc)
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaKhoaHoc(int? maKhoaHoc, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layBaiVietBaiGiangTheoMaKhoaHoc",
                    new object[] 
                    { 
                        maKhoaHoc
                    },
                    lienKet
                );
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return khongTruyVan(
                "xoaBaiVietBaiGiangTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong(
                "layBaiVietBaiGiangTheoMa",
                new object[] 
                { 
                    ma
                },
                lienKet
            );
        }

        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
            (
                "capNhatBaiVietBaiGiangTheoMa",
                new object[]
                {
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }
    }
}
