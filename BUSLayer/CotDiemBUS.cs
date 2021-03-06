﻿using System;
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
    public class CotDiemBUS : BUS
    {
        public static KetQua kiemTra(CotDiemDTO cotDiem, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("MaKhoaHoc", truong, kiemTra) && cotDiem.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrWhiteSpace(cotDiem.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            //Nếu không phải là điểm cộng thì phải có hệ số
            if (coKiemTra("HeSo", truong, kiemTra) && !cotDiem.laDiemCong && (!cotDiem.heSo.HasValue || cotDiem.heSo < 1))
            {
                loi.Add("Hệ số không hợp lệ");
            }
            if (coKiemTra("Ngay", truong, kiemTra) && cotDiem.ngay == null)
            {
                loi.Add("Ngày không được bỏ trống");
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

        public static void gan(ref CotDiemDTO cotDiem, Form form)
        {
            if (cotDiem == null)
            {
                cotDiem = new CotDiemDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "Ten":
                        cotDiem.ten = form.layString(key);
                        break;
                    case "MoTa":
                        cotDiem.moTa = form.layString(key);
                        break;
                    case "Ngay":
                        cotDiem.ngay = form.layDate(key);
                        break;
                    case "MaKhoaHoc":
                        cotDiem.khoaHoc = form.layDTO<KhoaHocDTO>(key);
                        break;
                    case "LaDiemCong":
                        cotDiem.laDiemCong = form.layBool(key);
                        if (!cotDiem.laDiemCong)
                        {
                            cotDiem.heSo = form.layInt("HeSo");
                        }
                        break;
                    case "LoaiDoiTuong":
                        cotDiem.loaiDoiTuong = form.layString(key);
                        break;
                    case "MaDoiTuong":
                        cotDiem.doiTuong = form.layDTO<DTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(CotDiemDTO cotDiem, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "Ten":
                        bangCapNhat.Add(key, cotDiem.ten, 2);
                        break;
                    case "MoTa":
                        bangCapNhat.Add(key, cotDiem.moTa, 2);
                        break;
                    case "Ngay":
                        bangCapNhat.Add(key, cotDiem.ngay.HasValue ? cotDiem.ngay.Value.ToString("d/M/yyyy") : null, 3);
                        break;
                    case "LaDiemCong":
                        bangCapNhat.Add(key, cotDiem.laDiemCong ? "1" : "0", 1);
                        if (!cotDiem.laDiemCong)
                        {
                            bangCapNhat.Add("HeSo", cotDiem.heSo.ToString(), 1);
                        }
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
            //Lấy mã người tạo
            int? maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã người tạo không được bỏ trống"
                };
            }

            //Lấy mã khóa học
            int? maKhoaHoc = form.layInt("MaKhoaHoc");
            if (!maKhoaHoc.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã khóa học không được bỏ trống"
                };
            }

            //Kiểm tra quyền
            if (!coQuyen("QLBangDiem", "KH", maKhoaHoc.Value, maNguoiTao))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền thêm cột điểm"
                };
            }
            #endregion

            CotDiemDTO cotDiem = new CotDiemDTO();

            gan(ref cotDiem, form);

            KetQua ketQua = kiemTra(cotDiem);
            
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            
            return CotDiemDAO.them(cotDiem);
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return CotDiemDAO.layTheoMaKhoaHoc(maKhoaHoc);
        }

        public static KetQua layTheoMa(int ma)
        {
            return CotDiemDAO.layTheoMa(ma);
        }

        public static KetQua xoaTheoMa(int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy mục chương trình
            var ketQua = CotDiemDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Cột điểm không tồn tại"
                };
            }
            var cotDiem = ketQua.ketQua as CotDiemDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLBangDiem", "KH", cotDiem.khoaHoc.ma.Value, maNguoiXoa))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xóa cột điểm"
                };
            }
            #endregion

            return CotDiemDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatThuTu(int thuTuCu, int thuTuMoi, int maKhoaHoc, int maNguoiSua)
        {
            #region Kiểm tra điều kiện
            //Kiểm tra quyền
            if (!coQuyen("QLBangDiem", "KH", maKhoaHoc, maNguoiSua))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa cột điểm"
                };
            }
            #endregion

            return CotDiemDAO.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc);
        }

        public static KetQua capNhat(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người sửa, mã
            var maNguoiSua = form.layInt("MaNguoiSua");
            var ma = form.layInt("Ma");
            
            //Lấy cột điểm
            var ketQua = CotDiemDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Cột điểm không tồn tại");
            }
            var cotDiem = ketQua.ketQua as CotDiemDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLBangDiem", "KH", cotDiem.khoaHoc.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có quyền sửa bảng điểm");
            }
            #endregion

            gan(ref cotDiem, form);

            ketQua = kiemTra(cotDiem, form.Keys.ToArray());
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            var bang = layBangCapNhat(cotDiem, form.Keys.ToArray());
            if (!bang.coDuLieu())
            {
                return new KetQua(cotDiem);
            }

            return CotDiemDAO.capNhatTheoMa(ma, bang);
        }

        public static KetQua layTheoLoaiDoiTuongVaMaDoiTuong(string loaiDoiTuong, int maDoiTuong)
        {
            return CotDiemDAO.layTheoLoaiDoiTuongVaMaDoiTuong(loaiDoiTuong, maDoiTuong);
        }
    }
}
