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
                return RedirectToAction("Index", "TrangChu");
            }

            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;
            int maNguoiDung = cauHoi.nguoiTao.ma.Value;
            int voteDiem = 0;

            ketQua = CauHoi_DiemDAO.layTheoMaNguoiTao_Diem(maNguoiDung);
            if (ketQua.trangThai == 0)
            {
                bool tam;
                tam = (bool)ketQua.ketQua;
                voteDiem = tam == true ? 1 : -1;
            }

            ViewData["VoteDiem"] = voteDiem;
            ViewData["MaCauHoi"] = cauHoi.ma.Value;

            ketQua = TraLoiBUS.layTheoMaCauHoi(cauHoi.ma.Value);
            if (ketQua.trangThai == 0)
            {
                cauHoi.danhSachTraLoi = ketQua.ketQua as List<TraLoiDTO>;
            }
            else
            {
                cauHoi.danhSachTraLoi = null;
            }
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
                "ChuDe"
            });
            if (ketQua.trangThai != 0)
            {
                return Json(ketQua.ketQua);
            }

            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;
            int maNguoiDung = cauHoi.nguoiTao.ma.Value;
            int voteDiem = 0;

            ketQua = CauHoi_DiemDAO.layTheoMaNguoiTao_Diem(maNguoiDung);
            if (ketQua.trangThai == 0)
            {
                bool tam;
                tam = (bool)ketQua.ketQua;
                voteDiem = tam == true ? 1 : -1;
            }

            ViewData["VoteDiem"] = voteDiem;
            ViewData["MaCauHoi"] = cauHoi.ma.Value;

            ketQua = TraLoiBUS.layTheoMaCauHoi(cauHoi.ma.Value);
            if (ketQua.trangThai == 0)
            {
                cauHoi.danhSachTraLoi = ketQua.ketQua as List<TraLoiDTO>;
            }
            else
            {
                cauHoi.danhSachTraLoi = null;
            }

            return Json(new KetQua() { 
                trangThai = 0,
                ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_CauHoi.cshtml", cauHoi, ViewData)
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

        #endregion
    }
}