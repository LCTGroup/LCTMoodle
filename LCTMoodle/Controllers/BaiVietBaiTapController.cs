using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;

namespace LCTMoodle.Controllers
{
    public class BaiVietBaiTapController : LCTController
    {
        public ActionResult _Khung(int maKhoaHoc)
        {
            ViewData["MaKhoaHoc"] = maKhoaHoc;

            KetQua ketQua = BaiVietBaiTapDAO.layTheoMaKhoaHoc(maKhoaHoc);
            List<BaiVietBaiTapViewDTO> danhSachBaiViet = 
                ketQua.trangThai == 0 ?
                ketQua.ketQua as List<BaiVietBaiTapViewDTO> :
                new List<BaiVietBaiTapViewDTO>();

            try
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua =
                            renderPartialViewToString(ControllerContext, "BaiVietBaiTap/_Khung.cshtml", danhSachBaiViet, ViewData)
                    }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new KetQua()
                    {
                        trangThai = 2
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            KetQua ketQua = BaiVietBaiTapBUS.them(chuyenDuLieuForm(formCollection));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BaiVietBaiTap/_Item.cshtml", ketQua.ketQua)
                    });
            }
            else
            {
                return Json(ketQua);
            }
        }
	}
}