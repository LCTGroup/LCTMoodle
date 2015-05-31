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
        public static BaiVietBaiTapViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            BaiVietBaiTapViewDTO baiViet = new BaiVietBaiTapViewDTO();

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
                        int maTapTin = layInt(dong, i);
                        baiViet.tapTin = maTapTin != 0 ?
                            new TapTinViewDTO()
                            {
                                ma = layInt(dong, i)
                            }
                            :
                            null;
                        break;
                    case "ThoiDiemHetHan":
                        baiViet.thoiDiemHetHan = layDateTime(dong, i);
                        break;
                    case "ThoiDiemTao":
                        baiViet.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        baiViet.nguoiTao = new NguoiDungViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaKhoaHoc":
                        baiViet.khoaHoc = new KhoaHocViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }

            return baiViet;
        }

        public static KetQua them(BaiVietBaiTapDataDTO baiVietBaiTap)
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
                }
            );
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return layDanhSachDong
            (
                "layBaiVietBaiTapTheoMaKhoaHoc",
                new object[] 
                { 
                    maKhoaHoc
                }
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
