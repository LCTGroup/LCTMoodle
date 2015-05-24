using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiTapNopDAO : DAO
    {
        public static KetQua them(BaiTapNopDataDTO baiTapNop)
        {
            return layDong<BaiTapNopViewDTO>
            (
                "themBaiTapNop",
                new object[] 
                { 
                    baiTapNop.maTapTin,
                    baiTapNop.duongDan,
                    baiTapNop.maNguoiTao,
                    baiTapNop.maBaiVietBaiTap
                }
            );
        }
    }
}
