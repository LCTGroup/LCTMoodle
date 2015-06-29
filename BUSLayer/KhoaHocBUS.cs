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
    public class KhoaHocBUS : BUS
    {
        public static KetQua kiemTra(KhoaHocDTO khoaHoc, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrEmpty(khoaHoc.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (coKiemTra("MoTa", truong, kiemTra) && string.IsNullOrEmpty(khoaHoc.moTa))
            {
                loi.Add("Mô tả không được bỏ trống");
            }
            if (coKiemTra("MaChuDe", truong, kiemTra) && khoaHoc.chuDe == null)
            {
                loi.Add("Chủ đề không được bỏ trống");
            }
            if (coKiemTra("MaHinhDaiDien", truong, kiemTra) && khoaHoc.hinhDaiDien == null)
            {
                loi.Add("Hình đại diện không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && khoaHoc.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (coKiemTra("CheDoRiengTu", truong, kiemTra) && string.IsNullOrEmpty(khoaHoc.cheDoRiengTu))
            {
                loi.Add("Chế độ riêng tư không được bỏ trống");
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

        public static void gan(ref KhoaHocDTO khoaHoc, Form form)
        {
            if (khoaHoc == null)
            {
                khoaHoc = new KhoaHocDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "Ten":
                        khoaHoc.ten = form.layString(key);
                        break;
                    case "MoTa":
                        khoaHoc.moTa = form.layString(key);
                        break;
                    case "MaChuDe":
                        khoaHoc.chuDe = form.layDTO<ChuDeDTO>(key);
                        break;
                    case "MaNguoiTao":
                        khoaHoc.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaHinhDaiDien":
                        khoaHoc.hinhDaiDien = TapTinBUS.chuyen("KhoaHoc_HinhDaiDien", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "CanDangKy":
                        khoaHoc.canDangKy = form.layBool(key);
                        if (khoaHoc.canDangKy)
                        {
                            khoaHoc.hanDangKy = form.layDateTime("HanDangKy");
                        }
                        break;
                    case "PhiThamGia":
                        khoaHoc.phiThamGia = form.layInt(key);
                        break;
                    case "CheDoRiengTu":
                        khoaHoc.cheDoRiengTu = form.layString(key);
                        break;
                    case "CoHan":
                        khoaHoc.thoiDiemHetHan = form.layDateTime("ThoiDiemHetHan");
                        break;
                    default:
                        break;
                }
            }
        }

        public static KetQua them(Form form, int? maNguoiDung = null)
        {
            #region Kiểm tra quyền
            //Lấy người dùng
            if (!maNguoiDung.HasValue)
            {
                maNguoiDung = Session["NguoiDung"] as int?;
            }
            if (!maNguoiDung.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }

            //Lấy quyền tạo khóa học của người dùng
            if (!coQuyen("QLNoiDung", "KH", 0, maNguoiDung))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền tạo khóa học"
                };
            }
            #endregion

            #region Thêm khóa học
		    var khoaHoc = new KhoaHocDTO();
            gan(ref khoaHoc, form);

            var ketQua = kiemTra(khoaHoc);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return KhoaHocDAO.them(khoaHoc); 
	        #endregion
        }

        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return KhoaHocDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua lay(LienKet lienKet = null)
        {
            return KhoaHocDAO.lay(lienKet);
        }

        public static KetQua layTheoMaChuDe(int maChuDe, LienKet lienKet = null)
        {
            return KhoaHocDAO.layTheoMaChuDe(maChuDe, lienKet);
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null)
        {
            return KhoaHocDAO.lay_TimKiem(tuKhoa, lienKet);
        }

        public static KetQua layTheoMaChuDe_TimKiem(int maChuDe, string tuKhoa, LienKet lienKet = null)
        {
            return KhoaHocDAO.layTheoMaChuDe_TimKiem(maChuDe, tuKhoa, lienKet);
        }

        public static KetQua layTheoMaNguoiDung(int maNguoiDung)
        {
            //Lấy toàn bộ khóa học mà người dùng liên quan
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaNguoiDung(maNguoiDung, new LienKet() { "KhoaHoc" });
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            List<KhoaHocDTO>
                danhSachThamGia = new List<KhoaHocDTO>(),
                danhSachDangKy = new List<KhoaHocDTO>(),
                danhSachDuocMoi = new List<KhoaHocDTO>(),
                danhSachBiChan = new List<KhoaHocDTO>();

            foreach(var thanhVien in ketQua.ketQua as List<KhoaHoc_NguoiDungDTO>)
            {
                switch(thanhVien.trangThai)
                {
                    case 0:
                        danhSachThamGia.Add(thanhVien.khoaHoc);
                        break;
                    case 1:
                        danhSachDangKy.Add(thanhVien.khoaHoc);
                        break;
                    case 2:
                        danhSachDuocMoi.Add(thanhVien.khoaHoc);
                        break;
                    case 3:
                        danhSachBiChan.Add(thanhVien.khoaHoc);
                        break;
                    default:
                        break;
                }
            }

            return new KetQua()
            {
                trangThai = 0,
                ketQua = new List<KhoaHocDTO>[]
                {
                    danhSachThamGia.Count != 0 ? danhSachThamGia : null,
                    danhSachDangKy.Count != 0 ? danhSachDangKy : null,
                    danhSachDuocMoi.Count != 0 ? danhSachDuocMoi : null,
                    danhSachBiChan.Count != 0 ? danhSachBiChan : null
                }
            };
        }

        public static KetQua layTheoMaNguoiDungVaTrangThai(int maNguoiDung, int trangThai)
        {
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaNguoiDungVaTrangThai(maNguoiDung, trangThai, new LienKet() { "KhoaHoc" });

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            var dsKhoaHoc = new List<KhoaHocDTO>();

            foreach (var thanhVien in ketQua.ketQua as List<KhoaHoc_NguoiDungDTO>)
            {
                dsKhoaHoc.Add(thanhVien.khoaHoc);
            }

            return new KetQua()
            {
                trangThai = 0,
                ketQua = dsKhoaHoc
            };
        }
    }
}
