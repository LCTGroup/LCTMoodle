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
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrWhiteSpace(khoaHoc.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (coKiemTra("MoTa", truong, kiemTra) && string.IsNullOrWhiteSpace(khoaHoc.moTa))
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
            if (coKiemTra("CheDoRiengTu", truong, kiemTra) && string.IsNullOrWhiteSpace(khoaHoc.cheDoRiengTu))
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

        public static BangCapNhat layBangCapNhat(KhoaHocDTO khoaHoc, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "Ten":
                        bangCapNhat.Add(key, khoaHoc.ten, 2);
                        break;
                    case "MoTa":
                        bangCapNhat.Add(key, khoaHoc.moTa, 2);
                        break;
                    case "MaChuDe":
                        bangCapNhat.Add(key, khoaHoc.chuDe != null ? khoaHoc.chuDe.ma.ToString() : null, 2);
                        break;
                    case "MaHinhDaiDien":
                        bangCapNhat.Add(key, khoaHoc.hinhDaiDien == null ? null : khoaHoc.hinhDaiDien.ma.ToString(), 1);
                        break;
                    case "CanDangKy":
                        bangCapNhat.Add(key, khoaHoc.canDangKy ? "1" : "0", 1);
                        if (khoaHoc.canDangKy)
                        {
                            bangCapNhat.Add("HanDangKy", khoaHoc.hanDangKy.HasValue ? khoaHoc.hanDangKy.Value.ToString("d/M/yyyy H:mm") : null, 3);
                        }
                        break;
                    case "PhiThamGia":
                        bangCapNhat.Add(key, khoaHoc.phiThamGia.ToString(), 1);
                        break;
                    case "CheDoRiengTu":
                        bangCapNhat.Add(key, khoaHoc.cheDoRiengTu, 2);
                        break;
                    case "CoHan":
                        bangCapNhat.Add("ThoiDiemHetHan", khoaHoc.thoiDiemHetHan.HasValue ? khoaHoc.thoiDiemHetHan.Value.ToString("d/M/yyyy H:mm") : null, 3);
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
                return new KetQua(3, "Người tạo không được bỏ trống");
            }

            //Kiểm tra có thuộc tính giảng viên không
            var maGiangVien = form.layInt("MaGiangVien");
            if (!maGiangVien.HasValue)
            {
                return new KetQua(3, "Khóa học cần có giảng viên");
            }

            //Kiểm tra thuộc tính chủ đề
            var maChuDe = form.layInt("MaChuDe");
            if (!maChuDe.HasValue)
            {
                return new KetQua(3, "Khóa học cần có chủ đề");
            }

            //Lấy quyền tạo khóa học của người dùng
            //Lấy chủ đề mà người dùng có thể tạo
            //Ở hệ thống
            var ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("HT", maNguoiTao.Value, "KH", "QLNoiDung");
            if (ketQua.trangThai != 0)
            {
                //Ở chủ đề
                ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("CD", maNguoiTao.Value, "KH", "QLNoiDung");

                if (ketQua.trangThai != 0)
                {
                    //Nếu không tìm thấy ở 2 phạm vi tren => ko có quyền
                    return new KetQua(3, "Bạn không có quyền tạo khóa học");
                }
            }

            //Kiểm tra người dùng có thể tạo khóa học ở chủ đề này không
            foreach (var maCD in (ketQua.ketQua as string).Split('|').Select(int.Parse))
            {
                ketQua = ChuDeBUS.thuocCay(maChuDe.Value, maCD);
                if (ketQua.trangThai != 0 || !(bool)ketQua.ketQua)
                {
                    return new KetQua(3, "Bạn không có quyền tạo khóa học");
                }
            }
            #endregion

            #region Thêm khóa học
		    var khoaHoc = new KhoaHocDTO();
            gan(ref khoaHoc, form);

            ketQua = kiemTra(khoaHoc);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = KhoaHocDAO.them(khoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
	        #endregion

            #region Tạo nhóm cho khóa học
            khoaHoc = ketQua.ketQua as KhoaHocDTO;
            ketQua = NhomNguoiDungBUS.themNhomMacDinh("KH", khoaHoc.ma.Value);
            if (ketQua.trangThai > 1)
            {
                return new KetQua(2, "Tạo nhóm người dùng thất bại");
            }

            //Thêm giảng viên vào nhóm giảng viên
            foreach (var nhom in ketQua.ketQua as List<NhomNguoiDungDTO>) 
            {
                if (nhom.giaTri == "GiangVien")
                {
                    ketQua = NhomNguoiDung_NguoiDungDAO.them("KH", nhom.ma, maGiangVien);
                    if (ketQua.trangThai != 0)
                    {
                        return new KetQua(3, "Đưa giảng viên vào nhóm thất bại");
                    }
                    break;
                }
            }
            #endregion

            return new KetQua(khoaHoc);
        }
        
        public static KetQua capNhat(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy người tạo, mã khóa học
            var maNguoiSua = form.layInt("MaNguoiSua");
            var ma = form.layInt("Ma");

            if (!maNguoiSua.HasValue)
            {
                return new KetQua(3, "Người sửa không thể bỏ trống.");
            }

            //Lấy mã chủ đề
            var maChuDe = form.layInt("MaChuDe");
            
            //Lấy khóa học
            var ketQua = KhoaHocDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1);
            }

            var khoaHoc = ketQua.ketQua as KhoaHocDTO;
            
            //Nếu có thay đổi chủ đề => kiểm tra quyền trên chủ đề mới
            //Nếu không => kiểm tra quyền cập nhật thông tin hoặc quyền trên chủ đề hiện tại
            if (maChuDe.HasValue)
            {
                //Lấy quyền tạo khóa học của người dùng
                //Lấy chủ đề mà người dùng có thể tạo
                //Ở hệ thống
                ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("HT", maNguoiSua.Value, "KH", "QLNoiDung");
                if (ketQua.trangThai != 0)
                {
                    //Ở chủ đề
                    ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("CD", maNguoiSua.Value, "KH", "QLNoiDung");

                    if (ketQua.trangThai != 0)
                    {
                        //Nếu không tìm thấy ở 2 phạm vi tren => ko có quyền
                        return new KetQua(3, "Bạn không có quyền sửa khóa học");
                    }
                }

                //Kiểm tra người dùng có thể quyền tạo khóa học ở chủ đề này không
                foreach (var maCD in (ketQua.ketQua as string).Split('|').Select(int.Parse))
                {
                    ketQua = ChuDeBUS.thuocCay(maChuDe.Value, maCD);
                    if (ketQua.trangThai != 0 || !(bool)ketQua.ketQua)
                    {
                        return new KetQua(3, "Bạn cần có quyền tạo khóa học trên chủ đề mới.");
                    }
                }
            }
            else
            {
                //Cần có quyền quản lý thông tin khóa học
                if (!coQuyen("QLThongTin", "KH", ma.Value, maNguoiSua))
                {
                    //Hoặc có quyền quản lý nội dung khóa học ở chủ đề hoặc hệ thống

                    //Lấy quyền tạo khóa học của người dùng
                    //Lấy chủ đề mà người dùng có thể tạo
                    //Ở hệ thống
                    ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("HT", maNguoiSua.Value, "KH", "QLNoiDung");
                    if (ketQua.trangThai != 0)
                    {
                        //Ở chủ đề
                        ketQua = QuyenBUS.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong("CD", maNguoiSua.Value, "KH", "QLNoiDung");

                        if (ketQua.trangThai != 0)
                        {
                            //Nếu không tìm thấy ở 2 phạm vi trên => ko có quyền
                            return new KetQua(3, "Bạn không có quyền sửa khóa học");
                        }
                    }

                    //Kiểm tra người dùng có thể quyền sửa khóa học ở chủ đề hiện tại của khóa học
                    foreach (var maCD in (ketQua.ketQua as string).Split('|').Select(int.Parse))
                    {
                        ketQua = ChuDeBUS.thuocCay(khoaHoc.chuDe.ma.Value, maCD);
                        if (ketQua.trangThai != 0 || !(bool)ketQua.ketQua)
                        {
                            return new KetQua(3, "Bạn không có quyền sửa khóa học.");
                        }
                    }
                }
            }
            #endregion
            
            gan(ref khoaHoc, form);

            kiemTra(khoaHoc, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BangCapNhat bang = layBangCapNhat(khoaHoc, form.Keys.ToArray());
            if (!bang.coDuLieu())
            {
                return new KetQua(khoaHoc);
            }

            return KhoaHocDAO.capNhatTheoMa(ma, bang);
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

        public static KetQua layTheoMaNguoiDung(int maNguoiDung, LienKet lienKet = null)
        {
            LienKet lk = new LienKet() 
            {
                { "KhoaHoc", lienKet }
            };
                
            //Lấy toàn bộ khóa học mà người dùng liên quan
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaNguoiDung(maNguoiDung, lk);
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

        public static KetQua xoaTheoMa(int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            if (!coQuyen("QLNoiDung", "KH", ma, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa khóa học");
            }
            #endregion

            return KhoaHocDAO.xoaTheoMa(ma);
        }

        public static KetQua timKiemPhanTrang(int trang, int soDongMoiTrang, string where = null, string orderBy = null, LienKet lienKet = null)
        {
            return KhoaHocDAO.lay_TimKiemPhanTrang(where, orderBy, trang, soDongMoiTrang, lienKet);
        }
    }
}
