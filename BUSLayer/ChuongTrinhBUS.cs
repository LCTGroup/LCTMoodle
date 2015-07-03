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
    public class ChuongTrinhBUS : BUS
    {
        public static KetQua kiemTra(ChuongTrinhDTO chuongTrinh, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("MaKhoaHoc", truong, kiemTra) && chuongTrinh.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (coKiemTra("BaiHoc", truong, kiemTra) && string.IsNullOrWhiteSpace(chuongTrinh.baiHoc))
            {
                loi.Add("Bài học không được bỏ trống");
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

        public static void gan(ref ChuongTrinhDTO chuongTrinh, Form form)
        {
            if (chuongTrinh == null)
            {
                chuongTrinh = new ChuongTrinhDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "MaKhoaHoc":
                        chuongTrinh.khoaHoc = form.layDTO<KhoaHocDTO>(key);
                        break;
                    case "BaiHoc":
                        chuongTrinh.baiHoc = form.layString(key);
                        break;
                    case "NoiDung":
                        chuongTrinh.noiDung = form.layString(key);
                        break;
                    case "ThoiGian":
                        chuongTrinh.thoiGian = form.layString(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(ChuongTrinhDTO chuongTrinh, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "BaiHoc":
                        bangCapNhat.Add(key, chuongTrinh.baiHoc, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add(key, chuongTrinh.noiDung, 2);
                        break;
                    case "ThoiGian":
                        bangCapNhat.Add(key, chuongTrinh.thoiGian, 2);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return ChuongTrinhDAO.layTheoMaKhoaHoc(maKhoaHoc);
        }

        public static KetQua them(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người tạo
            var maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã người tạo không được bỏ trống"
                };
            }

            //Lấy mã khóa học
            var maKhoaHoc = form.layInt("MaKhoaHoc");
            if (!maKhoaHoc.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã khóa học không được bỏ trống"
                };
            }

            //Kiểm tra quyền
            if (!coQuyen("QLChuongTrinh", "KH", maKhoaHoc.Value, maNguoiTao))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền thêm mục chương trình"
                };
            }
            #endregion

            ChuongTrinhDTO chuongTrinh = new ChuongTrinhDTO();

            gan(ref chuongTrinh, form);

            KetQua ketQua = kiemTra(chuongTrinh);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return ChuongTrinhDAO.them(chuongTrinh);
        }

        public static KetQua xoaTheoMa(int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy mục chương trình
            var ketQua = ChuongTrinhDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Mục chương trình không tồn tại"
                };
            }
            var chuongTrinh = ketQua.ketQua as ChuongTrinhDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLChuongTrinh", "KH", chuongTrinh.khoaHoc.ma.Value, maNguoiXoa))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xóa mục chương trình"
                };
            }
            #endregion

            return ChuongTrinhDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatThuTu(int thuTuCu, int thuTuMoi, int maKhoaHoc, int maNguoiSua)
        {
            #region Kiểm tra điều kiện
                        //Kiểm tra quyền
            if (!coQuyen("QLChuongTrinh", "KH", maKhoaHoc, maNguoiSua))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa mục chương trình"
                };
            }
            #endregion

            return ChuongTrinhDAO.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc);
        }
    }
}
