﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using Helpers;

namespace BUSLayer
{
    public class TraLoiBUS : BUS
    {

        public static KetQua kiemTra(TraLoiDTO traLoi, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(traLoi.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && traLoi.nguoiTao == null)
            {
                loi.Add("Người dùng không được bỏ trống");
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

        public static void gan(ref TraLoiDTO traLoi, Form form)
        {
            if (traLoi == null)
            {
                traLoi = new TraLoiDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "NoiDung":
                        traLoi.noiDung = form.layString(key);
                        break;
                    case "MaNguoiTao":
                        traLoi.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaCauHoi":
                        traLoi.cauHoi = form.layDTO<CauHoiDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(TraLoiDTO traLoi, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "NoiDung":
                        bangCapNhat.Add("NoiDung", traLoi.noiDung, 2);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }
        
        #region Thêm

        public static KetQua them(Form form)
        {
            #region Kiểm tra điều kiện

            int? maNguoiDung = form.layInt("MaNguoiTao");
            if (maNguoiDung == null)
            {
                return new KetQua(4, "Bạn chưa đăng nhập");
            }

            #endregion

            TraLoiDTO traLoi = new TraLoiDTO();
            gan(ref traLoi, form);

            KetQua ketQua = TraLoiBUS.kiemTra(traLoi);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = TraLoiDAO.them(traLoi, new LienKet() { "NguoiTao", "CauHoi" });
            if (ketQua.trangThai == 0)
            {
                var ketQuaTraLoi = ketQua.ketQua as TraLoiDTO;
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiDung,
                    loaiDoiTuongBiTacDong = "CH",
                    maDoiTuongBiTacDong = ketQuaTraLoi.cauHoi.ma,
                    hanhDong = layDTO<HanhDongDTO>(410),
                    duongDan = "/HoiDap/" + ketQuaTraLoi.cauHoi.ma
                });
            }
            return ketQua;
        }

        #endregion

        #region Xóa

        public static KetQua xoaTheoMa(int? ma, int? maNguoiXoa)
        {
            #region Kiểm tra điều kiện

            var ketQua = TraLoiBUS.layTheoMa(ma, new LienKet() { "CauHoi" });
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            var traLoi = ketQua.ketQua as TraLoiDTO;

            if (traLoi.nguoiTao.ma != maNguoiXoa && !BUS.coQuyen("XoaTraLoi", "HD", traLoi.cauHoi.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn chưa đủ quyền để xóa trả lời");
            }

            #endregion

            ketQua = TraLoiDAO.xoaTheoMa(ma);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiXoa,
                    loaiDoiTuongBiTacDong = "TL",
                    maDoiTuongBiTacDong = traLoi.ma,
                    hanhDong = layDTO<HanhDongDTO>(411),
                    duongDan = "/HoiDap/"
                });
            }
            return ketQua;
        }

        #endregion

        #region Sửa

        public static KetQua capNhat(Form form, LienKet lienKet = null)
        {
            #region Kiểm tra điều kiện

            int? maNguoiSua = form.layInt("MaNguoiSua");
            int? maTraLoi = form.layInt("Ma");

            KetQua ketQua = TraLoiBUS.layTheoMa(maTraLoi.Value, new LienKet() { "CauHoi" });
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            TraLoiDTO traLoi = ketQua.ketQua as TraLoiDTO;

            if (traLoi.nguoiTao.ma != maNguoiSua && !BUS.coQuyen("SuaTraLoi", "HD", traLoi.cauHoi.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có đủ quyền để sửa trả lời này");
            }

            #endregion

            gan(ref traLoi, form);

            ketQua = TraLoiBUS.kiemTra(traLoi, form.Keys.ToArray());
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = TraLoiDAO.capNhatTheoMa(maTraLoi, layBangCapNhat(traLoi, form.Keys.ToArray()), lienKet);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiSua,
                    loaiDoiTuongBiTacDong = "TL",
                    maDoiTuongBiTacDong = traLoi.ma,
                    hanhDong = layDTO<HanhDongDTO>(410),
                    duongDan = "/HoiDap/" + traLoi.cauHoi.ma
                });
            }
            return ketQua;
        }

        public static KetQua capNhatDuyetTheoMa(int? ma, bool duyet, int? maNguoiDuyet)
        {

            #region Kiểm tra điều kiện

            if (!maNguoiDuyet.HasValue)
            {
                return new KetQua(4);
            }

            var ketQua = TraLoiBUS.layTheoMa(ma, new LienKet() { "CauHoi" });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(3, "Lấy trả lời thất bại");
            }
            var traLoi = ketQua.ketQua as TraLoiDTO;
            
            if (maNguoiDuyet != traLoi.cauHoi.nguoiTao.ma)
            {
                return new KetQua(3, "Bạn không có quyền duyệt trả lời");
            }

            #endregion

            ketQua = TraLoiDAO.capNhatDuyetTheoMa(ma, duyet);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiDuyet,
                    loaiDoiTuongBiTacDong = "TL",
                    maDoiTuongBiTacDong = traLoi.ma,
                    hanhDong = layDTO<HanhDongDTO>(414),
                    duongDan = "/HoiDap/" + traLoi.cauHoi.ma
                });
            }
            return ketQua;
        }

        public static KetQua duyetHienThiTraLoi(int? maTraLoi, bool trangThai, int? maNguoiDuyet)
        {
            #region Kiểm tra điều kiện

            if (!maNguoiDuyet.HasValue)
            {
                return new KetQua(4);
            }

            if (!QuyenBUS.coQuyen("DuyetTraLoi", "HD", 0, maNguoiDuyet))
            {
                return new KetQua(3, "Bạn không đủ quyền duyệt trả lời");
            }

            #endregion
            
            var ketQua = TraLoiDAO.capNhatTheoMa_DuyetHienThi(maTraLoi, trangThai);
            if (ketQua.trangThai == 0)
            {
                int maCauHoi = (TraLoiBUS.layTheoMa(maTraLoi, new LienKet() { "CauHoi" }).ketQua as TraLoiDTO).cauHoi.ma.Value;
                HoatDongBUS.them(new HoatDongDTO()
                {                    
                    maNguoiTacDong = maNguoiDuyet,
                    loaiDoiTuongBiTacDong = "TL",
                    maDoiTuongBiTacDong = maTraLoi,
                    hanhDong = layDTO<HanhDongDTO>(413),
                    duongDan = "/HoiDap/" + maCauHoi
                });
            }
            return ketQua;
        }

        #endregion

        #region Lấy

        public static KetQua layTheoMaCauHoi(int maCauHoi, LienKet lienKet = null)
        {
            return TraLoiDAO.layTheoMaCauHoi(maCauHoi, lienKet);
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return TraLoiDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua laySoLuongTraLoiTrongCauHoi(int maCauHoi)
        {
            return TraLoiDAO.layTraLoiTheoMaCauHoi_SoLuong(maCauHoi);
        }

        public static KetQua layTraLoi_DanhSachMaLienQuan(int? maNguoiTao)
        {
            return TraLoiDAO.layTraLoi_DanhSachMaLienQuan(maNguoiTao);
        }

        #endregion
        
    }
}