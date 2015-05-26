using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Helpers;
using Data;

namespace LCTMoodle.Controllers
{
    public class ChuDeController : LCTController
    {
        public ActionResult QuanLy(string phamVi = null, int ma = 0)
        {
            return View(model: phamVi == null ? null : 
                new ChuDeViewDTO()
                {
                    ten = phamVi
                });
        }

        /// <returns>
        ///     Mảng gồm
        ///         cay: html của nút ở cay
        ///         danhSach: html của danh sách nút con của nút
        /// </returns>
        public ActionResult _Khung(string phamVi, int maChuDeCha = 0)
        {
            if (phamVi == "PhamVi")
            {
                List<PhamVi> danhSachPhamVi = PhamVi.layDanhSach();

                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = new
                    {
                        cay = renderPartialViewToString(ControllerContext, "ChuDe/Khung/_Cay_HeThong_Item.cshtml", danhSachPhamVi),
                        danhSach = renderPartialViewToString(ControllerContext, "ChuDe/Khung/_DanhSach_PhamVi.cshtml", danhSachPhamVi)
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PhamVi pv = PhamVi.lay(phamVi);

                if (pv == null)
                {
                    return Json(new KetQua()
                    {
                        trangThai = 1,
                    }, JsonRequestBehavior.AllowGet);
                }

                KetQua ketQua = ChuDeDAO.layTheoMaChuDeCha(phamVi, maChuDeCha);

                if (ketQua.trangThai != 0 && ketQua.trangThai != 1)
                {
                    return Json(ketQua, JsonRequestBehavior.AllowGet);
                }

                object danhSachChuDeCon = ketQua.ketQua;

                if (maChuDeCha == 0)
                {
                    return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = new
                        {
                            cay = renderPartialViewToString(ControllerContext, 
                                "ChuDe/Khung/_Cay_PhamVi_Item.cshtml", 
                                danhSachChuDeCon, 
                                new ViewDataDictionary()
                                {
                                    { "Ma", pv.ma },
                                    { "Ten", pv.ten }
                                }
                            ),
                            danhSach = renderPartialViewToString(ControllerContext, "ChuDe/Khung/_DanhSach.cshtml", danhSachChuDeCon)
                        }
                    }, JsonRequestBehavior.AllowGet);
                }

                ketQua = ChuDeDAO.layTheoMa(maChuDeCha);

                if (ketQua.trangThai != 0)
                {
                    return Json(ketQua, JsonRequestBehavior.AllowGet);
                }

                ChuDeViewDTO chuDe = ketQua.ketQua as ChuDeViewDTO;

                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = new
                    {
                        cay = renderPartialViewToString(ControllerContext,
                            "ChuDe/Khung/_Cay_Item.cshtml",
                            danhSachChuDeCon,
                            new ViewDataDictionary()
                            {
                                { "Ma", chuDe.ma },
                                { "Ten", chuDe.ten }
                            }
                        ),
                        danhSach = renderPartialViewToString(ControllerContext,
                            "ChuDe/khung/_DanhSach.cshtml",
                            danhSachChuDeCon
                        )
                    }
                }, JsonRequestBehavior.AllowGet);
            } 
        }

        public ActionResult _Form(int maChuDeCha, string phamVi = "HeThong")
        {
            ViewData["MaChuDeCha"] = maChuDeCha;
            ViewData["PhamVi"] = phamVi;

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Form.cshtml", null, ViewData)
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            return Json(ChuDeBUS.them(chuyenDuLieuForm(formCollection)));
        }

        public ActionResult Chon(string phamVi = "HeThong")
        {
            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Chon.cshtml")
                }, JsonRequestBehavior.AllowGet);
        }
	}
}