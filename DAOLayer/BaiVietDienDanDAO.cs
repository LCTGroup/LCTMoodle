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
        public static BaiVietDienDanViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            BaiVietDienDanViewDTO baiViet = new BaiVietDienDanViewDTO();

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
