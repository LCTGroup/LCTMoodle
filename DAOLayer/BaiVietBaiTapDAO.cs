using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietBaiTapDAO : DAO<BaiVietBaiTapDAO, BaiVietBaiTapViewDTO>
    {
        public static BaiVietBaiTapViewDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BaiVietBaiTapViewDTO baiViet = new BaiVietBaiTapViewDTO();

            int maTam;
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

                        if (maTam != 0)
                        {
                            if (LienKet.co(lienKet, "TapTin"))
                            {
                                KetQua ketQua = TapTinDAO.layTheoMa("BaiVietDienDan_TapTin", maTam);

                                if (ketQua.trangThai == 0)
                                {
                                    baiViet.tapTin = ketQua.ketQua as TapTinViewDTO;
                                }
                            }
                            else
                            {
                                baiViet.tapTin = new TapTinViewDTO()
                                {
                                    ma = maTam
                                };
                            }
                        }
                        break;
                    case "ThoiDiemHetHan":
                        baiViet.thoiDiemHetHan = layDateTime(dong, i);
                        break;
                    case "ThoiDiemTao":
                        baiViet.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam != 0)
                        {
                            if (LienKet.co(lienKet, "NguoiTao"))
                            {
                                KetQua ketQua = NguoiDungDAO.layTheoMa(maTam);

                                if (ketQua.trangThai == 0)
                                {
                                    baiViet.nguoiTao = ketQua.ketQua as NguoiDungViewDTO;
                                }
                            }
                            else
                            {
                                baiViet.nguoiTao = new NguoiDungViewDTO()
                                {
                                    ma = maTam
                                };
                            }
                        }
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
                                    baiViet.khoaHoc = ketQua.ketQua as KhoaHocViewDTO;
                                }
                            }
                            else
                            {
                                baiViet.khoaHoc = new KhoaHocViewDTO()
                                {
                                    ma = maTam
                                };
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            if (LienKet.co(lienKet, "BaiTapNop"))
            {
                KetQua ketQua = BaiTapNopDAO.layTheoMaBaiVietBaiTap(baiViet.ma);
                
                if (ketQua.trangThai == 0)
                {
                    baiViet.danhSachBaiTapNop = ketQua.ketQua as List<BaiTapNopViewDTO>;
                }
            }

            return baiViet;
        }

        public static KetQua them(BaiVietBaiTapDataDTO baiVietBaiTap, LienKet lienKet = null)
        {
            return layDong
            (
                "themBaiVietBaiTap",
                new object[] 
                {
                    baiVietBaiTap.tieuDe,
                    baiVietBaiTap.noiDung,
                    baiVietBaiTap.maTapTin,
                    baiVietBaiTap.thoiDiemHetHan,
                    baiVietBaiTap.maNguoiTao,
                    baiVietBaiTap.maKhoaHoc
                },
                lienKet
            );
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc, LienKet lienKet = null)
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

        public static KetQua xoaTheoMa(int ma)
        {
            return khongTruyVan(
                "xoaBaiVietBaiTapTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }
    }
}
