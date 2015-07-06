using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using System.Web;

namespace BUSLayer
{
    public class BaiVietDienDanBUS : BUS
    {
        public static KetQua kiemTra(BaiVietDienDanDTO baiViet, string[] truong = null, bool kiemTra = true)
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

        public static void gan(ref BaiVietDienDanDTO baiViet, Form form)
        {
            if (baiViet == null)
            {
                baiViet = new BaiVietDienDanDTO();
            }

            foreach(string key in form.Keys.ToArray())
            {
                switch(key)
                {
                    case "TieuDe":
                        baiViet.tieuDe = form.layString(key);
                        break;
                    case "NoiDung":
                        baiViet.noiDung = form.layString(key);
                        break;
                    case "MaTapTin":
                        baiViet.tapTin = TapTinBUS.chuyen("BaiVietDienDan_TapTin", form.layInt(key)).ketQua as TapTinDTO;
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

        public static BangCapNhat layBangCapNhat(BaiVietDienDanDTO baiViet, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add(key, baiViet.tieuDe, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add(key, baiViet.noiDung, 2);
                        break;
                    case "MaTapTin":
                        bangCapNhat.Add(key, baiViet.tapTin == null ? null : baiViet.tapTin.ma.ToString(), 1);
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
            //Lấy người tạo
            var maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }

            //Khóa học
            var maKhoaHoc = form.layInt("MaKhoaHoc");
            if (!maKhoaHoc.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã khóa học không được bỏ trống"
                };
            }

            //Người dùng là thành viên
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiTao);
            if (ketQua.trangThai != 0 || (ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn cần phải là thành viên của diễn đàn để đăng bài viết"
                };
            }
	        #endregion

            var baiViet = new BaiVietDienDanDTO();
            gan(ref baiViet, form);
            
            ketQua = kiemTra(baiViet);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietDienDanDAO.them(baiViet, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return BaiVietDienDanDAO.layTheoMaKhoaHoc(maKhoaHoc, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoMa(int ma)
        {
            return BaiVietDienDanDAO.layTheoMa(ma);
        }

        public static KetQua xoaTheoMa(int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy bài viết
            var ketQua = BaiVietDienDanDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài viết không tồn tại"
                };
            }

            var baiViet = ketQua.ketQua as BaiVietDienDanDTO;

            if (baiViet.nguoiTao.ma != maNguoiXoa && !coQuyen("DD_QLNoiDung", "KH", baiViet.khoaHoc.ma.Value, maNguoiXoa))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xóa bài viết"
                };
            }
            #endregion

            return BaiVietDienDanDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người sửa
            int? maNguoiSua = form.layInt("MaNguoiSua");
            if (!maNguoiSua.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã người sửa không được bỏ trống"
                };
            }

            //Lấy bài viết
            int? ma = form.layInt("Ma");
            if (!ma.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã bài viết không thể bỏ trống"
                };
            }
            var ketQua = BaiVietDienDanDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài viết không tồn tại"
                };
            }
            var baiViet = ketQua.ketQua as BaiVietDienDanDTO;

            //Kiểm tra quyền
            if (baiViet.nguoiTao.ma != maNguoiSua && !coQuyen("DD_QLNoiDung", "KH", baiViet.khoaHoc.ma.Value, maNguoiSua))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa bài viết"
                };
            }
            #endregion

            gan(ref baiViet, form);

            ketQua = kiemTra(baiViet, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BangCapNhat bang = layBangCapNhat(baiViet, form.Keys.ToArray());
            if (!bang.coDuLieu())
            {
                return new KetQua(baiViet);
            }

            return BaiVietDienDanDAO.capNhatTheoMa(ma, bang, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua ghim(int ma, bool ghim, int maNguoiGhim)
        {
            #region Kiểm tra điều kiện
            //Lấy bài viết
            var ketQua = BaiVietDienDanDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài viết không tồn tại"
                };
            }

            var baiViet = ketQua.ketQua as BaiVietDienDanDTO;
            if (!coQuyen("DD_QLNoiDung", "KH", baiViet.khoaHoc.ma.Value, maNguoiGhim))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền ghim"
                };
            }
            #endregion
            
            //Xóa ghim hiện tại nếu là ghim
            if (ghim)
            {
                BaiVietDienDanDAO.capNhatTheoMaKhoaHoc_XoaGhim(baiViet.khoaHoc.ma);
            }

            //Cập nhật ghim
            return BaiVietDienDanDAO.capNhatTheoMa_Ghim(ma, ghim);
        }

        public static KetQua capNhatDiem(int ma, int diem, int maNguoiSua)
        {
            #region Kiểm tra điều kiện
            //Lấy bài viết
            var ketQua = BaiVietDienDanDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bài viết không tồn tại");
            }
            var baiViet = ketQua.ketQua as BaiVietDienDanDTO;

            //Kiểm tra quyền
            if (!coQuyen("DD_QLDiem", "KH", baiViet.khoaHoc.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có quyền cho điểm bài viết");
            }
            #endregion

            return BaiVietDienDanDAO.capNhatTheoMa_Diem(ma, diem);
        }
    }
}
