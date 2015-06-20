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
    public class GiaoTrinhController : LCTController
    {
        public ActionResult Index(int ma) //Để tạm, sau này sửa route, đổi thành mã khóa học
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma);
            
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            ViewData["KhoaHoc"] = ketQua.ketQua as KhoaHocDTO;

            ketQua = GiaoTrinhBUS.layTheoMaKhoaHoc(ma);

            return View(model: ketQua.trangThai == 0 ? ketQua.ketQua : null);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            KetQua ketQua = GiaoTrinhBUS.them(chuyenDuLieuForm(formCollection));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "GiaoTrinh/_Item_Tao.cshtml", ketQua.ketQua)
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
            return Json(GiaoTrinhBUS.xoaTheoMa(ma));
        }

        [HttpPost]
        public ActionResult XuLyCapNhatThuTu(int thuTuCu, int thuTuMoi, int maKhoaHoc)
        {
            return Json(GiaoTrinhBUS.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc));
        }
	}
}