using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using Newtonsoft.Json;

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
            if (coKiemTra("MaNguoiDung", truong, kiemTra) && thanhVien.nguoiDung == null)
            {
                loi.Add("Người dùng không được bỏ trống");
            }
            if (coKiemTra("TrangThai", truong, kiemTra) && (0 > thanhVien.trangThai || thanhVien.trangThai > 3))
            {
                loi.Add("Trạng thái không hợp lệ");
            }
            if (coKiemTra("MaNguoiThem", truong, kiemTra) && thanhVien.nguoiThem == null)
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

        public static KetQua themDanhSachThanhVien(string dsBinhThuong, string dsTaiKhoan, string dsEmail, int maKhoaHoc, int maNguoiThem)
        {
            #region Kiểm tra điều kiện
            var ketQua = KhoaHocBUS.layTheoMa(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Khóa học không tồn tại");
            }
            var khoaHoc = ketQua.ketQua as KhoaHocDTO;

            if (!BUS.coQuyen("QLThanhVien", "KH", maKhoaHoc, maNguoiThem))
            {
                return new KetQua(3, "Cần có quyền quản lý thành viên để thực hiện chức năng này");
            }
            #endregion

            string dsMaNguoiDung = "";

            List<NguoiDungDTO> dsNguoiDungBT = JsonConvert.DeserializeObject<List<NguoiDungDTO>>(dsBinhThuong);
            if (dsNguoiDungBT.Count != 0)
            {
                ketQua = NguoiDungBUS.them(dsNguoiDungBT);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }

                dsMaNguoiDung += "," + ketQua.ketQua as string;
            }

            List<NguoiDungDTO> dsNguoiDungTK = JsonConvert.DeserializeObject<List<NguoiDungDTO>>(dsTaiKhoan);
            if (dsNguoiDungTK.Count != 0)
            {
                foreach (var nguoiDung in dsNguoiDungTK)
                {
                    ketQua = NguoiDungBUS.layTheoTenTaiKhoan(nguoiDung.tenTaiKhoan);
                    if (ketQua.trangThai != 0)
                    {
                        return ketQua;
                    }

                    dsMaNguoiDung += "," + (ketQua.ketQua as NguoiDungDTO).ma;
                }
            }

            List<NguoiDungDTO> dsNguoiDungE = JsonConvert.DeserializeObject<List<NguoiDungDTO>>(dsEmail);
            if (dsNguoiDungE.Count != 0)
            {
                foreach (var nguoiDung in dsNguoiDungE)
                {
                    ketQua = NguoiDungBUS.layTheoEmail(nguoiDung.email);
                    if (ketQua.trangThai != 0)
                    {
                        return ketQua;
                    }
                    dsMaNguoiDung += "," + (ketQua.ketQua as NguoiDungDTO).ma;
                }
            }

            dsMaNguoiDung = dsMaNguoiDung.Substring(1);
            return KhoaHoc_NguoiDungDAO.them_DanhSach(maKhoaHoc, layBangMa(dsMaNguoiDung), 0, maNguoiThem);
        }

        public static KetQua dangKyThamGia(int maKhoaHoc, int maNguoiDung)
        {
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
                switch (thanhVien.trangThai)
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

            ketQua = kiemTra(thanhVien, new string[] { "MaNguoiThem" }, false);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            } 
            #endregion

            //Thêm thành viên
            return KhoaHoc_NguoiDungDAO.them(thanhVien);
        }

        public static KetQua huyDangKy(int maKhoaHoc, int maNguoiDung)
        {
            #region Kiểm tra điều kiện
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
            #endregion

            //Hủy đăng ký
            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua roiKhoaHoc(int maKhoaHoc, int maNguoiDung)
        {
            #region Kiểm tra điều kiện
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

        public static KetQua chapNhanDangKy(int maKhoaHoc, int maNguoiDung, int maNguoiChapNhan)
        {
            #region Kiểm tra điều kiện
            //Lấy thành viên
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Người dùng chưa đăng ký vào nhóm"
                };
            }
            var thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
            
            //Kiểm tra trạng thái
            if (thanhVien.trangThai != 1)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Người dùng chưa đăng ký vào nhóm"
                };
            }

            //Kiểm tra quyền
            if (!coQuyen("QLThanhVien", "KH", maKhoaHoc, maNguoiChapNhan))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền chấp nhận đăng ký"
                };
            }
            #endregion 

            //Kiểm tra & cập nhật trạng thái thành viên
            #region Cập nhật trạng thái thành viên
            thanhVien.nguoiThem = layDTO<NguoiDungDTO>(maNguoiChapNhan);
            thanhVien.trangThai = 0;
            ketQua = kiemTra(thanhVien, new string[] { "MaNguoiThem", "TrangThai" });
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            //Thay đổi trạng thái đăng ký => thành viên
            return KhoaHoc_NguoiDungDAO.capNhatTheoMaKhoaHocVaMaNguoiDung_TrangThai(thanhVien); 
            #endregion
        }

        public static KetQua tuChoiDangKy(int maKhoaHoc, int maNguoiDung, int maNguoiTuChoi)
        {
            #region Kiểm tra điều kiện
            //Lấy thành viên
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Người dùng chưa đăng ký vào nhóm"
                };
            }
            var thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;

            //Kiểm tra trạng thái
            if (thanhVien.trangThai != 1)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Người dùng chưa đăng ký vào nhóm"
                };
            }

            //Kiểm tra quyền
            if (!coQuyen("QLThanhVien", "KH", maKhoaHoc, maNguoiTuChoi))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền từ chối đăng ký"
                };
            }
            #endregion 

            //Thay đổi trạng thái đăng ký => thành viên
            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua chanNguoiDung(int maKhoaHoc, int maNguoiDung, int maNguoiChan)
        {
            //Kiểm tra quyền
            if (!coQuyen("QLThanhVien", "KH", maKhoaHoc, maNguoiChan))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền chặn thành viên"
                };
            }

            //Kiểm tra xem người dùng có là thành viên hay không
            //Nếu là thành viên thì thay đổi trạng thái
            //Nếu đã bị chặn thì thông báo
            //Nếu chưa là thành viên thì thêm
            #region Kiểm tra trạng thái hiện tại của người dùng và xử lý
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return new KetQua()
                    {
                        trangThai = 2,
                        ketQua = "Chặn người dùng thất bại"
                    };
            }

            KhoaHoc_NguoiDungDTO thanhVien;
            if (ketQua.trangThai == 1)
            {
                //Trường hợp chưa có trong nhóm => thêm chặn
                thanhVien = new KhoaHoc_NguoiDungDTO()
                {
                    nguoiDung = layDTO<NguoiDungDTO>(maNguoiDung),
                    nguoiThem = layDTO<NguoiDungDTO>(System.Web.HttpContext.Current.Session["NguoiDung"] as int?),
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

            //Trường hợp đã có => cập nhật
            thanhVien = ketQua.ketQua as KhoaHoc_NguoiDungDTO;
            thanhVien.nguoiThem = layDTO<NguoiDungDTO>(System.Web.HttpContext.Current.Session["NguoiDung"] as int?);
            thanhVien.trangThai = 3;
            return KhoaHoc_NguoiDungDAO.capNhatTheoMaKhoaHocVaMaNguoiDung_TrangThai(thanhVien);
            #endregion
        }

        public static KetQua huyChanNguoiDung(int maKhoaHoc, int maNguoiDung, int maNguoiHuy)
        {
            //Kiểm tra quyền
            if (!coQuyen("QLThanhVien", "KH", maKhoaHoc, maNguoiHuy))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền hủy chặn thành viên"
                };
            }
            
            //Kiểm tra trạng thái hợp lệ
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return new KetQua() 
                {
                    trangThai = 2,
                    ketQua = "Hủy chặn người dùng thất bại"
                };
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

            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua xoaThanhVien(int maKhoaHoc, int maNguoiDung, int maNguoiXoa)
        {
            //Kiểm tra quyền
            if (!coQuyen("QLThanhVien", "KH", maKhoaHoc, maNguoiXoa))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xóa thành viên"
                };
            }

            //Kiểm tra xem thành viên có phải đang đăng ký hay không
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai > 1)
            {
                return new KetQua()
                    {
                        trangThai = 2,
                        ketQua = "Xóa người dùng thất bại"
                    };
            }

            if (ketQua.trangThai == 1 ||
                (ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Người dùng không phải là thành viên"
                };
            }

            return KhoaHoc_NguoiDungDAO.xoaTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }

        public static KetQua capNhatHocVien(int maKhoaHoc, int maNguoiDung, bool laHocVien, int maNguoiSua)
        {
            //Kiểm tra quyền
            if (!coQuyen("QLThanhVien", "KH", maKhoaHoc, maNguoiSua))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền thay đổi học viên"
                };
            }

            #region Kiểm tra người dùng
            var ketQua = KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            if ((ketQua.ketQua as KhoaHoc_NguoiDungDTO).trangThai != 0)
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
