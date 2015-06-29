﻿using System;
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
        public ActionResult QuanLy(int ma = 0)
        {
            KetQua ketQua;
            ChuDeDTO chuDe = null;

            #region Kiểm tra quyền
            if (ma != 0)
            {
                ketQua = ChuDeBUS.layTheoMa(ma);
                if (ketQua.trangThai != 0)
                {
                    return Redirect("/");
                }
                chuDe = ketQua.ketQua as ChuDeDTO;
            }

            if (!BUS.coQuyen("QLQuyen", "CD", ma))
            {
                return Redirect("/");
            }
            #endregion

            ketQua = ChuDeBUS.layTheoMaCha(ma);
            if (ketQua.trangThai > 1)
            {
                return Redirect("/");
            }
            ViewData["DSCon"] = ketQua.ketQua;

            return View(chuDe);
        }

        /// <returns>
        ///     Mảng gồm
        ///         cay: html của nút ở cây
        ///         danhSach: html của danh sách nút con của nút
        /// </returns>
        public ActionResult _Khung(int ma, bool coChon)
        {
            ChuDeDTO chuDe;
            if (ma != 0)
            {
                KetQua ketQua = ChuDeBUS.layTheoMa(ma, new LienKet() { "Con" });

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
                        chuDe.con,
                        new ViewDataDictionary() { { "CoChon", coChon } }
                    )
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _FormTao(int maCha)
        {
            ViewData["MaCha"] = maCha;
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Form.cshtml", null, ViewData)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _FormSua(int ma)
        {
            var ketQua = ChuDeBUS.layTheoMa(ma);

            if (ketQua.trangThai == 0)
            {
                ketQua.ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Form.cshtml", ketQua.ketQua);
            }

            return Json(ketQua, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            Form form = chuyenForm(formCollection);
            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", ((int)Session["NguoiDung"]).ToString());
            }
            KetQua ketQua = ChuDeBUS.them(form);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = new
                {
                    cayCon_Item = renderPartialViewToString(ControllerContext,
                        "ChuDe/_Cay_Con_Item.cshtml",
                        ketQua.ketQua
                    ),
                    danhSach_Item = renderPartialViewToString(ControllerContext,
                        "ChuDe/_DanhSach_Item.cshtml",
                        ketQua.ketQua,
                        new ViewDataDictionary() { { "CoChon", formCollection["CoChon"] == "1" ? true : false } }
                    )
                }
            });
        }
        
        [HttpPost]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            var ketQua = ChuDeBUS.capNhatTheoMa(chuyenForm(formCollection));
            if (ketQua.trangThai == 0)
            {
                ketQua.ketQua = new
                {
                    cayCon_Item = renderPartialViewToString(ControllerContext,
                        "ChuDe/_Cay_Con_Item.cshtml",
                        ketQua.ketQua
                    ),
                    danhSach_Item = renderPartialViewToString(ControllerContext,
                        "ChuDe/_DanhSach_Item.cshtml",
                        ketQua.ketQua,
                        new ViewDataDictionary() { { "CoChon", formCollection["CoChon"] == "1" ? true : false } }
                    )
                };
            }

            return Json(ketQua);
        }

        [HttpPost]
        public ActionResult XuLyChuyen(int ma, int maCha)
        {
            return Json(ChuDeBUS.capNhatCha(ma, maCha));
        }

        [HttpPost]
        public ActionResult XuLyXoa(int ma)
        {
            return Json(ChuDeDAO.xoaTheoMa(ma));
        }

        public ActionResult _Chon(int ma = 0)
        {
            KetQua ketQua;
            ChuDeDTO chuDe = null;

            if (ma != 0)
            {
                ketQua = ChuDeBUS.layTheoMa(ma);
                if (ketQua.trangThai != 0)
                {
                    return Json(ketQua, JsonRequestBehavior.AllowGet);
                }
                chuDe = ketQua.ketQua as ChuDeDTO;
            }

            ketQua = ChuDeBUS.layTheoMaCha(ma);

            if (ketQua.trangThai > 1)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewData["DSCon"] = ketQua.ketQua;
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Chon.cshtml", chuDe, ViewData)
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