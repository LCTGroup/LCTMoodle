using System;
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
    public class BaiVietDienDanController : LCTController
    {
        public ActionResult _Khung(int maKhoaHoc)
        {
            ViewData["MaKhoaHoc"] = maKhoaHoc;

            KetQua ketQua = BaiVietDienDanBUS.layTheoMaKhoaHoc(maKhoaHoc);

            List<BaiVietDienDanViewDTO> danhSachBaiViet = 
                ketQua.trangThai == 0 ?
                ketQua.ketQua as List<BaiVietDienDanViewDTO> :
                new List<BaiVietDienDanViewDTO>();

            try
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua =
                            renderPartialViewToString(ControllerContext, "BaiVietDienDan/_Khung.cshtml", danhSachBaiViet, ViewData)
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
            KetQua ketQua = BaiVietDienDanBUS.them(chuyenDuLieuForm(formCollection));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BaiVietDienDan/_Item.cshtml", ketQua.ketQua)
                    });
            }
            else
            {
                return Json(ketQua);
            }
        }

        [HttpPost]
        public ActionResult Xoa(int ma)
        {
            return Json(
                BaiVietDienDanDAO.xoaTheoMa(ma), 
                JsonRequestBehavior.AllowGet
            );
        }
	}
}