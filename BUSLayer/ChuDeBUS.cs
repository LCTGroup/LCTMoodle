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
    public class ChuDeBUS : BUS
    {
        public static KetQua kiemTra(ChuDeDTO chuDe, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrWhiteSpace(chuDe.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (coKiemTra("MoTa", truong, kiemTra) && string.IsNullOrWhiteSpace(chuDe.moTa))
            {
                loi.Add("Mô tả không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && chuDe.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
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

        public static void gan(ref ChuDeDTO chuDe, Form form)
        {
            if (chuDe == null)
            {
                chuDe = new ChuDeDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "Ten":
                        chuDe.ten = form.layString(key);
                        break;
                    case "MoTa":
                        chuDe.moTa = form.layString(key);
                        break;
                    case "MaHinhDaiDien":
                        chuDe.hinhDaiDien = TapTinBUS.chuyen("ChuDe_HinhDaiDien", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "MaNguoiTao":
                        chuDe.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaCha":
                        chuDe.cha = form.layDTO<ChuDeDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(ChuDeDTO chuDe, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "Ten":
                        bangCapNhat.Add(key, chuDe.ten, 2);
                        break;
                    case "MoTa":
                        bangCapNhat.Add(key, chuDe.moTa, 2);
                        break;
                    case "MaHinhDaiDien":
                        bangCapNhat.Add(key, chuDe.hinhDaiDien == null ? null : chuDe.hinhDaiDien.ma.ToString(), 1);
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
            var maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã người tạo không được bỏ trống"
                };
            }

            //Lấy chủ đề cha
            var maCha = form.layInt("MaCha");
            if (!maCha.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã cha không được bỏ trống"
                };
            }
            
            if (!coQuyen("QLNoiDung", "CD", maCha.Value, maNguoiTao))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền tạo chủ đề"
                };
            }
            #endregion

            ChuDeDTO chuDe = new ChuDeDTO();

            gan(ref chuDe, form);

            KetQua ketQua = kiemTra(chuDe);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return ChuDeDAO.them(chuDe);
        }

        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return ChuDeDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua layTheoMaCha(int maCha, LienKet lienKet = null)
        {
            return ChuDeDAO.layTheoMaCha(maCha, lienKet);
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null)
        {
            return ChuDeDAO.lay_TimKiem(tuKhoa, lienKet);
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người sửa
            var maNguoiSua = form.layInt("MaNguoiSua");
            if (!maNguoiSua.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Người sửa không được bỏ trống"
                };
            }

            //Lấy chủ đề
            var ma = form.layInt("Ma");
            if (!ma.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã chủ đề không được bỏ trống"
                };
            }
            var ketQua = ChuDeDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                    {
                        trangThai = 3,
                        ketQua = "Chủ đề không tồn tại"
                    };
            }
            var chuDe = ketQua.ketQua as ChuDeDTO;

            //Kiểm tra quyền
            if (chuDe.nguoiTao.ma != maNguoiSua && !coQuyen("QLNoiDung", "CD", chuDe.ma.Value, maNguoiSua))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa chủ đề"
                };
            }
            #endregion

            gan(ref chuDe, form);

            ketQua = kiemTra(chuDe, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }


            BangCapNhat bang = layBangCapNhat(chuDe, form.Keys.ToArray());
            if (!bang.coDuLieu())
            {
                return new KetQua(1);
            }

            return ChuDeDAO.capNhatTheoMa(ma, bang);
        }

        public static KetQua capNhatCha(int ma, int maCha, int maNguoiSua)
        {
            #region Kiểm tra điều kiện
            //Chính nó
            if (ma == maCha)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Chủ đề chuyển không hợp lệ"
                };
            }

            //Lấy chủ đề
            var ketQua = ChuDeDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Chủ đề không tồn tại"
                };
            }
            var chuDe = ketQua.ketQua as ChuDeDTO;

            //Kiểm tra quyền
            if (chuDe.nguoiTao.ma != maNguoiSua && !coQuyen("QLNoiDung", "CD", chuDe.ma.Value, maNguoiSua))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa chủ đề"
                };
            }
            #endregion

            return ChuDeDAO.capNhatTheoMa_MaCha(ma, maCha);
        }

        public static KetQua xoaTheoMa(int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy chủ đề
            var ketQua = ChuDeDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Chủ đề không tồn tại"
                };
            }
            var chuDe = ketQua.ketQua as ChuDeDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLNoiDung", "CD", chuDe.ma.Value, maNguoiXoa))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xóa chủ đề"
                };
            }
            #endregion

            return ChuDeDAO.xoaTheoMa(ma);
        }

        public static KetQua timKiemPhanTrang(int trang, int soDongMoiTrang, string where = null, string orderBy = null, LienKet lienKet = null)
        {
            return ChuDeDAO.lay_TimKiemPhanTrang(where, orderBy, trang, soDongMoiTrang, lienKet);
        }
    }
}
