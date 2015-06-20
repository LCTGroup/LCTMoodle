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
            //Kiểm tra phạm vi hợp lệ
            if (Array.IndexOf(new string[] { "HT", "KH" }, phamVi) == -1)
            {
                RedirectToAction("Index", "TrangChu");
            }

            KetQua ketQua;

            //Kiểm tra đối tượng hợp lệ
            switch(phamVi)
            {
                case "KH":
                    ketQua = KhoaHocBUS.layTheoMa(maDoiTuong);
                    if (ketQua.trangThai != 0)
                    {
                        return RedirectToAction("Index", "TrangChu");
                    }

                    ViewData["KhoaHoc"] = ketQua.ketQua;
                    break;
                default:
                    break;
            }

            //Lấy danh sách nhóm người dùng
            ketQua = NhomNguoiDungBUS.layTheoMaDoiTuong(phamVi, maDoiTuong);
            if (ketQua.trangThai == 0)
            {
                ViewData["DanhSachNhom"] = ketQua.ketQua;
            }

            //Lấy danh sách quyền hiển thị mặc định
            switch (phamVi)
            {
                case "HT":
                    ketQua = QuyenBUS.layTheoPhamVi_Cay("HT");
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

            ViewData["PhamVi"] = phamVi;

            //Render view
            switch (phamVi)
            {
                case "KH":
                    return View("~/Views/KhoaHoc/QuanLyQuyen.cshtml");
                default:
                    return View();
            }
        }

        public ActionResult _DanhSachQuyen(string phamVi)
        {
            KetQua ketQua = QuyenBUS.layTheoPhamVi_Cay(phamVi);

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

        public ActionResult _FormNhom(string phamVi, int doiTuong)
        {
            ViewData["PhamVi"] = phamVi;
            ViewData["DoiTuong"] = doiTuong;
            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "Quyen/_Form_Nhom.cshtml", null, ViewData)
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThemNhom(FormCollection form)
        {
            KetQua ketQua = NhomNguoiDungBUS.them(chuyenForm(form));

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
        public ActionResult XuLyXoaNhom(string phamVi, int ma)
        {
            return Json(NhomNguoiDungBUS.xoaTheoMa(phamVi, ma));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatQuyenNhom(string phamVi, int maNhom, int maQuyen, int maDoiTuong, bool them, bool la)
        {
            return Json(NhomNguoiDung_QuyenBUS.themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(phamVi, maNhom, maQuyen, maDoiTuong, them, la));
        }

        public ActionResult XulyLayQuyenNhom(string phamVi, int maNhom, int maDoiTuong)
        {
            KetQua ketQua = NhomNguoiDung_QuyenBUS.layTheoMaNhomNguoiDungVaMaDoiTuong(phamVi, maNhom, maDoiTuong);

            if (ketQua.trangThai == 0)
            {
                var danhSachQuyenNhom = new Dictionary<string, string>();

                foreach (var quyenNhom in ketQua.ketQua as List<NhomNguoiDung_QuyenDTO>)
                {
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
            KetQua ketQua = NhomNguoiDung_NguoiDungBUS.them(phamVi, maNhom, maNguoiDung);

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
            return Json(NhomNguoiDung_NguoiDungBUS.xoaTheoMaNhomNguoiDungVaMaNguoiDung(phamVi, maNhom, maNguoiDung));
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