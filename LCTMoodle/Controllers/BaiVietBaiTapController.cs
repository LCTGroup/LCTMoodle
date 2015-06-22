using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using Data;

namespace LCTMoodle.Controllers
{
    public class BaiVietBaiTapController : LCTController
    {
        public ActionResult _Khung(int maKhoaHoc)
        {
            ViewData["MaKhoaHoc"] = maKhoaHoc;

            KetQua ketQua = BaiVietBaiTapBUS.layTheoMaKhoaHoc(maKhoaHoc);
            List<BaiVietBaiTapDTO> danhSachBaiViet = 
                ketQua.trangThai == 0 ?
                ketQua.ketQua as List<BaiVietBaiTapDTO> :
                null;

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

        public ActionResult _Form(int ma = 0)
        {
            KetQua ketQua = BaiVietBaiTapBUS.layTheoMa(ma);

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
                        renderPartialViewToString(ControllerContext, "BaiVietBaiTap/_Form.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            Form form = chuyenForm(formCollection);
            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", ((int)Session["NguoiDung"]).ToString());
            }
            KetQua ketQua = BaiVietBaiTapBUS.them(form);

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

        [HttpPost]
        public ActionResult XuLyXoa(int ma)
        {
            return Json(
                BaiVietBaiTapBUS.xoaTheoMa(ma),
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyCapNhat(FormCollection form)
        {
            KetQua ketQua = BaiVietBaiTapBUS.capNhatTheoMa(chuyenForm(form));

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
                return Json
                (
                    ketQua
                );
            }
        }
	}
}