﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Data;

namespace LCTMoodle.Controllers
{
    public class BaiTapNopController : LCTController
    {
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            Form form = chuyenForm(formCollection);
            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", ((int)Session["NguoiDung"]).ToString());
            }
            return Json(BaiTapNopBUS.themHoacCapNhat(form));
        }

        public ActionResult _DanhSachNop(int maBaiTap)
        {
            #region Kiểm tra quyền
            //Lấy bài tập
            var ketQua = BaiVietBaiTapBUS.layTheoMa(maBaiTap);
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài tập không tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }

            var baiTap = ketQua.ketQua as BaiVietBaiTapDTO;
            if (!BUS.coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value))
            {
                return Json(new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xem bài nộp"
                }, JsonRequestBehavior.AllowGet);
            }
            #endregion

            ketQua = BaiTapNopBUS.layTheoMaBaiVietBaiTap(maBaiTap);
            List<BaiTapNopDTO> danhSachBaiTapNop =
                ketQua.trangThai == 0 ?
                    ketQua.ketQua as List<BaiTapNopDTO> :
                    null;

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "BaiTapNop/_DanhSachNop.cshtml", danhSachBaiTapNop)
            }, JsonRequestBehavior.AllowGet);
        }
	}
}