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
        public static KetQua kiemTra(BinhLuanDTO binhLuan)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(binhLuan.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (binhLuan.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (binhLuan.doiTuong == null)
            {
                loi.Add("Đối tượng bình luận không được bỏ trống");
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
    }
}
