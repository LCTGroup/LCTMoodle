using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using System.Web;
using Data;

namespace BUSLayer
{
    public class BaiVietBaiTapBUS : BUS
    {
        public static KetQua kiemTra(BaiVietBaiTapDTO baiViet, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrWhiteSpace(baiViet.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(baiViet.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (coKiemTra("Loai", truong, kiemTra) && !baiViet.loai.HasValue)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && baiViet.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (coKiemTra("MaKhoaHoc", truong, kiemTra) && baiViet.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
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

        public static void gan(ref BaiVietBaiTapDTO baiViet, Form form)
        {
            if (baiViet == null)
            {
                baiViet = new BaiVietBaiTapDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "TieuDe":
                        baiViet.tieuDe = form.layString(key);
                        break;
                    case "NoiDung":
                        baiViet.noiDung = form.layString(key);
                        break;
                    case "MaTapTin":
                        baiViet.tapTin = TapTinBUS.chuyen("BaiVietBaiTap_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "Loai":
                        baiViet.loai = form.layInt(key);
                        break;
                    case "CoThoiDiemHetHan":
                        baiViet.thoiDiemHetHan = form.layBool("CoThoiDiemHetHan") ? form.layDateTime("ThoiDiemHetHan") : null;
                        break;
                    case "MaNguoiTao":
                        baiViet.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaKhoaHoc":
                        baiViet.khoaHoc = form.layDTO<KhoaHocDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(BaiVietBaiTapDTO baiTap, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add("TieuDe", baiTap.tieuDe, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add("NoiDung", baiTap.noiDung, 2);
                        break;
                    case "MaTapTin":
                        bangCapNhat.Add("MaTapTin", baiTap.tapTin == null ? null : baiTap.tapTin.ma.ToString(), 1);
                        break;
                    case "Loai":
                        bangCapNhat.Add("Loai", baiTap.loai.HasValue ? baiTap.loai.Value.ToString() : null, 1);
                        break;
                    case "CoThoiDiemHetHan":
                        bangCapNhat.Add("ThoiDiemHetHan", baiTap.thoiDiemHetHan.HasValue ? baiTap.thoiDiemHetHan.Value.ToString("d/M/yyyy H:mm") : null, 3);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua them(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người dùng
            var maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua(3, "Người tạo không hợp lệ");
            }

            //Lấy mã khóa học
            var maKhoaHoc = form.layInt("MaKhoaHoc");
            if (!maKhoaHoc.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Khóa học không hợp lệ"
                };
            }

            if (!coQuyen("BT_Them", "KH", maKhoaHoc.Value, maNguoiTao))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền đăng bài tập"
                };
            }

            //Loại bài tập
            var loai = form.layInt("Loai", 1).Value;
            if (!(loai == 0 || loai == 1 || loai == 2))
            {
                return new KetQua(3, "Loại không được bỏ trống");
            }

            if (!coQuyen("QLBangDiem", "KH", maKhoaHoc.Value, maNguoiTao))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền tạo bài tập có điểm"
                };
            }
            #endregion

            BaiVietBaiTapDTO baiViet = new BaiVietBaiTapDTO();
            gan(ref baiViet, form);

            var ketQua = kiemTra(baiViet, new string[] { "MaKhoaHoc", "Loai", "MaNguoiTao" }, false);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = BaiVietBaiTapDAO.them(baiViet, new LienKet()
                {
                    "NguoiTao",
                    "TapTin"
                });

            if (loai == 0)
            {
                return ketQua;
            }
            else
            {
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }

                //Thêm cột điểm
                var baiTap = ketQua.ketQua as BaiVietBaiTapDTO;

                Form formCotDiem = new Form()
                {
                    { "MaKhoaHoc", maKhoaHoc.ToString() },
                    { "Ten", "Bài tập " + baiTap.thoiDiemTao.Value.ToString("d/M") },
                    { "MoTa", "Bài tập: " + baiTap.tieuDe },
                    { "LaDiemCong", loai == 1 ? "1" : "0" },
                    { "MaNguoiTao", maNguoiTao.Value.ToString() },
                    { "Ngay", DateTime.Now.ToString("d/M/yyyy") },
                    { "LoaiDoiTuong", "BaiTap" },
                    { "MaDoiTuong", baiTap.ma.ToString() }
                };

                if (loai == 2)
                {
                    formCotDiem.Add("HeSo", form.layString("CD_HeSo"));
                }

                ketQua = CotDiemBUS.them(formCotDiem);

                if (ketQua.trangThai != 0)
                {
                    BaiVietBaiTapDAO.xoaTheoMa(baiTap.ma);
                    return ketQua;
                }

                return new KetQua(baiTap);
            }
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return BaiVietBaiTapDAO.layTheoMaKhoaHoc(maKhoaHoc, new LienKet()
            {
                "NguoiTao",
                "TapTin",
                "BaiTapNop"
            });
        }

        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return BaiVietBaiTapDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return BaiVietBaiTapDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            int? maBaiViet = form.layInt("Ma");
            if (!maBaiViet.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = layTheoMa(maBaiViet.Value);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiTapDTO baiViet = ketQua.ketQua as BaiVietBaiTapDTO;

            gan(ref baiViet, form);

            ketQua = kiemTra(baiViet, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiTapDAO.capNhatTheoMa(maBaiViet, layBangCapNhat(baiViet, form.Keys.ToArray()), new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
