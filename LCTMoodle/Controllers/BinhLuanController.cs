using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTOLayer;
using BUSLayer;
using DAOLayer;
using Helpers;
using Data;

namespace LCTMoodle.Controllers
{
    public class BinhLuanController : LCTController
    {
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            Form form = chuyenForm(formCollection);
            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", Session["NguoiDung"].ToString());
            }
            KetQua ketQua = BinhLuanBUS.them(form);

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BinhLuan/_Item.cshtml", ketQua.ketQua)
                    });
            }
            else
            {
                return Json(ketQua);
            }
        }

        [HttpPost]
        public ActionResult XuLyXoa(string loaiDoiTuong, int ma)
        {
            return Json(BinhLuanDAO.xoaTheoMa(loaiDoiTuong, ma));
        }

        public ActionResult _Form(string loaiDoiTuong, int ma)
        {
            var ketQua = BinhLuanBUS.layTheoMa(loaiDoiTuong, ma);
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }

            ViewData["LoaiDoiTuong"] = loaiDoiTuong;
            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "BinhLuan/_Form.cshtml", ketQua.ketQua, ViewData)
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            var ketQua = BinhLuanBUS.capNhatTheoMa(chuyenForm(formCollection));
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            return Json(new KetQua() 
            { 
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "BinhLuan/_Item.cshtml", ketQua.ketQua)
            });
        }
	}
}