using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class BaiTapNopBUS : BUS
    {
        public static KetQua kiemTra(BaiTapNopDTO baiNop)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (baiNop.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (baiNop.baiVietBaiTap == null)
            {
                loi.Add("Bài tập được nộp không được bỏ trống");
            }
            else
            {
                int cachNop = baiNop.baiVietBaiTap.cachNop.HasValue ? baiNop.baiVietBaiTap.cachNop.Value : 0;
                if (cachNop == 0)
                {
                    if (baiNop.tapTin == null && string.IsNullOrWhiteSpace(baiNop.duongDan))
                    {
                        loi.Add("Nội dung không hợp lệ");
                    }
                }
                else if (cachNop == 1)
                {
                    if (baiNop.tapTin == null)
                    {
                        loi.Add("Nội dung không hợp lệ");
                    }
                }
                else if (cachNop == 2)
                {
                    if (string.IsNullOrWhiteSpace(baiNop.duongDan))
                    {
                        loi.Add("Nội dung không hợp lệ");
                    }
                }
            }
            #endregion

            if (loi.Count > 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = loi
                };
            }
            else
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }
        }

        public static void gan(ref BaiTapNopDTO baiNop, Form form)
        {
            if (baiNop == null)
            {
                baiNop = new BaiTapNopDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "MaTapTin":
                        baiNop.tapTin = TapTinBUS.chuyen("BaiTapNop_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "DuongDan":
                        baiNop.duongDan = form.layString(key);
                        break;
                    case "MaNguoiTao":
                        baiNop.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaBaiVietBaiTap":
                        baiNop.baiVietBaiTap = form.layDTO<BaiVietBaiTapDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static KetQua themHoacCapNhat(Form form, LienKet lienKet = null)
        {
            #region Kiểm tra điều kiện
            //Lấy người dùng
            var maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }

            //Lấy bài viết bài tập để lấy khóa học
            var maBaiVietBaiTap = form.layInt("MaBaiVietBaiTap");
            if (!maBaiVietBaiTap.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã bài tập không thể bỏ trống",
                };
            }
            var ketQua = BaiVietBaiTapDAO.layTheoMa(maBaiVietBaiTap);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài tập không tồn tại"
                };
            }
            var baiTap = ketQua.ketQua as BaiVietBaiTapDTO;

            //Kiểm tra thời hạn nộp
            if (baiTap.thoiDiemHetHan.HasValue && baiTap.thoiDiemHetHan.Value < DateTime.Now)
            {
                return new KetQua(3, "Bài tập đã hết hạn nộp");
            }

            //Kiểm tra người dùng là thành viên của khóa học
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(baiTap.khoaHoc.ma, maNguoiTao);
            var thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
            if (ketQua.trangThai != 0 || thanhVien.trangThai != 0 || !thanhVien.laHocVien)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn cần là học viên của khóa học để nộp bài"
                };
            }
            #endregion

            BaiTapNopDTO baiNop = new BaiTapNopDTO();

            gan(ref baiNop, form);

            baiNop.baiVietBaiTap = baiTap;
            ketQua = kiemTra(baiNop);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiTapNopDAO.themHoacCapNhat(baiNop, lienKet);
        }

        public static KetQua layTheoMaBaiVietBaiTap(int maBaiVietBaiTap, LienKet lienKet = null)
        {
            return BaiTapNopDAO.layTheoMaBaiVietBaiTap(maBaiVietBaiTap);
        }

        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return BaiTapNopDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua chamDiem(int ma, double diem, int maNguoiCham)
        {
            #region Kiểm tra điều kiện
            //Lấy bài nộp
            var ketQua = BaiTapNopDAO.layTheoMa(ma, new LienKet() { "BaiVietBaiTap" });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bài nộp không tồn tại");
            }
            var baiNop = ketQua.ketQua as BaiTapNopDTO;

            //Lấy bài tập
            var baiTap = baiNop.baiVietBaiTap;
            if (baiTap == null)
            {
                return new KetQua(1, "Bài tập không tồn tại");
            }

            //Kiểm tra bài tập
            if (baiTap.loai != 1 && baiTap.loai != 2)
            {
                return new KetQua(3, "Bài tập không phù hợp");
            }

            //Kiểm tra quyền
            if (!coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value, maNguoiCham))
            {
                return new KetQua(3, "Bạn không có quyền chấm điểm bài nộp");
            }
            #endregion

            return BaiTapNopDAO.capNhatTheoMa_Diem(ma, diem);
        }

        public static KetQua ghiChu(int ma, string ghiChu, int maNguoiGhiChu)
        {
            #region Kiểm tra điều kiện
            //Lấy bài nộp
            var ketQua = BaiTapNopDAO.layTheoMa(ma, new LienKet() { "BaiVietBaiTap" });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bài nộp không tồn tại");
            }
            var baiNop = ketQua.ketQua as BaiTapNopDTO;

            //Lấy bài tập
            var baiTap = baiNop.baiVietBaiTap;
            if (baiTap == null)
            {
                return new KetQua(1, "Bài tập không tồn tại");
            }

            //Kiểm tra quyền
            if (!coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value, maNguoiGhiChu))
            {
                return new KetQua(3, "Bạn không có quyền sửa ghi chú");
            }
            #endregion

            return BaiTapNopDAO.capNhatTheoMa_GhiChu(ma, ghiChu);
        }

        public static KetQua xoa(int ma, string lyDo, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy bài nộp
            var ketQua = BaiTapNopDAO.layTheoMa(ma, new LienKet() { "BaiVietBaiTap" });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bài nộp không tồn tại");
            }
            var baiNop = ketQua.ketQua as BaiTapNopDTO;

            //Lấy bài tập
            var baiTap = baiNop.baiVietBaiTap;
            if (baiTap == null)
            {
                return new KetQua(1, "Bài tập không tồn tại");
            }

            //Kiểm tra quyền
            if (!coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa bài nộp");
            }
            #endregion

            return BaiTapNopDAO.capNhatTheoMa_DaXoa(ma, lyDo);
        }

        public static KetQua xoa(string dsMa_string, string lyDo, int maNguoiXoa)
        {
            int[] dsMa;
            try
            {
                 dsMa = Array.ConvertAll<string, int>(dsMa_string.Split(','), int.Parse);
            }
            catch
            {
                return new KetQua(2, "Có lỗi xảy ra khi lấy bài nộp");
            }

            #region Kiểm tra điều kiện
            //Lấy bài nộp đầu tiên để lấy thông tin bài tập, khóa học
            var ketQua = BaiTapNopDAO.layTheoMa(dsMa[0], new LienKet() { "BaiVietBaiTap" });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bài nộp không tồn tại");
            }

            var baiTap = (ketQua.ketQua as BaiTapNopDTO).baiVietBaiTap;
            if (baiTap == null)
            {
                return new KetQua(1, "Bài tập không tồn tại");
            }

            //Lấy danh sách bài nộp để kiểm tra danh sách bài nộp hợp lệ
            ketQua = BaiTapNopDAO.layTheoMaBaiVietBaiTap(baiTap.ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(3, "Lấy bài nộp thất bại");
            }
            var dsBaiNop = ketQua.ketQua as List<BaiTapNopDTO>;
            foreach(var ma in dsMa)
            {
                if (!dsBaiNop.Exists(x => x.ma == ma))
                {
                    return new KetQua(3, "Danh sách bài nộp không hợp lệ");
                }
            }

            //Kiểm tra quyền
            if (!coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa bài nộp");
            }
            #endregion

            return BaiTapNopDAO.capNhatTheoMa_DaXoa_Nhieu(dsMa_string, lyDo);
        }

        public static KetQua nen(string dsMa_string, int maNguoiTai)
        {
            int[] dsMa;
            try
            {
                dsMa = Array.ConvertAll<string, int>(dsMa_string.Split(','), int.Parse);
            }
            catch
            {
                return new KetQua(2, "Có lỗi xảy ra khi lấy bài nộp");
            }

            #region Kiểm tra điều kiện
            //Lấy bài nộp đầu tiên để lấy thông tin bài tập, khóa học
            var ketQua = BaiTapNopDAO.layTheoMa(dsMa[0], new LienKet() 
            { 
                "BaiVietBaiTap"
            });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bài nộp không tồn tại");
            }

            var baiTap = (ketQua.ketQua as BaiTapNopDTO).baiVietBaiTap;
            if (baiTap == null)
            {
                return new KetQua(1, "Bài tập không tồn tại");
            }

            //Lấy danh sách bài nộp để kiểm tra danh sách bài nộp hợp lệ
            ketQua = BaiTapNopDAO.layTheoMaBaiVietBaiTap(baiTap.ma, new LienKet() 
            { 
                "NguoiTao",
                "TapTin"
            });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(3, "Lấy bài nộp thất bại");
            }
            var dsBaiNop = ketQua.ketQua as List<BaiTapNopDTO>;
            foreach (var ma in dsMa)
            {
                if (!dsBaiNop.Exists(x => x.ma == ma))
                {
                    return new KetQua(3, "Danh sách bài nộp không hợp lệ");
                }
            }

            //Kiểm tra quyền
            if (!coQuyen("BT_QLBaiNop", "KH", baiTap.khoaHoc.ma.Value, maNguoiTai))
            {
                return new KetQua(3, "Bạn không có quyền xóa bài nộp");
            }
            #endregion

            //Lấy tập tin
            string duongDanGoc = Helpers.TapTinHelper.layDuongDanGoc();
            List<string> dsDuongDan = new List<string>();
            foreach(var baiNop in dsBaiNop)
            {
                dsDuongDan.Add(duongDanGoc + "BaiTapNop_TapTin/" + baiNop.tapTin.ma + baiNop.tapTin.duoi);
                dsDuongDan.Add(
                    Path.GetFileNameWithoutExtension(baiNop.tapTin.ten) + "_" + 
                    Helpers.LCTHelper.boDau(baiNop.nguoiTao.ho + baiNop.nguoiTao.tenLot + baiNop.nguoiTao.ten).Replace(" ", "") + "_" +
                    baiNop.nguoiTao.tenTaiKhoan +
                    baiNop.tapTin.duoi);
            }

            string duongDanTapTinNen;
            //Nén
            try
            {
                duongDanTapTinNen = Helpers.TapTinHelper.nen(dsDuongDan.ToArray(), "");
            }
            catch
            {
                return new KetQua(3, "Nén file lỗi");
            }

            return new KetQua(new string []
                {
                    duongDanTapTinNen,
                    "application/zip",
                    baiTap.ma.Value + "_" + Helpers.LCTHelper.boDau(baiTap.tieuDe).Replace(' ', '_') + ".zip"
                });
        }
    }
}
