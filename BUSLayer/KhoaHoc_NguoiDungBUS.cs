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
    public class KhoaHoc_NguoiDungBUS : BUS
    {
        public static KetQua kiemTra(KhoaHoc_NguoiDungDTO thanhVien, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("KhoaHoc", truong, kiemTra) && thanhVien.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (coKiemTra("NguoiDung", truong, kiemTra) && thanhVien.nguoiDung == null)
            {
                loi.Add("Người dùng không được bỏ trống");
            }
            if (coKiemTra("TrangThai", truong, kiemTra) && (0 > thanhVien.trangThai || thanhVien.trangThai > 3))
            {
                loi.Add("Trạng thái không hợp lệ");
            }
            if (coKiemTra("NguoiThem", truong, kiemTra) && thanhVien.nguoiThem == null)
            {
                loi.Add("Người thêm không thể bỏ trống");
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

        public static KetQua dangKyThamGia(int maKhoaHoc)
        {
            //Kiểm tra xem người dùng đã đăng nhập hay chưa
            //Lấy người dùng hiện tại
            #region Lấy và kiểm tra người dùng hiện tại
            if (Session["NguoiDung"] == null || true)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }
            //Lấy mã người dùng hiện tại
            int maNguoiDung = (int)Session["NguoiDung"]; 
            #endregion

            //Lấy khóa học & kiểm tra khóa học có tồn tại hay không
            #region Lấy & kiểm tra tồn tại
            //Lấy khóa học để kiểm tra hạn đăng ký nếu có
            //và thiết lập trạng thái nếu cần
            KetQua ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            KhoaHocDTO khoaHoc = ketQua.ketQua as KhoaHocDTO; 
            #endregion

            //Kiểm tra xem người dùng đã là thành viên trong khóa học hay chưa
            #region Kiểm tra đã là thành viên
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return ketQua;
            }
            KhoaHoc_NguoiDungDTO thanhVien;
            if (ketQua.trangThai == 0) 
            {
                thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
                switch(thanhVien.trangThai)
                {
                    case 0:
                        ketQua.ketQua = new List<string>() { "Bạn đã là thành viên" };
                        break;
                    case 1:
                        ketQua.ketQua = new List<string>() { "Bạn đã đăng ký" };
                        break;
                    case 2:
                        ketQua.ketQua = new List<string>() { "Bạn đã được mời" };
                        break;
                    case 3:
                        ketQua.ketQua = new List<string>() { "Bạn đã bị chặn" };
                        break;
                    default: 
                        break;
                }
                ketQua.trangThai = 3;
                return ketQua;
            }
            #endregion

            //Kiểm tra xem là tham gia trực tiếp hay cần đăng ký
            //Kiểm tra hạn đăng ký
            #region Kiểm tra ràng buộc khóa học
            //Thiết lập trạng thái là đăng ký hay tham gia
            int loai = khoaHoc.canDangKy ? 1 : 0;

            //Kiểm tra thời hạn đăng ký
            if (loai == 1 && DateTime.Now > khoaHoc.hanDangKy)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>()
                        {
                            "Khóa học đã hết hạn đăng ký"
                        }
                };
            }
            #endregion

            //Tạo đối tượng thành viên và kiểm tra hợp lệ
            #region Tạo thành viên, kiểm tra
            thanhVien = new KhoaHoc_NguoiDungDTO()
                {
                    khoaHoc = layDTO<KhoaHocDTO>(maKhoaHoc),
                    nguoiDung = layDTO<NguoiDungDTO>(maNguoiDung),
                    trangThai = loai
                };

            ketQua = kiemTra(thanhVien, new string[] { "NguoiThem" }, false);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            } 
            #endregion

            //Thêm thành viên
            return KhoaHoc_NguoiDungDAO.them(thanhVien);
        }

        public static KetQua huyDangKy(int maKhoaHoc)
        {
            //Kiểm tra xem người dùng đã đăng nhập hay chưa
            //Lấy người dùng hiện tại
            #region Lấy và kiểm tra người dùng hiện tại
            if (Session["NguoiDung"] == null)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }
            //Lấy mã người dùng hiện tại
            int maNguoiDung = (int)Session["NguoiDung"];
            #endregion

            //Kiểm tra xem thành viên có phải đã đăng ký hay không
            #region Kiểm tra đã đăng ký
            KetQua ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            KhoaHoc_NguoiDungDTO thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
            if (thanhVien.trangThai != 1)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>() { "Bạn chưa đăng ký" }
                };
            }
            #endregion

            //Hủy đăng ký
            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua roiKhoaHoc(int maKhoaHoc)
        {
            #region Lấy người dùng hiện tại
            if (Session["NguoiDung"] == null)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }
            //Lấy mã người dùng hiện tại
            int maNguoiDung = (int)Session["NguoiDung"];
            #endregion

            //Kiểm tra người dùng có phải là thành viên của khóa học không
            #region Kiểm tra trạng thái người dùng
            KetQua ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return ketQua;
            }
            if (ketQua.trangThai == 1 ||
                (ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>() { "Người dùng không phải là thành viên" }
                };
            }
            #endregion

            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua layTheoMaKhoaHocVaMaNguoiDung(int maKhoaHoc, int maNguoiDung)
        {
            return KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua layTheoMaKhoaHocVaTrangThai(int maKhoahoc, int trangThai, LienKet lienKet = null)
        {
            return KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaTrangThai(maKhoahoc, trangThai, lienKet);
        }

        public static KetQua chapNhanDangKy(int maKhoaHoc, int maNguoiDung)
        {
            //Lấy khóa học và kiểm tra tồn tại
            #region Lấy khóa học và kiểm tra
            KetQua ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            KhoaHocDTO khoaHoc = ketQua.ketQua as KhoaHocDTO;
            #endregion

            //Kiểm tra xem thành viên có phải đang đăng ký hay không
            #region Kiểm tra trạng thái hợp lệ
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return ketQua;
            }
            if (ketQua.trangThai == 1 || 
                (ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 1)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>() { "Người dùng chưa đăng ký vào nhóm" }
                };
            }
            var thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
            #endregion

            //Kiểm tra & cập nhật trạng thái thành viên
            #region Cập nhật trạng thái thành viên
            thanhVien.nguoiThem = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?);
            thanhVien.trangThai = 0;
            ketQua = kiemTra(thanhVien, new string[] { "NguoiThem", "TrangThai" });
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            //Thay đổi trạng thái đăng ký => thành viên
            return KhoaHoc_NguoiDungDAO.capNhatTheoMaKhoaHocVaMaNguoiDung_TrangThai(thanhVien); 
            #endregion
        }

        public static KetQua tuChoiDangKy(int maKhoaHoc, int maNguoiDung)
        {
            //Lấy khóa học và kiểm tra tồn tại
            #region Lấy khóa học và kiểm tra
            KetQua ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            KhoaHocDTO khoaHoc = ketQua.ketQua as KhoaHocDTO;
            #endregion

            //Kiểm tra xem thành viên có phải đang đăng ký hay không
            #region Kiểm tra trạng thái hợp lệ
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return ketQua;
            }
            if (ketQua.trangThai == 1 ||
                (ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 1)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>() { "Người dùng chưa đăng ký vào nhóm" }
                };
            }
            #endregion

            //Thay đổi trạng thái đăng ký => thành viên
            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua chanNguoiDung(int maKhoaHoc, int maNguoiDung)
        {
            //Lấy khóa học và kiểm tra tồn tại
            #region Lấy khóa học và kiểm tra
            KetQua ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            KhoaHocDTO khoaHoc = ketQua.ketQua as KhoaHocDTO;
            #endregion

            //Kiểm tra xem người dùng có là thành viên hay không
            //Nếu là thành viên thì thay đổi trạng thái
            //Nếu đã bị chặn thì thông báo
            //Nếu chưa là thành viên thì thêm
            #region Kiểm tra trạng thái hiện tại của người dùng và xử lý
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return ketQua;
            }
            KhoaHoc_NguoiDungDTO thanhVien;
            if (ketQua.trangThai == 1)
            {
                thanhVien = new KhoaHoc_NguoiDungDTO()
                {
                    nguoiDung = layDTO<NguoiDungDTO>(maNguoiDung),
                    nguoiThem = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?),
                    khoaHoc = layDTO<KhoaHocDTO>(maKhoaHoc),
                    trangThai = 3
                };
                ketQua = kiemTra(thanhVien);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }
                return KhoaHoc_NguoiDungDAO.them(thanhVien);
            }

            thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
            thanhVien.nguoiThem = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?);
            thanhVien.trangThai = 3;
            return KhoaHoc_NguoiDungDAO.capNhatTheoMaKhoaHocVaMaNguoiDung_TrangThai(thanhVien);
            #endregion
        }

        public static KetQua huyChanNguoiDung(int maKhoaHoc, int maNguoiDung)
        {
            //Lấy khóa học và kiểm tra tồn tại
            #region Lấy khóa học và kiểm tra
            KetQua ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            #endregion

            //Kiểm tra xem thành viên có phải đang đăng ký hay không
            #region Kiểm tra trạng thái hợp lệ
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return ketQua;
            }
            if (ketQua.trangThai == 1 ||
                (ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 3)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>() { "Người dùng không bị chặn" }
                };
            }
            #endregion

            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua xoaThanhVien(int maKhoaHoc, int maNguoiDung)
        {
            //Lấy khóa học và kiểm tra tồn tại
            #region Lấy khóa học và kiểm tra
            KetQua ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            #endregion

            //Kiểm tra xem thành viên có phải đang đăng ký hay không
            #region Kiểm tra trạng thái hợp lệ
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return ketQua;
            }
            if (ketQua.trangThai == 1 ||
                (ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>() { "Người dùng không phải là thành viên" }
                };
            }
            #endregion

            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua capNhatHocVien(int maKhoaHoc, int maNguoiDung, bool laHocVien)
        {
            #region Kiểm tra người dùng hiện tại
            if (Session["NguoiDung"] == null)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }
            #endregion

            #region Kiểm tra khóa học
            var ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            #endregion

            #region Kiểm tra người dùng
            ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            var thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
            if (thanhVien.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = new List<string>() { "Người dùng chưa phải là thành viên" }
                };
            }
            #endregion

            return KhoaHoc_NguoiDungDAO.capNhatTheoMaKhoaHocVaMaNguoiDung_LaHocVien(maKhoaHoc, maNguoiDung, laHocVien);
        }
    }
}
