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
        public ActionResult QuanLy(int ma = 0)
        {
            KetQua ketQua;
            ChuDeDTO chuDe = null;

            #region Kiểm tra quyền
            var maNguoiDung = Session["NguoiDung"] as int?;
            if (!maNguoiDung.HasValue)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để sử dụng chức năng này."));
            }

            if (ma != 0)
            {
                ketQua = ChuDeBUS.layTheoMa(ma);
                if (ketQua.trangThai != 0)
                {
                    return Redirect("/?tb=" + HttpUtility.UrlEncode("Chủ đề không tồn tại."));
                }
                chuDe = ketQua.ketQua as ChuDeDTO;
            }

            if (!BUS.coQuyen("QLQuyen", "CD", ma, maNguoiDung))
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần có quyền quản lý để thực hiện chức năng này."));
            }
            #endregion

            ketQua = ChuDeBUS.layTheoMaCha(ma);
            if (ketQua.trangThai > 1)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Có lỗi xảy ra."));
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
                            { "Ma", chuDe.ma.Value },
                            { "Ten", chuDe.ten }
                        }
                    ),
                    danhSach = renderPartialViewToString(ControllerContext,
                        "ChuDe/_DanhSach.cshtml",
                        chuDe.con,
                        new ViewDataDictionary() { 
                            { "Ma", chuDe.ma.Value }, 
                            { "CoChon", coChon }
                        }
                    )
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _FormTao(int maCha)
        {
            KetQua ketQua;
            ChuDeDTO chuDe = null;

            #region Kiểm tra quyền
            var maNguoiDung = Session["NguoiDung"] as int?;
            if (!maNguoiDung.HasValue)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để sử dụng chức năng này."));
            }

            if (maCha != 0)
            {
                ketQua = ChuDeBUS.layTheoMa(maCha);
                if (ketQua.trangThai != 0)
                {
                    return Redirect("/?tb=" + HttpUtility.UrlEncode("Chủ đề không tồn tại."));
                }
                chuDe = ketQua.ketQua as ChuDeDTO;
            }

            if (!BUS.coQuyen("QLNoiDung", "CD", maCha, maNguoiDung))
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần có quyền quản lý nội dung để thực hiện chức năng này."));
            }
            #endregion

            ViewData["MaCha"] = maCha;
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "ChuDe/_Form.cshtml", null, ViewData)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _FormSua(int ma)
        {
            KetQua ketQua;
            ChuDeDTO chuDe = null;

            #region Kiểm tra quyền
            var maNguoiDung = Session["NguoiDung"] as int?;
            if (!maNguoiDung.HasValue)
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để sử dụng chức năng này."));
            }

            if (ma != 0)
            {
                ketQua = ChuDeBUS.layTheoMa(ma);
                if (ketQua.trangThai != 0)
                {
                    return Redirect("/?tb=" + HttpUtility.UrlEncode("Chủ đề không tồn tại."));
                }
                chuDe = ketQua.ketQua as ChuDeDTO;
            }

            if (!BUS.coQuyen("QLNoiDung", "CD", ma, maNguoiDung))
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần có quyền quản lý nội dung để thực hiện chức năng này."));
            }
            #endregion

            return Json(new KetQua(0, renderPartialViewToString(ControllerContext, "ChuDe/_Form.cshtml", chuDe)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                    {
                        trangThai = 4
                    });
            }
            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiTao", Session["NguoiDung"].ToString());

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
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                    {
                        trangThai = 4
                    });
            }
            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiSua", Session["NguoiDung"].ToString());

            var ketQua = ChuDeBUS.capNhatTheoMa(form);
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
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }

            return Json(ChuDeBUS.capNhatCha(ma, maCha, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyXoa(int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4
                });
            }

            return Json(ChuDeBUS.xoaTheoMa(ma, (int)Session["NguoiDung"]));
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