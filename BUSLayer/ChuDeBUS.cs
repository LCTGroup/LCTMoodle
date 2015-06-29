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
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrEmpty(chuDe.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (coKiemTra("MoTa", truong, kiemTra) && string.IsNullOrEmpty(chuDe.moTa))
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
            int? ma = form.layInt("Ma");
            if (!ma.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = ChuDeDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ChuDeDTO chuDe = ketQua.ketQua as ChuDeDTO;

            gan(ref chuDe, form);

            ketQua = kiemTra(chuDe, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return ChuDeDAO.capNhatTheoMa(ma, layBangCapNhat(chuDe, form.Keys.ToArray()));
        }

        public static KetQua capNhatCha(int ma, int maCha)
        {
            if (ma == maCha)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Chủ đề chuyển không hợp lệ"
                };
            }
            return ChuDeDAO.capNhatTheoMa_MaCha(ma, maCha);
        }
    }
}
