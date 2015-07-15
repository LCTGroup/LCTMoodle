using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;

namespace LCTMoodle.LCTView
{
    public class View
    {
        public static string HienThiThoiGian(DateTime? thoiGian)
        {
            if (!thoiGian.HasValue)
            {
                return null;
            }
            
            if (thoiGian.Value.Day == DateTime.Now.Day)
            {
                return string.Format("Hôm nay, lúc {0} giờ {1} phút", thoiGian.Value.Hour, thoiGian.Value.Minute);
            }

            if (thoiGian.Value.Day == DateTime.Now.Day - 1)
            {
                return string.Format("Hôm qua, lúc {0} giờ {1} phút", thoiGian.Value.Hour, thoiGian.Value.Minute);
            }

            return string.Format("{0} tháng {1}, lúc {2} giờ {3} phút", thoiGian.Value.Day, thoiGian.Value.Month + 1, thoiGian.Value.Hour, thoiGian.Value.Minute);
        }
    }
}