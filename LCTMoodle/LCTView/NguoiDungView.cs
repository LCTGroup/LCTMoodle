using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;

namespace LCTMoodle.LCTView
{
    public class NguoiDungView
    {
        /// <summary>
        /// Tạo ra thẻ link người dùng
        /// </summary>
        /// <param name="nguoiDung">Người dùng</param>
        /// <param name="thamSo">Gồm có: class, style</param>
        /// <returns></returns>
        public static HtmlString link(NguoiDungDTO nguoiDung, Dictionary<string, string> thamSo = null)
        {
            if (nguoiDung == null)
            {
                return null;
            }

            if (thamSo == null)
            {
                thamSo = new Dictionary<string, string>();
            }

            return new HtmlString("<a class=" + 
                (thamSo.ContainsKey("class") ? thamSo["class"] : null) + " style=" + 
                (thamSo.ContainsKey("style") ? thamSo["style"] : null) + " href='/NguoiDung/Xem/" + 
                nguoiDung.ma + "'>" + nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten + "</a>");
        }

        public static HtmlString hinhDaiDien(NguoiDungDTO nguoiDung, Dictionary<string, string> thamSo = null)
        {
            if (nguoiDung == null)
            {
                return null;
            }

            if (thamSo == null)
            {
                thamSo = new Dictionary<string, string>();
            }

            return new HtmlString("<img class=" + 
                (thamSo.ContainsKey("class") ? thamSo["class"] : null) + " style=" + 
                (thamSo.ContainsKey("style") ? thamSo["style"] : null) + " href='/NguoiDung/Xem/' alt='" +
                nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten + "' src='" +
                (nguoiDung.hinhDaiDien == null ? "/HinhDaiDienMacDinh.png/NguoiDung" : "/LayHinh/NguoiDung_HinhDaiDien/" + nguoiDung.hinhDaiDien.ma) + "'></img>");
            
        }
    }
}