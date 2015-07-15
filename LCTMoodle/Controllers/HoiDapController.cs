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

            #region Hiển thị

            public ActionResult Index()
            {
                string order = "ThoiDiemCapNhat DESC";
                string where = "DuyetHienThi = 1";
                //KetQua ketQua = CauHoiBUS.timKiemPhanTrang(1, Data.GiaTri.soLuongCauHoiMoiTrang, where, order, new LienKet() { "NguoiTao", "HinhDaiDien", "TraLoi" });
                var ketQua = CauHoiBUS.layDanhSach(null, new LienKet() { "NguoiTao", "HinhDaiDien", "TraLoi" }, "MoiNhat");
                if (ketQua.trangThai != 0)
                {
                    return Redirect("/?tb=" + HttpUtility.UrlEncode("Lỗi."));
                }
                return View(ketQua.ketQua);
            }

            public ActionResult QuanLyCauHoi()
            {

                #region Kiểm tra điều kiện

                int? maNguoiDung = Session["NguoiDung"] as int?;

                if (!maNguoiDung.HasValue)
                {
                    return Redirect("/HoiDap");
                }
                else if (!QuyenBUS.coQuyen("DuyetCauHoi", "HD", 0, maNguoiDung))
                {
                    return Redirect("/HoiDap");
                }

                #endregion

                var ketQua = CauHoiDAO.layCauHoi_ChuaDuyet();

                List<CauHoiDTO> dsCauHoi = ketQua.ketQua as List<CauHoiDTO>;

                return View(dsCauHoi);
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

                if (cauHoi.DuyetHienThi == false)
                {
                    return Redirect("/HoiDap");
                }

                ViewData["TrangThaiVote"] = CauHoi_DiemBUS.trangThaiVoteCuaNguoiDungTrongCauHoi(ma, (int?)Session["NguoiDung"]);

                #region Kiểm tra trạng thái Vote trả lời

                if (cauHoi.danhSachTraLoi != null)
                {
                    Dictionary<int, int> trangThaiVoteTraLoi = new Dictionary<int, int>();
                    foreach (var traLoi in cauHoi.danhSachTraLoi)
                    {
                        trangThaiVoteTraLoi.Add(traLoi.ma.Value, TraLoi_DiemBUS.trangThaiVoteCuaNguoiDungTrongTraLoi(traLoi.ma, (int?)Session["NguoiDung"]));
                    }
                    ViewData["DSTrangThaiVoteTraLoi"] = trangThaiVoteTraLoi;
                }

                #endregion

                return View(cauHoi);
            }

            public ActionResult ThemCauHoi()
            {
                #region Kiểm tra điều kiện

                if (Session["NguoiDung"] == null)
                {
                    return Redirect("/?tb=" + HttpUtility.UrlEncode("Bạn cần đăng nhập để thực hiện chức năng này"));
                }

                #endregion

                return View();
            }

            [HttpPost]
            public ActionResult XemMoPhongCauHoi(int ma)
            {
                var ketQua = CauHoiBUS.layTheoMa(ma, new LienKet() { "NguoiTao", "ChuDe" });
                if (ketQua.trangThai != 0)
                {
                    return Json(ketQua);
                }

                ViewData["CauHoi"] = ketQua.ketQua;

                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "/HoiDap/_Item_QuanLyHoiDap.cshtml", null, new ViewDataDictionary() { { "CauHoi", ViewData["CauHoi"] } })
                });
            }

            #endregion

            #region Hiển thị _PartialView

            [HttpPost]
            [ValidateInput(false)]
            public ActionResult _Form_CauHoi(int ma = 0)
            {
                KetQua ketQua = CauHoiBUS.layTheoMa(ma, new LienKet() { 
                "ChuDe"
            });
                if (ketQua.trangThai != 0)
                {
                    return Json(ketQua);
                }
                CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;
                if (cauHoi.nguoiTao.ma.Value != (int)Session["NguoiDung"] && !BUS.coQuyen("SuaCauHoi", "HD", cauHoi.ma.Value, (int?)Session["NguoiDung"]))
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

            public ActionResult _DanhSach_Tim(string tuKhoa = "", int maChuDe = 0, int trang = 1, string cachHienThi = "")
            {
                string order;
                switch (cachHienThi)
                {
                    case "MoiNhat":
                        order = "ThoiDiemCapNhat DESC";
                        break;
                    case "DiemCaoNhat":
                        order = "Diem DESC";
                        break;
                    case "TraLoiNhieuNhat":
                        order = "SoLuongTraLoi DESC";
                        break;
                    default:
                        order = null;
                        break;
                }

                //Sửa số dòng mỗi trang nhớ sửa ở view
                var ketQua = CauHoiBUS.timKiemPhanTrang(trang, Data.GiaTri.soLuongCauHoiMoiTrang,
                    "TieuDe LIKE '%" + tuKhoa + "%'" +
                    (maChuDe != 0 ?
                    "AND MaChuDe = " + maChuDe :
                    null),
                    order,
                    new LienKet() { "NguoiTao", "ChuDe" });

                if (ketQua.trangThai == 0)
                {
                    ketQua.ketQua = new
                    {
                        danhSach = renderPartialViewToString(ControllerContext, "HoiDap/_DanhSachCauHoi.cshtml", ketQua.ketQua),
                        phanTrang = renderPartialViewToString(ControllerContext, "LCT/_PhanTrang.cshtml", null, new ViewDataDictionary()
                        {
                            { "TongSoLuong", LCTHelper.layGiaTri<int>((ketQua.ketQua as List<CauHoiDTO>)[0].duLieuThem, "TongSoDong", 0) },
                            { "SoLuongMoiTrang", Data.GiaTri.soLuongCauHoiMoiTrang }
                        })
                    };
                }

                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }

            #endregion

            #region Xử lý

            [HttpPost]
            public ActionResult XuLyXoaCauHoi(int ma)
            {
                if (Session["NguoiDung"] == null)
                {
                    return Json(new KetQua(4, "Chưa đăng nhập"));
                }
                return Json(CauHoiBUS.xoaTheoMa(ma, (int?)Session["NguoiDung"]));
            }

            [HttpPost]
            [ValidateInput(false)]
            public ActionResult XuLyCapNhatCauHoi(FormCollection formCollection)
            {
                if (Session["NguoiDung"] == null)
                {
                    return Json(new KetQua(4, "Bạn chưa đăng nhập"));
                }

                Form form = chuyenForm(formCollection);
                form.Add("MaNguoiSua", Session["NguoiDung"].ToString());

                KetQua ketQua = CauHoiBUS.capNhat(form, new LienKet() 
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
                    return Json(ketQua);
                }
                CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;

                ViewData["TrangThaiVote"] = CauHoi_DiemBUS.trangThaiVoteCuaNguoiDungTrongCauHoi(cauHoi.ma.Value, (int)Session["NguoiDung"]);

                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Item_CauHoi.cshtml", ketQua.ketQua, ViewData)
                });
            }

            [HttpPost]
            public ActionResult XuLyDuyetHienThiCauHoi(int maCauHoi, bool trangThai)
            {
                int? nguoiDuyet = Session["NguoiDung"] as int?;

                return Json(CauHoiBUS.duyetHienThiCauHoi(maCauHoi, trangThai, nguoiDuyet));
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

        #endregion

        #region Trả Lời

            #region Hiển thị

            public ActionResult QuanLyTraLoi()
            {

                #region Kiểm tra điều kiện

                int? maNguoiDung = Session["NguoiDung"] as int?;

                if (!maNguoiDung.HasValue)
                {
                    return Redirect("/HoiDap");
                }
                if (!QuyenBUS.coQuyen("DuyetTraLoi", "HD", 0, maNguoiDung))
                {
                    return Redirect("/HoiDap");
                }

                #endregion

                var ketQua = TraLoiDAO.layTraLoiChuaDuyet();

                List<TraLoiDTO> dsTraLoi = ketQua.ketQua as List<TraLoiDTO>;

                return View(dsTraLoi);
            }

            #endregion

            #region Hiển thị _PartialView

            [HttpPost]
            public ActionResult _Form_TraLoi(int ma = 0)
            {
                #region Kiểm tra điều kiện

                if (Session["NguoiDung"] == null)
                {
                    return Json(new KetQua(4));
                }

                KetQua ketQua = TraLoiBUS.layTheoMa(ma);
                if (ketQua.trangThai != 0)
                {
                    return Json(new KetQua(1, "Trả lời không tồn tại"));
                }
                TraLoiDTO traLoi = ketQua.ketQua as TraLoiDTO;

                if (traLoi.nguoiTao.ma != (int?)Session["NguoiDung"] && !BUS.coQuyen("SuaTraLoi", "HD", 0, (int?)Session["NguoiDung"]))
                {
                    return Json(new KetQua()
                    {
                        trangThai = 3,
                        ketQua = "Bạn không có quyền sửa trả lời này"
                    });
                }

                #endregion

                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "HoiDap/_Form_TraLoi.cshtml", traLoi)
                });
            }

            #endregion

            #region Xử lý

            public ActionResult XuLyDuyetHienThiTraLoi(int? maTraLoi, bool trangThai)
            {
                return Json(TraLoiBUS.duyetHienThiTraLoi(maTraLoi, trangThai));
            }

            public ActionResult XemChiTietTraLoi(int maTraLoi = 0)
            {
                var ketQua = TraLoiBUS.layTheoMa(maTraLoi, new LienKet() { 
                { "CauHoi", new LienKet() { "NguoiTao", "ChuDe" } },
                "NguoiTao"
            });
                if (ketQua.trangThai == 0)
                {
                    ViewData["TraLoi"] = ketQua.ketQua;
                }
                else
                {
                    return Json(ketQua);
                }
                return Json(new KetQua()
                {
                    trangThai = 0,
                    ketQua = renderPartialViewToString(ControllerContext, "/HoiDap/_Item_QuanLyHoiDap.cshtml", null, new ViewDataDictionary() { { "TraLoi", ViewData["TraLoi"] } })
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
                #region Kiểm tra điều kiện

                if (Session["NguoiDung"] == null)
                {
                    return Json(new KetQua(4, "Bạn chưa đăng nhập"));
                }

                #endregion

                return Json(TraLoiBUS.xoaTheoMa(ma, (int?)Session["NguoiDung"]));
            }

            [HttpPost]
            [ValidateInput(false)]
            public ActionResult XuLyCapNhatTraLoi(FormCollection form)
            {

                #region Kiểm tra điều kiện

                if (Session["NguoiDung"] != null)
                {
                    form.Add("MaNguoiSua", Session["NguoiDung"].ToString());
                }
                else
                {
                    return Json(new KetQua(4, "Bạn chưa đăng nhập"));
                }

                #endregion

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