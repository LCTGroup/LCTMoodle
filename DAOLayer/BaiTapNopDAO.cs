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
    public class BaiTapNopDAO : DAO<BaiTapNopDAO, BaiTapNopDTO>
    {
        public static BaiTapNopDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BaiTapNopDTO baiTapNop = new BaiTapNopDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        baiTapNop.ma = layInt(dong, i);
                        break;
                    case "MaTapTin":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiTapNop.tapTin = LienKet.co(lienKet, "TapTin") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("BaiTapNop_TapTin", maTam.Value, lienKet["TapTin"])) :
                                new TapTinDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "DuongDan":
                        baiTapNop.duongDan = layString(dong, i);
                        break;
                    case "ThoiDiemTao":
                        baiTapNop.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiTapNop.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaBaiVietBaiTap":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            baiTapNop.baiVietBaiTap = LienKet.co(lienKet, "BaiVietBaiTap") ?
                                layDTO<BaiVietBaiTapDTO>(BaiVietBaiTapDAO.layTheoMa(maTam, lienKet["BaiVietBaiTap"])) : //Tạm -- Nhớ đổi ko lỗi
                                new BaiVietBaiTapDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "DaChuyenDiem":
                        baiTapNop.daChuyenDiem = layBool(dong, i);
                        break;
                    case "Diem":
                        baiTapNop.diem = layDouble(dong, i);
                        break;
                    case "GhiChu":
                        baiTapNop.ghiChu = layString(dong, i);
                        break;
                    case "DaXoa":
                        baiTapNop.daXoa = layBool(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return baiTapNop;
        }

        public static KetQua themHoacCapNhat(BaiTapNopDTO baiTapNop)
        {
            return layDong
            (
                "themHoacCapNhatBaiTapNop",
                new object[] 
                { 
                    layMa(baiTapNop.tapTin),
                    baiTapNop.duongDan,
                    layMa(baiTapNop.nguoiTao),
                    layMa(baiTapNop.baiVietBaiTap)
                }
            );
        }

        public static KetQua layTheoMaBaiVietBaiTap(int? maBaiVietBaiTap, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layBaiTapNopTheoMaBaiVietBaiTap",
                new object[]
                {
                    maBaiVietBaiTap
                },
                lienKet
            );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layBaiTapNopTheoMa",
                    new object[]
                    {
                        ma
                    },
                    lienKet
                );
        }

        public static KetQua capNhatTheoMa_Diem(int? ma, double? diem)
        {
            return khongTruyVan
                (
                    "capNhatBaiTapNopTheoMa_Diem",
                    new object[] 
                    { 
                        ma,
                        diem
                    }
                );
        }

        public static KetQua capNhatTheoMa_GhiChu(int? ma, string ghiChu)
        {
            return khongTruyVan
                (
                    "capNhatBaiTapNopTheoMa_GhiChu",
                    new object[] 
                    { 
                        ma,
                        ghiChu
                    }
                );
        }

        public static KetQua layTheoMaBaiVietBaiTapVaDaChuyenDiem(int? maBaiVietBaiTap, bool daChuyenDiem, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layBaiTapNopTheoMaBaiVietBaiTapVaDaChuyenDiem",
                    new object[]
                    {
                        maBaiVietBaiTap,
                        daChuyenDiem
                    },
                    lienKet
                );
        }

        public static KetQua capNhatTheoMaBaiVietBaiTap_DaChuyenDiem(int? maBaiVietBaiTap)
        {
            return khongTruyVan
                (
                    "capNhatBaiTapNopTheoMaBaiVietBaiTap_DaChuyenDiem",
                    new object[]
                    {
                        maBaiVietBaiTap
                    }
                );
        }

        public static KetQua capNhatTheoMa_DaXoa(int? ma, string ghiChu)
        {
            return khongTruyVan
                (
                    "capNhatBaiTapNopTheoMa_DaXoa",
                    new object[]
                    {
                        ma, 
                        ghiChu
                    }
                );
        }

        public static KetQua capNhatTheoMa_DaXoa_Nhieu(string dsMa, string ghiChu)
        {
            return khongTruyVan
                (
                    "capNhatBaiTapNopTheoMa_DaXoa_Nhieu",
                    new object[]
                    {
                        dsMa,
                        ghiChu
                    }
                );
        }
    }
}
