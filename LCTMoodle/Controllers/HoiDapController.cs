using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Data;
using Helpers;

namespace LCTMoodle.Controllers
{
    public class HoiDapController : LCTController
    {
        #region Câu Hỏi

        public ActionResult Index()
        {
            return View((CauHoiBUS.layDanhSach(null, new LienKet() 
            { 
                "NguoiTao",
                "ChuDe"
            }).ketQua) as List<CauHoiDTO>);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _Form_CauHoi(int? ma = 0)
        {
            KetQua ketQua = CauHoiBUS.layTheoMa(ma, new LienKet() { 
                "ChuDe"
            });
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }
            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;
            if (cauHoi.nguoiTao.ma.Value != (int)Session["NguoiDung"])
            {
                return Json(new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa câu hỏi này"
                });
            }

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "/HoiDap/_Form_CauHoi.cshtml", cauHoi)
            });
        }

        public ActionResult ThemCauHoi()
        {
            return View();
        }

        public ActionResult XemCauHoi(int ma)
        {
            LienKet lienKetMacDinh = new LienKet()
            {
                "NguoiTao",                
                {
                    "TraLoi",
                    new LienKet() 
                    {
                        "NguoiTao"
                    }
                },
                "ChuDe"
            };

            KetQua ketQua = CauHoiBUS.layTheoMa(ma, lienKetMacDinh);
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "HoiDap");
            }
            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;
            ViewData["MaCauHoi"] = ma;
            ViewData["TrangThaiVote"] = CauHoi_DiemBUS.trangThaiVoteCuaNguoiDungTrongCauHoi(ma, (int?)Session["NguoiDung"]);

            Dictionary<int, int> trangThaiVoteTraLoi = new Dictionary<int, int>();
            foreach(var traLoi in cauHoi.danhSachTraLoi)
            {
                trangThaiVoteTraLoi.Add(traLoi.ma.Value, TraLoi_DiemBUS.trangThaiVoteCuaNguoiDungTrongTraLoi(traLoi.ma, (int?)Session["NguoiDung"]));
            }
            ViewData["DSTrangThaiVoteTraLoi"] = trangThaiVoteTraLoi;

            return View(cauHoi);
        }

        public ActionResult XuLyXoaCauHoi(int ma)
        {
            return Json(CauHoiBUS.xoaTheoMa(ma));
        }

        public ActionResult _DanhSach_Tim(string tuKhoa = "", int maChuDe = 0)
        {
            KetQua ketQua;
            if (maChuDe == 0)
            {
                ketQua = CauHoiBUS.lay_TimKiem(tuKhoa, new LienKet() { 
                    "NguoiTao",
                    "HinhDaiDien"
                });
            }
            else
            {
                ketQua = CauHoiBUS.layTheoMaChuDe_TimKiem(maChuDe, tuKhoa, new LienKet() { 
                    "NguoiTao",
                    "HinhDaiDien"
                });
            }

            if (ketQua.trangThai == 0)
            {
                ketQua.ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_DanhSachCauHoi.cshtml", ketQua.ketQua);
            }

            return Json(ketQua, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyCapNhatCauHoi(FormCollection form)
        {
            KetQua ketQua = CauHoiBUS.capNhat(chuyenForm(form), new LienKet() 
            {
                "NguoiTao",
                {
                    "TraLoi",
                    new LienKet() {
                        "NguoiTao"
                    }
                },
                "ChuDe"
            });
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua.ketQua);
            }
            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;

            ViewData["TrangThaiVote"] = CauHoi_DiemBUS.trangThaiVoteCuaNguoiDungTrongCauHoi(cauHoi.ma.Value, (int?)Session["NguoiDung"]);

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_CauHoi.cshtml", ketQua.ketQua, ViewData)
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyThemCauHoi(FormCollection form)
        {
            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", Session["NguoiDung"].ToString());
            }
            return Json(CauHoiBUS.them(chuyenForm(form)));
        }

        #endregion

        #region Trả Lời

        [HttpPost]
        public ActionResult _Form_TraLoi(int ma = 0)
        {
            KetQua ketQua = TraLoiBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }
            TraLoiDTO traLoi = ketQua.ketQua as TraLoiDTO;
            if (traLoi.nguoiTao.ma.Value != (int)Session["NguoiDung"])
            {
                return Json(new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa trả lời này"
                });
            }

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Form_TraLoi.cshtml", traLoi)
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyThemTraLoi(FormCollection form)
        {
            //Kiểm tra đăng nhập
            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", Session["NguoiDung"].ToString());
            }
            KetQua ketQua = TraLoiBUS.them(chuyenForm(form));
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_TraLoi.cshtml", ketQua.ketQua)
            });
        }

        [HttpPost]
        public ActionResult XuLyXoaTraLoi(int ma)
        {
            return Json(TraLoiBUS.xoaTheoMa(ma));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyCapNhatTraLoi(FormCollection form)
        {
            KetQua ketQua = TraLoiBUS.capNhat(chuyenForm(form), new LienKet() { 
                "NguoiTao",
                "CauHoi"
            });
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }
            TraLoiDTO traLoi = ketQua.ketQua as TraLoiDTO;

            ViewData["TrangThaiVoteTraLoi"] = TraLoi_DiemBUS.trangThaiVoteCuaNguoiDungTrongTraLoi(traLoi.ma.Value, (int?)Session["NguoiDung"]);

            return Json(new KetQua()
            {
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_TraLoi.cshtml", ketQua.ketQua, ViewData)
            });
        }

        [HttpPost]
        public ActionResult XuLyDuyetTraLoi(int maTraLoi, bool trangThaiDuyet)
        {
            return Json(TraLoiBUS.capNhatDuyetTheoMa(maTraLoi, trangThaiDuyet));
        }
        #endregion

        #region Câu hỏi - Điểm

        [HttpPost]
        public ActionResult XuLyChoDiemCauHoi(int maCauHoi, bool diem)
        {
            return Json(CauHoi_DiemBUS.them(maCauHoi, Session["NguoiDung"] as int?, diem));
        }

        [HttpPost]
        public ActionResult XuLyBoChoDiemCauHoi(int maCauHoi)
        {
            return Json(CauHoi_DiemBUS.xoa(maCauHoi, Session["NguoiDung"] as int?));
        }

        #endregion

        #region Trả lời - Điểm

        [HttpPost]
        public ActionResult XuLyChoDiemTraLoi(int maTraLoi, bool diem)
        {
            return Json(TraLoi_DiemBUS.them(maTraLoi, Session["NguoiDung"] as int?, diem));
        }

        [HttpPost]
        public ActionResult XuLyBoChoDiemTraLoi(int maTraLoi)
        {
            return Json(TraLoi_DiemBUS.xoa(maTraLoi, Session["NguoiDung"] as int?));
        }

        #endregion
    }
}