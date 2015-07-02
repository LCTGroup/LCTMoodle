using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietBaiTapDAO : DAO<BaiVietBaiTapDAO, BaiVietBaiTapDTO>
    {
        public static BaiVietBaiTapDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BaiVietBaiTapDTO baiViet = new BaiVietBaiTapDTO();

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
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("BaiVietDienDan_TapTin", maTam.Value)) :
                                new TapTinDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "Loai":
                        baiViet.loai = layInt(dong, i);
                        break;
                    case "ThoiDiemHetHan":
                        baiViet.thoiDiemHetHan = layDateTime(dong, i);
                        break;
                    case "ThoiDiemTao":
                        baiViet.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiViet.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);

                        baiViet.khoaHoc = LienKet.co(lienKet, "KhoaHoc") ?
                            layDTO<KhoaHocDTO>(KhoaHocDAO.layTheoMa(maTam.Value)) :
                            new KhoaHocDTO()
                            {
                                ma = maTam
                            };
                        break;
                    default:
                        break;
                }
            }

            baiViet.danhSachBaiTapNop = LienKet.co(lienKet, "BaiTapNop") && baiViet.ma.HasValue ?
                layDanhSachDTO<BaiTapNopDTO>(BaiTapNopDAO.layTheoMaBaiVietBaiTap(baiViet.ma.Value, lienKet["BaiTapNop"])) :
                null;

            return baiViet;
        }

        public static KetQua them(BaiVietBaiTapDTO baiTap, LienKet lienKet = null)
        {
            return layDong
            (
                "themBaiVietBaiTap",
                new object[] 
                {
                    baiTap.tieuDe,
                    baiTap.noiDung,
                    layMa(baiTap.tapTin),
                    baiTap.loai,
                    baiTap.thoiDiemHetHan,
                    layMa(baiTap.nguoiTao),
                    layMa(baiTap.khoaHoc)
                },
                lienKet
            );
        }

        public static KetQua layTheoMaKhoaHoc(int? maKhoaHoc, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layBaiVietBaiTapTheoMaKhoaHoc",
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
                "xoaBaiVietBaiTapTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
            (
                "layBaiVietBaiTapTheoMa",
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
                "capNhatBaiVietBaiTapTheoMa",
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
