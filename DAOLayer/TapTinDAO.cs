using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class TapTinDAO : DAO
    {
        public static KetQua them(TapTinDataDTO tapTin)
        {
            return layDong<TapTinViewDTO>
                (
                    "themTapTin",
                    new object[] 
                    { 
                        tapTin.ten,
                        tapTin.loai,
                        tapTin.duoi
                    }
                );
        }

        public static KetQua chuyen(int maTapTin, string loai)
        {
            return layDong<TapTinViewDTO>
                (
                    "chuyenTapTin",
                    new object[] 
                    { 
                        maTapTin,
                        loai
                    }
                );
        }
       
        public static KetQua lay(string loai, int ma)
        {
            return layDong<TapTinViewDTO>
                (
                    "layTapTin",
                    new object[] 
                    { 
                        loai,
                        ma
                    }
                );
        }
    }
}
