using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BinhLuanBaiVietDienDanDAO : DAO<BinhLuanBaiVietDienDanDAO, BinhLuanBaiVietDienDanDTO>
    {
        public static BinhLuanBaiVietDienDanDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BinhLuanBaiVietDienDanDTO binhLuan = new BinhLuanBaiVietDienDanDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        binhLuan.ma = layInt(dong, i);
                        break;
                    case "NoiDung":
                        binhLuan.noiDung = layString(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            binhLuan.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaBaiVietDienDan":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            binhLuan.baiVietDienDan = LienKet.co(lienKet, "BaiVietDienDan") ?
                                layDTO<BaiVietDienDanDTO>(BaiVietDienDanDAO.layTheoMa(maTam)) :
                                new BaiVietDienDanDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaTapTin":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            binhLuan.tapTin = LienKet.co(lienKet, "TapTin") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("BinhLuanBaiVietDienDan_TapTin", maTam)) :
                                new TapTinDTO()
                                {
                                    ma = layInt(dong, i)
                                };
                        }
                        break;
                    case "ThoiDiemTao":
                        binhLuan.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "Diem":
                        binhLuan.diem = layInt(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return binhLuan;
        }

        public static KetQua them(BinhLuanBaiVietDienDanDTO binhLuan, LienKet lienKet = null)
        {
            return layDong
            (
                "themBinhLuanBaiVietDienDan",
                new object[] { 
                    binhLuan.noiDung,
                    layMa(binhLuan.tapTin),
                    layMa(binhLuan.baiVietDienDan),
                    layMa(binhLuan.nguoiTao),
                },
                lienKet
            );
        }

        public static KetQua layTheoMaBaiVietDienDan(int? maBaiVietDienDan, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layBinhLuanBaiVietDienDanTheoMaBaiVietDienDan",
                new object[] {
                    maBaiVietDienDan
                },
                lienKet
            );
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return khongTruyVan(
                "xoaBinhLuanBaiVietDienDanTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }

        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
            (
                "capNhatBinhLuanBaiVietDienDanTheoMa",
                new object[]
                {
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layBinhLuanBaiVietDienDanTheoMa",
                    new object[]
                    {
                        ma
                    },
                    lienKet
                );
        }

        public static KetQua capNhatTheoMa_Diem(int? ma, int? diem)
        {
            return khongTruyVan
                (
                    "capNhatBinhLuanBaiVietDienDanTheoMa_Diem",
                    new object[]
                    {
                        ma,
                        diem
                    }
                );
        }
    }
}
