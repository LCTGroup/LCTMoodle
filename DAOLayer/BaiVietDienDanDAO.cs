using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietDienDanDAO : DAO<BaiVietDienDanDAO, BaiVietDienDanViewDTO>
    {
        public static BaiVietDienDanViewDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BaiVietDienDanViewDTO baiViet = new BaiVietDienDanViewDTO();
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
                            if (lienKet.co("TapTin"))
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
                    case "ThoiDiemTao":
                        baiViet.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam != 0)
                        {
                            if (lienKet.co("NguoiTao"))
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
                            if (lienKet.co("KhoaHoc"))
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

            return baiViet;
        }

        public static KetQua them(BaiVietDienDanDataDTO baiVietDienDan)
        {
            return layDong
                (
                    "themBaiVietDienDan",
                    new object[] 
                    {
                        baiVietDienDan.tieuDe,
                        baiVietDienDan.noiDung,
                        baiVietDienDan.maTapTin,
                        baiVietDienDan.maNguoiTao,
                        baiVietDienDan.maKhoaHoc
                    }
                );
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return layDanhSachDong
                (
                    "layBaiVietDienDanTheoMaKhoaHoc",
                    new object[] 
                    { 
                        maKhoaHoc
                    }
                );
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return khongTruyVan(
                "xoaBaiVietDienDanTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }
    }
}
