using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietBaiTapDAO : DAO
    {
        public static KetQua them(BaiVietBaiTapDataDTO baiVietBaiTap)
        {
            return layDong<BaiVietBaiTapViewDTO>
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
            return layDanhSachDong<BaiVietBaiTapViewDTO>
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
