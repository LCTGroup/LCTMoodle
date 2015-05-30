using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietBaiGiangDAO : DAO<BaiVietBaiGiangDAO, BaiVietBaiGiangViewDTO>
    {
        public static BaiVietBaiGiangViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            BaiVietBaiGiangViewDTO baiViet = new BaiVietBaiGiangViewDTO();

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
                        baiViet.tapTin = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
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

        public static KetQua them(BaiVietBaiGiangDataDTO baiVietBaiGiang)
        {
            return layDong
                (
                    "themBaiVietBaiGiang",
                    new object[] 
                    {
                        baiVietBaiGiang.tieuDe,
                        baiVietBaiGiang.noiDung,
                        baiVietBaiGiang.maTapTin,
                        baiVietBaiGiang.maNguoiTao,
                        baiVietBaiGiang.maKhoaHoc
                    }
                );
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return layDanhSachDong
                (
                    "layBaiVietBaiGiangTheoMaKhoaHoc",
                    new object[] 
                    { 
                        maKhoaHoc
                    }
                );
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return khongTruyVan(
                "xoaBaiVietBaiGiangTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }
    }
}
