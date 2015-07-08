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
    public class BaiVietDienDanDAO : DAO<BaiVietDienDanDAO, BaiVietDienDanDTO>
    {
        public static BaiVietDienDanDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BaiVietDienDanDTO baiViet = new BaiVietDienDanDTO();

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
                            baiViet.tapTin = 
                                LienKet.co(lienKet, "TapTin") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("BaiVietDienDan_TapTin", maTam.Value, lienKet["TapTin"])) :
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
                                baiViet.khoaHoc = new KhoaHocDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "Ghim":
                        baiViet.ghim = layBool(dong, i);
                        break;
                    case "Diem":
                        baiViet.diem = layInt(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return baiViet;
        }

        public static KetQua them(BaiVietDienDanDTO baiVietDienDan, LienKet lienKet = null)
        {
            return layDong
                (
                    "themBaiVietDienDan",
                    new object[] 
                    {
                        baiVietDienDan.tieuDe,
                        baiVietDienDan.noiDung,
                        layMa(baiVietDienDan.tapTin),
                        layMa(baiVietDienDan.nguoiTao),
                        layMa(baiVietDienDan.khoaHoc)
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaKhoaHoc(int? maKhoaHoc, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layBaiVietDienDanTheoMaKhoaHoc",
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
                "xoaBaiVietDienDanTheoMa",
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
                "layBaiVietDienDanTheoMa",
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
                "capNhatBaiVietDienDanTheoMa",
                new object[]
                {
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }

        public static KetQua capNhatTheoMa_Ghim(int? ma, bool ghim)
        {
            return khongTruyVan
                (
                    "capNhatBaiVietDienDanTheoMa_Ghim",
                    new object[]
                    {
                        ma,
                        ghim
                    }
                );
        }

        public static KetQua capNhatTheoMaKhoaHoc_XoaGhim(int? maKhoaHoc)
        {
            return khongTruyVan
                (
                    "capNhatBaiVietDienDanTheoMaKhoaHoc_XoaGhim",
                    new object[] 
                    {
                        maKhoaHoc
                    }
                );
        }

        public static KetQua capNhatTheoMa_Diem(int? ma, int? diem)
        {
            return khongTruyVan
                (
                    "capNhatBaiVietDienDanTheoMa_Diem",
                    new object[]
                    {
                        ma,
                        diem
                    }
                );
        }
    }
}
