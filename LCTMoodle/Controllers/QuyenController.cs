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
        public ActionResult QuanLy()
        {
            KetQua ketQua = NhomNguoiDungBUS.layTheoMaDoiTuong("HT", 0);

            if (ketQua.trangThai == 0)
            {
                ViewData["DanhSachNhom"] = ketQua.ketQua;
            }

            ketQua = QuyenBUS.layTheoPhamVi("HT");

            if (ketQua.trangThai == 0)
            {
                ViewData["DanhSachQuyen"] = ketQua.ketQua;
            }

            return View();
        }

        public ActionResult _DanhSachQuyen(string phamVi)
        {
            KetQua ketQua = QuyenBUS.layTheoPhamVi(phamVi);

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

        public ActionResult _FormNhom(string phamVi)
        {
            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "Quyen/_Form_Nhom.cshtml", phamVi)
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
        public ActionResult XuLyCapNhatQuyenNhom(string phamVi, int maNhom, int maQuyen, int maDoiTuong, bool them)
        {
            return Json(NhomNguoiDung_QuyenBUS.themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(phamVi, maNhom, maQuyen, maDoiTuong, them));
        }

        public ActionResult XulyLayQuyenNhom(string phamVi, int maNhom)
        {
            KetQua ketQua = NhomNguoiDung_QuyenBUS.layTheoMaNhomNguoiDung(phamVi, maNhom);

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

        public ActionResult _DanhSachNguoiDung_Tim(string tuKhoa, string phamVi, int maNhom)
        {
            KetQua ketQua = NhomNguoiDung_NguoiDungBUS.layTheoTuKhoa(tuKhoa, phamVi, maNhom);

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