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
    public class BinhLuanBaiVietDienDanController : LCTController
    {
        public ActionResult XuLyThem(FormCollection formCollection)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            Form form = chuyenForm(formCollection);
            form.Add("MaNguoiTao", Session["NguoiDung"].ToString());

            KetQua ketQua = BinhLuanBaiVietDienDanBUS.them(form);

            ViewData["CoQuyenQLNoiDung"] = true;
            ViewData["CoQuyenQLDiem"] = false;
            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                    {
                        trangThai = 0,
                        ketQua = renderPartialViewToString(ControllerContext, "BinhLuanBaiVietDienDan/_Item.cshtml", ketQua.ketQua)
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
            return Json(BinhLuanBaiVietDienDanDAO.xoaTheoMa(ma));
        }

        public ActionResult _Form(int ma)
        {
            var ketQua = BinhLuanBaiVietDienDanBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }

            return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "BinhLuanBaiVietDienDan/_Form.cshtml", ketQua.ketQua, ViewData)
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyCapNhat(FormCollection formCollection)
        {
            var ketQua = BinhLuanBaiVietDienDanBUS.capNhatTheoMa(chuyenForm(formCollection));
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            ViewData["CoQuyenQLNoiDung"] = true;
            ViewData["CoQuyenQLDiem"] = false;
            return Json(new KetQua() 
            { 
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "BinhLuanBaiVietDienDan/_Item.cshtml", ketQua.ketQua, ViewData)
            });
        }

        public ActionResult _Khung(int maBaiVietDienDan)
        {
            #region Kiểm tra điều kiện
            //Lấy bài viết diễn đàn
            var ketQua = BaiVietDienDanBUS.layTheoMa(maBaiVietDienDan);
            if (ketQua.trangThai != 0)
            {
                return Json(new KetQua(3, "Bài viết không tồn tại"), JsonRequestBehavior.AllowGet);
            }
            var baiViet = ketQua.ketQua as BaiVietDienDanDTO;
            #endregion

            //Lấy danh sách bình luận
            ketQua = BinhLuanBaiVietDienDanBUS.layTheoMaBaiVietDienDan(maBaiVietDienDan);
            var dsBinhLuan = ketQua.trangThai == 0 ?
                ketQua.ketQua as List<BinhLuanBaiVietDienDanDTO> :
                null;

            ViewData["MaBaiVietDienDan"] = maBaiVietDienDan;
            ViewData["MaKhoaHoc"] = baiViet.khoaHoc.ma.Value;

            return Json(
                new KetQua(renderPartialViewToString(ControllerContext, "BinhLuanBaiVietDienDan/_Khung.cshtml", dsBinhLuan, ViewData)), 
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyChoDiem(int ma, int diem)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua(4));
            }

            return Json(BinhLuanBaiVietDienDanBUS.capNhatDiem(ma, diem, (int)Session["NguoiDung"]));
        }
	}
}