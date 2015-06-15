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
            if (coKiemTra("TrangThai", truong, kiemTra) && 1 <= thanhVien.trangThai && thanhVien.trangThai <= 4)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loai">
        /// Nếu loại = 0 => Sẽ là đăng ký hoặc tham gia tùy theo khóa học thiết lập
        /// 1: Tham gia --- 2: Đăng ký --- 3: Mời --- 4: Chặn
        /// </param>
        /// <param name="maNguoiDung">
        /// Trường hợp trạng thái = 1, 2 (tham gia, đăng ký) => không cần truyền
        /// Trường hợp trạng thái = 3, 4 (mời, chặn) => bắt buộc truyền
        /// </param>
        /// <returns></returns>
        public static KetQua them(int maKhoaHoc, int loai = 0, int? maNguoiDung = null)
        {
            KhoaHoc_NguoiDungDTO thanhVien;

            if (loai == 0 || loai == 1 || loai == 2)
            {
                //Lấy khóa học để kiểm tra hạn đăng ký nếu có
                //và thiết lập trạng thái nếu cần
                KetQua ketQua = KhoaHocDAO.layTheoMa(maKhoaHoc);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }
                KhoaHocDTO khoaHoc = ketQua.ketQua as KhoaHocDTO;

                //Thiết lập trạng thái nếu cần
                if (loai == 0)
                {
                    loai = khoaHoc.canDangKy ? 2 : 1;
                }

                //Kiểm tra thời hạn
                if (loai == 2 && DateTime.Now > khoaHoc.hanDangKy)
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

                //Tạo đối tượng thành viên
                thanhVien = new KhoaHoc_NguoiDungDTO()
                {
                    khoaHoc = layDTO<KhoaHocDTO>(maKhoaHoc),
                    nguoiDung = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?),
                    trangThai = loai
                };

                ketQua = kiemTra(thanhVien, new string[] { "NguoiThem" }, false);

                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }
            }
            else
            {
                //Nếu đã trong nhóm thì ko thể mời
                if (loai == 3 && KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung).trangThai == 0)
                {
                    return new KetQua()
                    {
                        trangThai = 3,
                        ketQua = new List<string>()
                        {
                            "Thành viên hiện đang trong nhóm"
                        }
                    };
                }

                thanhVien = new KhoaHoc_NguoiDungDTO()
                {
                    khoaHoc = layDTO<KhoaHocDTO>(maKhoaHoc),
                    nguoiDung = layDTO<NguoiDungDTO>(maNguoiDung),
                    trangThai = loai,
                    nguoiThem = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?)
                };

                KetQua ketQua = kiemTra(thanhVien);

                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }
            }

            return KhoaHoc_NguoiDungDAO.them(thanhVien);
        }

        public static KetQua layTheoMaKhoaHocVaMaNguoiDung(int maKhoaHoc, int maNguoiDung)
        {
            return KhoaHoc_NguoiDungDAO.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
        }
    }
}
