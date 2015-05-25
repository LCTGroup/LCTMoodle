using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiVietBaiGiangDAO : DAO
    {
        public static KetQua them(BaiVietBaiGiangDataDTO baiVietBaiGiang)
        {
            return layDong<BaiVietBaiGiangViewDTO>
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
            return layDanhSachDong<BaiVietBaiGiangViewDTO>
                (
                    "layBaiVietBaiGiangTheoMaKhoaHoc",
                    new object[] 
                    { 
                        maKhoaHoc
                    }
                );
        }
    }
}
