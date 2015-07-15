using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTOLayer;
using BUSLayer;

namespace LCTMoodle.LCTView
{
    public class HoatDongView
    {
        //Tạm thời chỉ đọc chủ động
        public static HtmlString link(HoatDongDTO hoatDong, Dictionary<string, string> thamSo = null)
        {
            if (hoatDong == null || hoatDong.hanhDong.loiNhan == null)
            {
                return null;
            }

            if (thamSo == null)
            {
                thamSo = new Dictionary<string, string>();
            }

            var maNguoiDung = HttpContext.Current.Session["NguoiDung"] as int?;

            string chuoi = hoatDong.hanhDong.loiNhan;
            KetQua ketQua;
            #region ND
		    if (chuoi.IndexOf("{ND}") != -1)
            {
                if (hoatDong.maNguoiTacDong == maNguoiDung)
                {
                    chuoi = chuoi.Replace("{ND}", "Bạn");
                }
                else
                {
                    ketQua = NguoiDungBUS.layTheoMa(hoatDong.maNguoiTacDong.Value);
                    if (ketQua.trangThai == 0)
                    {
                        var nguoiDung = ketQua.ketQua as NguoiDungDTO;
                        chuoi = chuoi.Replace("{ND}", nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten);
                    }
                    else
                    {
                        chuoi = chuoi.Replace("{ND}", "người dùng Moodle");
                    }
                }
            } 
	        #endregion

            #region CD
		    if (chuoi.IndexOf("{CD}") != -1)
            {
                switch (hoatDong.loaiDoiTuongTacDong)
                {
                    case "ND":
                        if (hoatDong.maDoiTuongTacDong == maNguoiDung)
                        {
                            chuoi = chuoi.Replace("{CD}", "Bạn");
                        }
                        else
                        {
                            ketQua = NguoiDungBUS.layTheoMa(hoatDong.maDoiTuongTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var nguoiDung = ketQua.ketQua as NguoiDungDTO;
                                chuoi = chuoi.Replace("{CD}", nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten);
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{CD}", "người dùng Moodle");
                            }
                        }
                        break;
                    case "CD":
                        if (hoatDong.maDoiTuongTacDong.HasValue)
                        {
                            ketQua = ChuDeBUS.layTheoMa(hoatDong.maDoiTuongTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var chuDe = ketQua.ketQua as ChuDeDTO;
                                chuoi = chuoi.Replace("{CD}", "\"" + chuDe.ten + "\"");
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{CD}", "hệ thống chủ đề");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{CD}", "hệ thống chủ đề");
                        }
                        break;
                    case "HD":
                        if (hoatDong.maDoiTuongTacDong.HasValue)
                        {
                            ketQua = CauHoiBUS.layTheoMa(hoatDong.maDoiTuongTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var cauHoi = ketQua.ketQua as CauHoiDTO;
                                if (cauHoi.nguoiTao.ma == maNguoiDung)
                                {
                                    chuoi = chuoi.Replace("{CD}", "của bạn");
                                }
                                else
                                {
                                    chuoi = chuoi.Replace("{CD}", "\"" + cauHoi.tieuDe + "\"");
                                }
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{CD}", "ở hệ thống");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{CD}", "ở hệ thống");
                        }
                        break;
                    case "KH":
                        if (hoatDong.maDoiTuongTacDong.HasValue)
                        {
                            ketQua = KhoaHocBUS.layTheoMa(hoatDong.maDoiTuongTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var khoaHoc = ketQua.ketQua as KhoaHocDTO;
                                chuoi = chuoi.Replace("{CD}", "\"" + khoaHoc.ten + "\"");
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{CD}", "ở hệ thống");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{CD}", "ở hệ thống");
                        }
                        break;
                    case "Q":
                        if (hoatDong.maDoiTuongTacDong.HasValue)
                        {
                            ketQua = QuyenBUS.layTheoMa(hoatDong.maDoiTuongTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var quyen = ketQua.ketQua as QuyenDTO;
                                chuoi = chuoi.Replace("{CD}", "\"" + quyen.ten + "\"");
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{CD}", "ở hệ thống");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{CD}", "ở hệ thống");
                        }
                        break;
                    default:
                        chuoi = chuoi.Replace("{CD}", "hệ thống");
                        break;
                }
            }
            #endregion

            #region BD
            if (chuoi.IndexOf("{BD}") != -1)
            {
                switch (hoatDong.loaiDoiTuongBiTacDong)
                {
                    case "ND":
                        if (hoatDong.maDoiTuongBiTacDong == maNguoiDung)
                        {
                            chuoi = chuoi.Replace("{BD}", "Bạn");
                        }
                        else
                        {
                            ketQua = NguoiDungBUS.layTheoMa(hoatDong.maDoiTuongBiTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var nguoiDung = ketQua.ketQua as NguoiDungDTO;
                                chuoi = chuoi.Replace("{BD}", nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten);
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{BD}", "người dùng Moodle");
                            }
                        }
                        break;
                    case "CD":
                        if (hoatDong.maDoiTuongBiTacDong.HasValue)
                        {
                            ketQua = ChuDeBUS.layTheoMa(hoatDong.maDoiTuongBiTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var chuDe = ketQua.ketQua as ChuDeDTO;
                                chuoi = chuoi.Replace("{BD}", chuDe.ten);
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{BD}", "hệ thống chủ đề");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{BD}", "hệ thống chủ đề");
                        }
                        break;
                    case "HD":
                        if (hoatDong.maDoiTuongBiTacDong.HasValue)
                        {
                            ketQua = CauHoiBUS.layTheoMa(hoatDong.maDoiTuongBiTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var cauHoi = ketQua.ketQua as CauHoiDTO;
                                if (cauHoi.nguoiTao.ma == maNguoiDung)
                                {
                                    chuoi = chuoi.Replace("{BD}", "của bạn");
                                }
                                else
                                {
                                    chuoi = chuoi.Replace("{BD}", cauHoi.tieuDe);
                                }
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{BD}", "ở hệ thống");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{BD}", "ở hệ thống");
                        }
                        break;
                    case "KH":
                        if (hoatDong.maDoiTuongBiTacDong.HasValue)
                        {
                            ketQua = KhoaHocBUS.layTheoMa(hoatDong.maDoiTuongBiTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var khoaHoc = ketQua.ketQua as KhoaHocDTO;
                                chuoi = chuoi.Replace("{BD}", khoaHoc.ten);
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{BD}", "ở hệ thống");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{BD}", "ở hệ thống");
                        }
                        break;
                    case "Q":
                        if (hoatDong.maDoiTuongBiTacDong.HasValue)
                        {
                            ketQua = QuyenBUS.layTheoMa(hoatDong.maDoiTuongBiTacDong.Value);
                            if (ketQua.trangThai == 0)
                            {
                                var quyen = ketQua.ketQua as QuyenDTO;
                                chuoi = chuoi.Replace("{BD}", quyen.ten);
                            }
                            else
                            {
                                chuoi = chuoi.Replace("{BD}", "ở hệ thống");
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace("{BD}", "ở hệ thống");
                        }
                        break;
                    default:
                        chuoi = chuoi.Replace("{BD}", "hệ thống");
                        break;
                }
            }
            #endregion

            chuoi = char.ToUpper(chuoi[0]) + chuoi.Substring(1);

            return new HtmlString("<a class=" +
                (thamSo.ContainsKey("class") ? thamSo["class"] : null) + " style=" + (thamSo.ContainsKey("style") ? thamSo["style"] : null) +
                (string.IsNullOrWhiteSpace(hoatDong.duongDan) ? null : "href='" + hoatDong.duongDan + "'>") +
                chuoi +
                "</a>");
        }

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
                (thamSo.ContainsKey("class") ? thamSo["class"] : null) + " style=" + (thamSo.ContainsKey("style") ? thamSo["style"] : null) +
                " href='/NguoiDung/Xem/" + nguoiDung.ma + "'>" +
                nguoiDung.ho + " " + nguoiDung.tenLot + " " + nguoiDung.ten +
                "</a>");
        }
    }
}