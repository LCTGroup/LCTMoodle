﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using System.Web;

namespace BUSLayer
{
    public class KhoaHocBUS : BUS
    {
        public static KetQua kiemTra(KhoaHocDataDTO khoaHoc)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(khoaHoc.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (string.IsNullOrEmpty(khoaHoc.moTa))
            {
                loi.Add("Mô tả không được bỏ trống");
            }
            if (khoaHoc.maChuDe == 0)
            {
                loi.Add("Chủ đề không được bỏ trống");
            }
            if (khoaHoc.maHinhDaiDien == 0)
            {
                loi.Add("Hình đại diện không được bỏ trống");
            }
            if (khoaHoc.maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (CheDoRiengTu.lay(khoaHoc.cheDoRiengTu) == null)
            {
                loi.Add("Chế độ riêng tư không hợp lệ");
            }
            #endregion

            if (loi.Count > 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = loi
                };
            }
            else
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }
        }

        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("KhoaHoc_HinhDaiDien", layInt(form, "HinhDaiDien"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            //Chưa xử lý quản lý
            KhoaHocDataDTO khoaHoc  = new KhoaHocDataDTO()
            {
                ten = layString(form, "Ten"),
                moTa = layString(form, "MoTa"),
                maChuDe = layInt(form, "ChuDe"),
                maNguoiTao = (int)Session["NguoiDung"],
                maHinhDaiDien = (ketQua.ketQua as TapTinViewDTO).ma,
                canDangKy = layBool(form, "CanDangKy"),
                phiThamGia = layInt(form, "PhiThamGia"),
                cheDoRiengTu = layString(form, "CheDoRiengTu")
            };

            if (layBool(form, "CoHan"))
            {
                khoaHoc.han = layDateTime_Full(form, "Han_Ngay", "Han_Gio");
            }

            if (khoaHoc.canDangKy && layBool(form, "CoHanDangKy"))
            {
                khoaHoc.hanDangKy = layDateTime_Full(form, "HanDangKy_Ngay", "HanDangKy_Gio");
            }

            ketQua = kiemTra(khoaHoc);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return KhoaHocDAO.them(khoaHoc);
        }

        public static KetQua layTheoMa(int ma)
        {
            return KhoaHocDAO.layTheoMa(ma);
        }
    }
}
