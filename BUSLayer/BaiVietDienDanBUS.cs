﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class BaiVietDienDanBUS : BUS
    {
        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("BaiVietDienDan_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietDienDanDataDTO baiVietDienDan = new BaiVietDienDanDataDTO()
            {
                tieuDe = layString(form, "TieuDe"),
                noiDung = layString(form, "NoiDung"),
                maTapTin = (ketQua.ketQua as TapTinViewDTO).ma,
                maNguoiTao = 1, //Tạm
                maKhoaHoc = layInt(form, "KhoaHoc")
            };
            
            ketQua = baiVietDienDan.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietDienDanDAO.them(baiVietDienDan);
        }
    }
}
