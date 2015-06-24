using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using BUSLayer;
using DAOLayer;
using DTOLayer;

namespace LCTMoodle.Controllers
{
    public class ChuongTrinhController : LCTController
    {
        public ActionResult Index(int ma) //Để tạm, sau này sửa route, đổi thành mã khóa học
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);
            
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            ViewData["KhoaHoc"] = ketQua.ketQua as KhoaHocDTO;

            ketQua = ChuongTrinhBUS.layTheoMaKhoaHoc(ma);

            return View(model: ketQua.trangThai == 0 ? ketQua.ketQua : null);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            KetQua ketQua = ChuongTrinhBUS.them(chuyenDuLieuForm(formCollection));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "ChuongTrinh/_Item.cshtml", ketQua.ketQua)
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
            return Json(ChuongTrinhBUS.xoaTheoMa(ma));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatThuTu(int thuTuCu, int thuTuMoi, int maKhoaHoc)
        {
            return Json(ChuongTrinhBUS.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc));
        }
	}
}