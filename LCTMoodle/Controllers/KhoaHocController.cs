using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DAOLayer;
using DTOLayer;
using Data;

namespace LCTMoodle.Controllers
{
    public class KhoaHocController : LCTController
    {
        public ActionResult Xem(int ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);

            if (ketQua.trangThai == 0)
            {
                return View(ketQua.ketQua);
            }
            else
            {
                return RedirectToAction("Index", "TrangChu");
            }
        }

        public ActionResult Tao()
        {
            return View();
        }       

        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(KhoaHocBUS.them(chuyenDuLieuForm(formCollection)));
        }

        public ActionResult _Khung(int ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);

            if (ketQua.trangThai == 0)
            {
                var khoaHoc = ketQua.ketQua;

                ketQua = DAOLayer.BaiVietBaiGiangDAO.layTheoMaKhoaHoc(ma);
                ViewData["BaiGiang"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

                ketQua = DAOLayer.BaiVietBaiTapDAO.layTheoMaKhoaHoc(ma);
                ViewData["BaiTap"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

                ketQua = DAOLayer.BaiVietDienDanDAO.layTheoMaKhoaHoc(ma);
                ViewData["DienDan"] = ketQua.trangThai == 0 ? ketQua.ketQua : null;

                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "KhoaHoc/_Khung.cshtml", khoaHoc, ViewData)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ketQua);
            }
        }

        [HttpPost]
        public ActionResult XuLyDangKyThamGia(int ma)
        {
            return Json(null);
        }

        public ActionResult XuLyMoi(int ma, int maNguoiDung)
        {
            return null;
        }

        public ActionResult XuLyChan(int ma, int maNguoiDung)
        {
            return null;
        }
	}
}