using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;

namespace LCTMoodle.Controllers
{
    public class QuyenController : LCTController
    {
        public ActionResult QuanLy(string phamVi = "HT", int maDoiTuong = 0)
        {
            #region Kiểm tra tham số
            switch(phamVi)
            {
                case "HT":
                case "CD":
                    break;
                case "KH":
                    if (maDoiTuong == 0)
                    {
                        return Redirect("/?tb=" + HttpUtility.UrlEncode("Trang không hợp lệ"));
                    }
                    break;
                default:
                    return Redirect("/?tb=" + HttpUtility.UrlEncode("Trang không hợp lệ"));
            }
            #endregion

            KetQua ketQua;
            #region Kiểm tra quyền
            if (!BUS.coQuyen("QLQuyen", phamVi, maDoiTuong))
            {
                return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần có quyền quản lý để thực hiện chức năng này."));
            }
            #endregion

            #region Kiểm tra đối tượng
            switch (phamVi)
            {
                case "KH":
                    ketQua = KhoaHocBUS.layTheoMa(maDoiTuong);
                    if (ketQua.trangThai != 0)
                    {
                        return Redirect("/?tb=" + HttpUtility.UrlEncode("Khóa học không tồn tại."));
                    }

                    ViewData["KhoaHoc"] = ketQua.ketQua;
                    break;
                case "CD":
                    if (maDoiTuong != 0)
                    {
                        ketQua = ChuDeBUS.layTheoMa(maDoiTuong);
                        if (ketQua.trangThai != 0)
                        {
                            return Redirect("/?tb=" + HttpUtility.UrlEncode("Chủ đề không tồn tại."));
                        }

                        ViewData["ChuDe"] = ketQua.ketQua;
                    }
                    break;
                default:
                    break;
            } 
            #endregion

            #region Lấy nhóm người dùng
            ketQua = NhomNguoiDungBUS.layTheoMaDoiTuong(phamVi, maDoiTuong);
            if (ketQua.trangThai == 1)
            {
                ketQua = NhomNguoiDungBUS.themNhomMacDinh(phamVi, maDoiTuong);
            }
            if (ketQua.trangThai == 0)
            {
                ViewData["DanhSachNhom"] = ketQua.ketQua;
            } 
            #endregion

            #region Lấy danh sách quyền hiển thị mặc định
            switch (phamVi)
            {
                case "HT":
                    ketQua = QuyenBUS.layTheoPhamVi_Cay("HT");
                    break;
                case "CD":
                    ketQua = QuyenBUS.layTheoPhamVi_Cay("CD");
                    break;
                case "KH":
                    ketQua = QuyenBUS.layTheoPhamVi_Cay("KH");
                    break;
                default:
                    ketQua.trangThai = 1;
                    break;
            }
            if (ketQua.trangThai == 0)
            {
                ViewData["DanhSachQuyen"] = ketQua.ketQua;
            } 
            #endregion

            #region Chọn view
            switch (phamVi)
            {
                case "KH":
                    return View("~/Views/Quyen/QuyenKhoaHoc.cshtml");
                case "CD":
                    return View("~/Views/Quyen/QuyenChuDe.cshtml");
                default:
                    return View("~/Views/Quyen/QuyenHeThong.cshtml");
            } 
            #endregion
        }

        public ActionResult _DanhSachQuyen(string phamVi, bool laQuyenChung)
        {
            KetQua ketQua = QuyenBUS.layTheoPhamVi_Cay(phamVi, laQuyenChung);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new KetQua() 
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "Quyen/_DanhSach_Quyen.cshtml", ketQua.ketQua)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _FormNhom(string phamVi, int maDoiTuong)
        {
            ViewData["PhamVi"] = phamVi;
            ViewData["MaDoiTuong"] = maDoiTuong;
            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "Quyen/_Form_Nhom.cshtml", null, ViewData)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _FormNhom_Sua(string phamVi, int ma)
        {
            #region Kiểm tra điều kiện
            //Lấy người dùng
            var maNguoiDung = Session["NguoiDung"] as int?;
            if (!maNguoiDung.HasValue)
            {
                return Json(new KetQua(4), JsonRequestBehavior.AllowGet);
            }

            //Lấy nhóm
            var ketQua = NhomNguoiDungBUS.layTheoMa(phamVi, ma);
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua(1, "Nhóm người dùng không tồn tại"), JsonRequestBehavior.AllowGet);
            }
            var nhom = ketQua.ketQua as NhomNguoiDungDTO;

            //Kiểm tra quyền
            if (!BUS.coQuyen("QLQuyen", phamVi, phamVi == "HT" ? 0 : nhom.doiTuong.ma.Value, maNguoiDung))
            {
                return Json(new KetQua(3, "Bạn không có quyền sửa nhóm"));
            }
            #endregion

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "Quyen/_Form_Nhom.cshtml", nhom)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThemNhom(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiTao", Session["NguoiDung"].ToString());

            KetQua ketQua = NhomNguoiDungBUS.them(form);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "Quyen/_Item_Nhom.cshtml", ketQua.ketQua)
                });
        }

        [HttpPost]
        public ActionResult XuLyCapNhatNhom(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }
            var form = chuyenForm(formCollection);
            form.Add("MaNguoiSua", Session["NguoiDung"].ToString());

            return Json(NhomNguoiDungBUS.capNhat(form));
        }

        [HttpPost]
        public ActionResult XuLyXoaNhom(string phamVi, int ma)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(NhomNguoiDungBUS.xoaTheoMa(phamVi, ma, (int)Session["NguoiDung"]));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatQuyenNhom(string phamVi, int maNhom, int maQuyen, int maDoiTuong, bool la, bool them)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(NhomNguoiDung_QuyenBUS.themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(phamVi, maNhom, maQuyen, maDoiTuong, la, them, (int)Session["NguoiDung"]));
        }

        public ActionResult XulyLayQuyenNhom(string phamVi, int maNhom)
        {
            KetQua ketQua = NhomNguoiDung_QuyenBUS.layTheoMaNhomNguoiDung(phamVi, maNhom);

            if (ketQua.trangThai == 0)
            {
                var danhSachQuyenNhom = new Dictionary<string, string>();

                foreach (var quyenNhom in ketQua.ketQua as List<NhomNguoiDung_QuyenDTO>)
                {
                    if (quyenNhom.quyen == null)
                    {
                        continue;
                    }

                    string key = quyenNhom.quyen.phamVi + quyenNhom.doiTuong.ma.Value.ToString();

                    if (!danhSachQuyenNhom.ContainsKey(key))
                    {
                        danhSachQuyenNhom.Add(key, "|");
                    }

                    danhSachQuyenNhom[key] += quyenNhom.quyen.ma.Value.ToString() + "|";
                }

                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = danhSachQuyenNhom
                    }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _DanhSachNguoiDung_Tim(string tuKhoa, string phamVi, int maNhom, int maDoiTuong = 0)
        {
            KetQua ketQua = NhomNguoiDung_NguoiDungBUS.layNguoiDung_TimKiem(tuKhoa, phamVi, maNhom, maDoiTuong);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "Quyen/_DanhSach_NguoiDung_Tim.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThemNguoiDung(string phamVi, int maNhom, int maNguoiDung)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            KetQua ketQua = NhomNguoiDung_NguoiDungBUS.them(phamVi, maNhom, maNguoiDung, (int)Session["NguoiDung"]);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            ketQua = NguoiDungBUS.layTheoMa(maNguoiDung);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "Quyen/_Item_NguoiDung.cshtml", ketQua.ketQua)
                });
        }

        [HttpPost]
        public ActionResult XuLyXoaNguoiDung(string phamVi, int maNhom, int maNguoiDung)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(NhomNguoiDung_NguoiDungBUS.xoaTheoMaNhomNguoiDungVaMaNguoiDung(phamVi, maNhom, maNguoiDung, (int)Session["NguoiDung"]));
        }

        public ActionResult _DanhSachNguoiDung(string phamVi, int maNhom)
        {
            KetQua ketQua = NguoiDungBUS.layTheoMaNhomNguoiDung(phamVi, maNhom);

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "Quyen/_DanhSach_NguoiDung.cshtml", ketQua.ketQua)
                }, JsonRequestBehavior.AllowGet);
        }
	}
}