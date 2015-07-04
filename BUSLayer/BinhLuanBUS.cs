using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using System.Web;
using Data;

namespace BUSLayer
{
    public class BinhLuanBUS : BUS //Nhớ bổ sung T
    {
        public static KetQua kiemTra(BinhLuanDTO binhLuan, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(binhLuan.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && binhLuan.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (coKiemTra("MaDoiTuong", truong, kiemTra) && binhLuan.doiTuong == null)
            {
                loi.Add("Đối tượng bình luận không được bỏ trống");
            }
            if (coKiemTra("LoaiDoiTuong", truong, kiemTra) && string.IsNullOrWhiteSpace(binhLuan.loaiDoiTuong))
            {
                loi.Add("Loại đối tượng bình luận không được bỏ trống");
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

        public static void gan(ref BinhLuanDTO binhLuan, Form form)
        {
            if (binhLuan == null)
            {
                binhLuan = new BinhLuanDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "NoiDung":
                        binhLuan.noiDung = form.layString(key);
                        break;
                    case "MaTapTin":
                        binhLuan.tapTin = TapTinBUS.chuyen("BinhLuan_" + form.layString("LoaiDoiTuong") + "_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "MaNguoiTao":
                        binhLuan.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaDoiTuong":
                        binhLuan.doiTuong = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "LoaiDoiTuong":
                        binhLuan.loaiDoiTuong = form.layString(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(BinhLuanDTO binhLuan, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "NoiDung":
                        bangCapNhat.Add(key, binhLuan.noiDung, 2);
                        break;
                    case "MaTapTin":
                        bangCapNhat.Add(key, binhLuan.tapTin == null ? null : binhLuan.tapTin.ma.ToString(), 1);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua them(Form form)
        {
            var binhLuan = new BinhLuanDTO();
            gan(ref binhLuan, form);

            KetQua ketQua = kiemTra(binhLuan);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BinhLuanDAO.them(binhLuan, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoDoiTuong(string loaiDoiTuong, int maDoiTuong)
        {
            return BinhLuanDAO.layTheoDoiTuong(loaiDoiTuong, maDoiTuong, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            int? ma = form.layInt("Ma");
            string loaiDoiTuong = form.layString("LoaiDoiTuong");
            if (!ma.HasValue || loaiDoiTuong == null)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = BinhLuanDAO.layTheoMa(loaiDoiTuong, ma);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BinhLuanDTO binhLuan = ketQua.ketQua as BinhLuanDTO;

            gan(ref binhLuan, form);

            ketQua = kiemTra(binhLuan, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BinhLuanDAO.capNhatTheoMa(loaiDoiTuong, ma, layBangCapNhat(binhLuan, form.Keys.ToArray()), new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoMa(string loaiDoiTuong, int ma)
        {
            return BinhLuanDAO.layTheoMa(loaiDoiTuong, ma);
        }
    }
}
