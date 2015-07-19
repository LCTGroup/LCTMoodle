using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietTaiLieuDAO : DAO<BaiVietTaiLieuDAO, BaiVietTaiLieuDTO>
    {
        public static BaiVietTaiLieuDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BaiVietTaiLieuDTO baiViet = new BaiVietTaiLieuDTO();

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
                    case "MaTapTin":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiViet.tapTin = LienKet.co(lienKet, "TapTin") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("BaiVietTaiLieu_TapTin", maTam.Value, lienKet["TapTin"])) :
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

        public static KetQua them(BaiVietTaiLieuDTO baiViet, LienKet lienKet = null)
        {
            return layDong
                (
                    "themBaiVietTaiLieu",
                    new object[] 
                    {
                        baiViet.tieuDe,
                        baiViet.noiDung,
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
                    "layBaiVietTaiLieuTheoMaKhoaHoc",
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
                "xoaBaiVietTaiLieuTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong(
                "layBaiVietTaiLieuTheoMa",
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
                "capNhatBaiVietTaiLieuTheoMa",
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
