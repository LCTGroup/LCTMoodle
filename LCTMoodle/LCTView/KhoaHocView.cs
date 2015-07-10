using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;

namespace LCTMoodle.LCTView
{
    public class KhoaHocView
    {
        public static HtmlString hinhDaiDien(KhoaHocDTO khoaHoc, Dictionary<string, string> thamSo = null)
        {
            if (khoaHoc == null)
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
                khoaHoc.ten + "' src='" +
                (khoaHoc.hinhDaiDien == null ? "/HinhDaiDienMacDinh.png/KhoaHoc" : "/LayHinh/KhoaHoc_HinhDaiDien/" + khoaHoc.hinhDaiDien.ma) + "'></img>");
            
        }
    }
}