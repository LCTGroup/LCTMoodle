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
    public class BaiVietBaiGiangController : LCTController
    {
        public ActionResult _Khung(int maKhoaHoc)
        {
            ViewData["MaKhoaHoc"] = maKhoaHoc;

            KetQua ketQua = BaiVietBaiGiangBUS.layTheoMaKhoaHoc(maKhoaHoc);
            List<BaiVietBaiGiangDTO> danhSachBaiViet = 
                ketQua.trangThai == 0 ?
                (List<BaiVietBaiGiangDTO>)ketQua.ketQua :
                null;

            try
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua =
                            renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Khung.cshtml", danhSachBaiViet, ViewData)
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

        public ActionResult _Form(int ma = 0)
        {
            KetQua ketQua = BaiVietBaiGiangBUS.layTheoMa(ma);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua =
                        renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Form.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            KetQua ketQua = BaiVietBaiGiangBUS.them(chuyenDuLieuForm(formCollection));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Item.cshtml", ketQua.ketQua)
                    });
            }
            else
            {
                return Json(ketQua);
            }
        }

        [HttpPost]
        public ActionResult XuLyXoa(int ma)
        {
            return Json(
                BaiVietBaiGiangDAO.xoaTheoMa(ma),
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyCapNhat(FormCollection form)
        {
            KetQua ketQua = BaiVietBaiGiangBUS.capNhatTheoMa(chuyenForm(form));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "BaiVietBaiGiang/_Item.cshtml", ketQua.ketQua)
                });
            }
            else
            {
                return Json
                (
                    ketQua
                );
            }
        }
	}
}