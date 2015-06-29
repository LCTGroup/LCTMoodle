using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;

namespace LCTMoodle.LCTView
{
    public class NguoiDungView
    {
        public static HtmlString link(NguoiDungDTO nguoiDung)
        {
            if (nguoiDung == null)
            {
                return null;
            }

            return new HtmlString("<a href='/NguoiDung/Xem/" + nguoiDung.ma + "'>" + nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten + "</a>");
        }
    }
}