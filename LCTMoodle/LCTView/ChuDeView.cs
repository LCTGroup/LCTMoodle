using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;

namespace LCTMoodle.LCTView
{
    public class ChuDeView
    {
        public static HtmlString hinhDaiDien(ChuDeDTO chuDe, Dictionary<string, string> thamSo = null)
        {
            if (chuDe == null)
            {
                return null;
            }

            if (thamSo == null)
            {
                thamSo = new Dictionary<string, string>();
            }

            return new HtmlString("<img class=" + 
                (thamSo.ContainsKey("class") ? thamSo["class"] : null) + " style=" + 
                (thamSo.ContainsKey("style") ? thamSo["style"] : null) + " alt='" +
                chuDe.ten + "' src='" +
                (chuDe.hinhDaiDien == null ? "/HinhDaiDienMacDinh.png/ChuDe" : "/LayHinh/ChuDe_HinhDaiDien/" + chuDe.hinhDaiDien.ma) + "'></img>");
            
        }
    }
}