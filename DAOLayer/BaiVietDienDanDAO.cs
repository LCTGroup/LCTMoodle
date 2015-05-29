using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietDienDanDAO : DAO
    {
        public static KetQua them(BaiVietDienDanDataDTO baiVietDienDan)
        {
            return layDong<BaiVietDienDanViewDTO>
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
            return layDanhSachDong<BaiVietDienDanViewDTO>
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
