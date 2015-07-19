using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class CauHoiBUS : BUS
    {
        /// <summary>
        /// Gán
        /// </summary>
        /// <param name="cauHoi">Đối tượng câu hỏi cần gán giá trị</param>
        /// <param name="form">Các giá trị gán vào câu hỏi</param>
        public static void gan(ref CauHoiDTO cauHoi, Form form)
        {
            if (cauHoi == null)
            {
                cauHoi = new CauHoiDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "TieuDe":
                        cauHoi.tieuDe = form.layString(key);
                        break;
                    case "NoiDung":
                        cauHoi.noiDung = form.layString(key);
                        break;
                    case "MaNguoiTao":
                        cauHoi.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaChuDe":
                        cauHoi.chuDe = form.layDTO<ChuDeDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }
        
        /// <summary>
        /// Kiểm tra
        /// </summary>
        /// <param name="cauHoi">Đối tượng câu hỏi cần kiểm tra</param>
        /// <param name="truong">Các trường cần kiểm tra</param>
        /// <param name="kiemTra">Trạng thái bắt buộc kiểm tra</param>
        /// <returns>KetQua</returns>
        public static KetQua kiemTra(CauHoiDTO cauHoi, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrWhiteSpace(cauHoi.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(cauHoi.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");                
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && cauHoi.nguoiTao == null)
            {
                loi.Add("Chưa đăng nhập");
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
        
        /// <summary>
        /// Lấy bảng cập nhật
        /// </summary>
        /// <param name="cauHoi">Đối tượng câu hỏi</param>
        /// <param name="keys">Các trường cần cập nhật</param>
        /// <returns>BangCapNhat</returns>
        public static BangCapNhat layBangCapNhat(CauHoiDTO cauHoi, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add("TieuDe", cauHoi.tieuDe, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add("NoiDung", cauHoi.noiDung, 2);
                        break;
                    case "MaChuDe":
                        bangCapNhat.Add("MaChuDe", layMa_String(cauHoi.chuDe), 1);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        #region Thêm

        /// <summary>
        /// Thêm câu hỏi
        /// </summary>
        /// <param name="form">Các trường cần thêm</param>
        /// <returns>KetQua</returns>
        /// ketQua: int - Mã câu hỏi
        public static KetQua them(Form form)
        {
            #region Kiểm tra điều kiện

            int? maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua(4);
            }

            #endregion

            CauHoiDTO cauHoi = new CauHoiDTO();
            gan(ref cauHoi, form);

            KetQua ketQua = kiemTra(cauHoi);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = CauHoiDAO.them(cauHoi);
            if (ketQua.trangThai == 0)
            {
                int? maCauHoi = (ketQua.ketQua) as int?;
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiTao,
                    loaiDoiTuongBiTacDong = "CH",
                    maDoiTuongBiTacDong = maCauHoi,
                    hanhDong = layDTO<HanhDongDTO>(406),
                    duongDan = "/HoiDap/" + maCauHoi
                });
            }
            return ketQua;
        }

        #endregion

        #region Xóa

        /// <summary>
        /// Xóa theo mã
        /// </summary>
        /// <param name="ma">Mã câu hỏi</param>
        /// <param name="maNguoiXoa">Mã người xóa</param>
        /// <returns></returns>
        public static KetQua xoaTheoMa(int ma, int? maNguoiXoa)
        {
            #region Kiểm tra điều kiện

            //Lấy câu hỏi
            var ketQua = CauHoiBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Câu hỏi không tồn tại");
            }

            var cauHoi = ketQua.ketQua as CauHoiDTO;
            if (cauHoi.nguoiTao.ma != maNguoiXoa && !coQuyen("XoaCauHoi", "HD", cauHoi.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa câu hỏi");
            }

            #endregion

            ketQua = CauHoiDAO.xoaTheoMa(ma);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiXoa,
                    loaiDoiTuongBiTacDong = "CH",
                    maDoiTuongBiTacDong = ma,
                    hanhDong = layDTO<HanhDongDTO>(407),
                    duongDan = "/HoiDap/"
                });
            }
            return ketQua;
        }

        #endregion

        #region Sửa

        /// <summary>
        /// Cập nhật câu hỏi
        /// </summary>
        /// <param name="form">Các trường cần cập nhật</param>
        /// <param name="lienKet">Giá trị liên kết với các bảng liên quan</param>
        /// <returns>KetQua</returns>
        /// ketQua: CauHoiDTO
        public static KetQua capNhat(Form form, LienKet lienKet = null)
        {

            #region Kiểm tra điều kiện

            int? maNguoiSua = form.layInt("MaNguoiSua");
            int? maCauHoi = form.layInt("Ma");

            var ketQua = CauHoiDAO.layTheoMa(maCauHoi);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Câu hỏi không tồn tại");
            }

            var cauHoi = ketQua.ketQua as CauHoiDTO;
            if (cauHoi.nguoiTao.ma != maNguoiSua && !coQuyen("SuaCauHoi", "HD", cauHoi.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có quyền sửa câu hỏi này");
            }

            #endregion

            gan(ref cauHoi, form);

            ketQua = kiemTra(cauHoi, form.Keys.ToArray());
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            
            ketQua = CauHoiDAO.capNhatTheoMa(maCauHoi, layBangCapNhat(cauHoi, form.Keys.ToArray()), lienKet);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiSua,
                    loaiDoiTuongBiTacDong = "CH",
                    maDoiTuongBiTacDong = cauHoi.ma,
                    hanhDong = layDTO<HanhDongDTO>(408),
                    duongDan = "/HoiDap/" + cauHoi.ma
                });
            }

            return ketQua;
        }

        /// <summary>
        /// Duyệt hiển thị câu hỏi
        /// </summary>
        /// <param name="maCauHoi">Mã câu hỏi cần duyệt</param>
        /// <param name="trangThai">Trạng thái hiển thị câu hỏi</param>
        /// Hiển thị: true
        /// Không hiển thị: false
        /// <param name="nguoiDuyet">Mã người duyệt</param>
        /// <returns>KetQua</returns>
        public static KetQua duyetHienThiCauHoi(int maCauHoi, bool trangThai, int? nguoiDuyet)
        {
            #region Kiểm tra điều kiện

            if (!nguoiDuyet.HasValue)
            {
                return new KetQua(4, "Bạn chưa đăng nhập");
            }

            if (!QuyenBUS.coQuyen("DuyetCauHoi", "HD", maCauHoi, nguoiDuyet))
            {
                return new KetQua(3, "Bạn chưa đủ quyền để duyệt câu hỏi");
            }

            #endregion

            var ketQua =  CauHoiDAO.capNhatTheoMa_DuyetHienThi(maCauHoi, trangThai);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = nguoiDuyet,
                    loaiDoiTuongBiTacDong = "CH",
                    maDoiTuongBiTacDong = maCauHoi,
                    hanhDong = layDTO<HanhDongDTO>(409),
                    duongDan = "/HoiDap/" + maCauHoi
                });
            }

            return ketQua;
        }

        #endregion

        #region Lấy

        /// <summary>
        /// Lấy câu hỏi theo mã
        /// </summary>
        /// <param name="ma">Mã câu hỏi cần lấy</param>
        /// <param name="lienKet">Giá trị liên kết dữ liệu với các bảng liên quan</param>
        /// 
        /// <returns>KetQua</returns>
        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return CauHoiDAO.layTheoMa(ma, lienKet);
        }

        /// <summary>
        /// Lấy danh sách câu hỏi chưa được duyệt hiển thị
        /// </summary>
        /// <returns>KetQua</returns>
        public static KetQua layDanhSachChuaDuyet()
        {
            return CauHoiDAO.layCauHoi_ChuaDuyet();
        }

        /// <summary>
        /// Lấy danh sách n câu hỏi
        /// </summary>
        /// <param name="soDong">Số câu hỏi cần lấy</param>
        /// <param name="lienKet">Giá trị liên kết dữ liệu với các bảng liên quan</param>
        /// <param name="tieuChiHienThi">Tiêu chí hiển thị</param>
        /// <returns>KetQua</returns>
        public static KetQua layDanhSach(int? soDong = null, LienKet lienKet = null, string tieuChiHienThi = null)
        {
            return CauHoiDAO.lay(soDong, tieuChiHienThi, lienKet);
        }

        /// <summary>
        /// Lấy câu hỏi theo mã người tạo
        /// </summary>
        /// <param name="maNguoiTao">Mã người tạo câu hỏi</param>
        /// <param name="lienKet">Giá trị liên kết dữ liệu với các bảng liên quan</param>
        /// <returns>KetQua</returns>
        public static KetQua layTheoMaNguoiDung(int maNguoiTao, LienKet lienKet = null)
        {
            return CauHoiDAO.layTheoMaNguoiTao(maNguoiTao, lienKet);
        }

        /// <summary>
        /// Tìm các câu hỏi theo mã chủ đề
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="tuKhoa">Từ khóa cần tìm</param>
        /// <param name="lienKet">Giá trị liên kết dữ liệu với các bảng liên quan</param>
        /// <param name="cachHienThi">Tiêu chí hiển thị</param>
        /// <returns>KetQua</returns>
        public static KetQua layTheoMaChuDe_TimKiem(int ma, string tuKhoa, LienKet lienKet = null, string cachHienThi = null)
        {
            return CauHoiDAO.layTheoMaChuDe_TimKiem(ma, tuKhoa, lienKet, cachHienThi);
        }

        /// <summary>
        /// Lấy tìm kiếm
        /// </summary>
        /// <param name="tuKhoa">Từ khóa</param>
        /// <param name="lienKet">Liên kết</param>
        /// <param name="cachHienThi">Cách hiển thị</param>
        /// <returns>KetQua</returns>
        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null, string cachHienThi = null)
        {
            return CauHoiDAO.lay_TimKiem(tuKhoa, lienKet, cachHienThi);
        }

        /// <summary>
        /// Tìm kiếm phân trang câu hỏi
        /// </summary>
        /// <param name="trang">Số trang hiển thị mặc định</param>
        /// <param name="soDongMoiTrang">Số đối tượng có trong một trang</param>
        /// <param name="where">Câu điều kiện where</param>
        /// <param name="orderBy">Câu sắp xếp Order By</param>
        /// <param name="lienKet">Giá trị liên kết dữ liệu với các bảng liên quan</param>
        /// <returns>KetQua</returns>
        public static KetQua timKiemPhanTrang(int trang, int soDongMoiTrang, string where = null, string orderBy = null, LienKet lienKet = null)
        {
            return CauHoiDAO.lay_TimKiemPhanTrang(where, orderBy, trang, soDongMoiTrang, lienKet);
        }

        /// <summary>
        /// Lấy danh sách câu hỏi có liên quan tới người dùng
        /// </summary>
        /// <param name="maNguoiTao">Mã người dùng</param>
        /// <returns>KetQua</returns>
        public static KetQua layCauHoi_DanhSachMaLienQuan(int? maNguoiTao)
        {
            return CauHoiDAO.layCauHoi_DanhSachMaLienQuan(maNguoiTao);
        }

        #endregion

    }
}