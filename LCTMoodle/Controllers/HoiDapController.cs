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
        //
        // GET: /HoiDap/
        public ActionResult Index()
        {
            return View((CauHoiBUS.layDanhSach(null, new LienKet() 
            { 
                "NguoiTao",
                "ChuDe"
            }).ketQua) as List<CauHoiDTO>);
        }

        #region Câu Hỏi

        [HttpPost]
        public ActionResult _Form_CauHoi(int? ma = 0)
        {
            KetQua ketQua = CauHoiBUS.layTheoMa(ma, new LienKet()
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
            });

            if (ketQua.trangThai != 0)
            {
                return Json(ketQua);
            }
            else
            {
                return Json(new KetQua()
                {
                    trangThai = 0,

                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Form_CauHoi.cshtml", ketQua.ketQua)
                });
            }
        }
        
        public ActionResult ThemCauHoi()
        {            
            return View();
        }

        public ActionResult XemCauHoi(int ma)
        {
            int VoteDiem = 0;
            KetQua ketQua = new KetQua();

            if (Session["NguoiDung"] == null)
            {
                VoteDiem = 0;
            }
            else
            {
                ketQua = CauHoi_DiemBUS.layDiemVoteNguoiDung((int?)Session["NguoiDung"]);
                if (ketQua.trangThai != 0)
                {
                    VoteDiem = 0;
                }
                else
                {
                    bool ketQuaVote = (bool)ketQua.ketQua;
                    VoteDiem = ketQuaVote == true ?  1 : 2;
                }
            }

            //0: chưa vote, 1:vote cộng, 2:vote trừ
            ViewData["VoteDiem"] = VoteDiem;

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

            ViewData["MaCauHoi"] = ma;
            ketQua = CauHoiBUS.layTheoMa(ma, lienKetMacDinh);
            if (ketQua.trangThai != 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }

            return View(ketQua.ketQua);
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
                "ChuDe"
            });
            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_CauHoi.cshtml", ketQua.ketQua)
                });
            }
            else
            {
                return Json(ketQua);
            }
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
            else
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Form_TraLoi.cshtml", ketQua.ketQua)
                });
            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XuLyThemTraLoi(FormCollection form)
        {
            //Kiểm tra đăng nhập
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                {
                    trangThai = 4,
                    ketQua = "Bạn chưa đăng nhập"
                });
            }

            KetQua ketQua = TraLoiBUS.them(chuyenForm(form));

            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_TraLoi.cshtml", ketQua.ketQua)
                });
            }
            return Json(ketQua);
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
            KetQua ketQua = TraLoiBUS.capNhat(chuyenForm(form));
            if (ketQua.trangThai == 0)
            {
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_TraLoi.cshtml", ketQua.ketQua)
                });
            }
            else
            {
                return Json(ketQua);
            }
        }

        [HttpPost]
        public ActionResult XuLyDuyetTraLoi(int maTraLoi, bool trangThaiDuyet)
        {
            return Json(TraLoiBUS.capNhatDuyetTheoMa(maTraLoi, trangThaiDuyet));
        }
        #endregion

        #region Câu hỏi - Điểm
        
        public ActionResult XuLyDiemCauHoi(int? maCauHoi, bool diem)
        {
            if (Session["NguoiDung"] == null)
            {
                return Json(new KetQua()
                    {
                        trangThai = 4,
                        ketQua = "Chưa đăng nhập"
                    }, JsonRequestBehavior.AllowGet);
            }
            int? maNguoiTao = (int?)Session["NguoiDung"];

            KetQua ketQua = CauHoi_DiemBUS.layDiemVoteNguoiDung(maNguoiTao);
            if (ketQua.trangThai != 0)
            {
                CauHoi_DiemBUS.them(maCauHoi, maNguoiTao, diem);
            }
            else
            {
                CauHoi_DiemBUS.xoaTheoMa(maCauHoi, maNguoiTao);
            }

            ketQua = CauHoiBUS.capNhatDiem(maCauHoi, diem);
            
            return Json(ketQua, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Trả lời - Điểm

        #endregion
    }
}