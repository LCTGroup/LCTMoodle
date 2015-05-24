﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class BaiTapNopDataDTO : DTO
    {
        public int maTapTin;
        public string duongDan;
        public DateTime? thoiDiemTao;
        public int maNguoiTao;
        public int maBaiVietBaiTap;

        public override KetQua kiemTra()
        {
            KetQua ketQua = new KetQua();
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (maTapTin == 0 && string.IsNullOrEmpty(duongDan))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (maBaiVietBaiTap == 0)
            {
                loi.Add("Bài tập được nộp không được bỏ trống");
            }
            #endregion

            if (loi.Count > 0)
            {
                ketQua.trangThai = 3;
                ketQua.ketQua = loi;
            }
            else 
            {
                ketQua.trangThai = 0;
            }
            
            return ketQua;
        }
    }
}
