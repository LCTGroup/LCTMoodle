using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;

namespace DAOLayer
{
    public class TapTinDAO : DAO
    {
        public static KetQua themTapTin(TapTinDataDTO tapTin)
        {
            return layDong<TapTinViewDTO>(
                "themTapTin",
                new object[] 
                {
                    tapTin.ten,
                    tapTin.loai,
                    tapTin.thuMuc
                }
            );
        }

        public static KetQua layTapTinTheoMa(int ma)
        {
            return layDong<TapTinViewDTO>(
                "layTapTinTheoMa",
                new object[] 
                {
                    ma
                }
            );
        }
    }
}
