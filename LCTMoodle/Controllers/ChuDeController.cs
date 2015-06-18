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
        public ActionResult QuanLy(int ma = 0)//Mã cha
        {
            KetQua ketQua = ChuDeBUS.layTheoMaCha(ma);

            if (ketQua.trangThai > 1)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            return View(model: ketQua.ketQua);
        }

        /// <returns>
        ///     Mảng gồm
        ///         cay: html của nút ở cây
        ///         danhSach: html của danh sách nút con của nút
        /// </returns>
        public ActionResult _Khung(int ma)
        {
            ChuDeDTO chuDe;
            if (ma != 0)
            {
                KetQua ketQua = ChuDeBUS.layTheoMa(ma);

                if (ketQua.trangThai != 0)
                {
                    return Json(ketQua, JsonRequestBehavior.AllowGet);
                }

                chuDe = ketQua.ketQua as ChuDeDTO;
            }
            else
            {
                KetQua ketQua = ChuDeBUS.layTheoMaCha(ma);

                if (ketQua.trangThai > 1)
                {
                    return Json(ketQua, JsonRequestBehavior.AllowGet);
                }

                chuDe = new ChuDeDTO()
                {
                    ma = 0,
                    ten = "Danh sách",
                    con = ketQua.ketQua as List<ChuDeDTO>
                };
            }

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = new
                {
                    cay = renderPartialViewToString(ControllerContext,
                        "ChuDe/_Cay_Item.cshtml",
                        chuDe.con,
                        new ViewDataDictionary()
                        {
                            { "Ma", chuDe.ma },
                            { "Ten", chuDe.ten }
                        }
                    ),
                    danhSach = renderPartialViewToString(ControllerContext,
                        "ChuDe/_DanhSach.cshtml",
                        chuDe.con
                    )
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Form(int ma)
        {
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Form.cshtml", ma)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            KetQua ketQua = ChuDeBUS.them(chuyenForm(formCollection));

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = new
                {
                    cayCon = renderPartialViewToString(ControllerContext,
                        "ChuDe/_Cay_Con_Item.cshtml",
                        ketQua.ketQua
                    ),
                    danhSach_Item = renderPartialViewToString(ControllerContext,
                        "ChuDe/_DanhSach_Item.cshtml",
                        ketQua.ketQua
                    )
                }
            });
        }
        
        [HttpPost]
        public ActionResult XuLyXoa(int ma)
        {
            return Json(ChuDeDAO.xoaTheoMa(ma));
        }

        public ActionResult _Chon(int ma = 0)
        {
            KetQua ketQua = ChuDeBUS.layTheoMaCha(ma);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Chon.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _GoiY(string tuKhoa)
        {
            KetQua ketQua = ChuDeBUS.lay_TimKiem(tuKhoa);

            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 1
                }, JsonRequestBehavior.AllowGet);
            }

            List<ChuDeDTO> chuDe = ketQua.ketQua as List<ChuDeDTO>;

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = chuDe.Select(x => new { ma = x.ma, ten = x.ten }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
	}
}