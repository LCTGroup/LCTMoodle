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

        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("BinhLuan_" + layString(form, "Loai") + "_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            var binhLuan = new BinhLuanDTO()
            {
                noiDung = layString(form, "NoiDung"),
                tapTin = ketQua.ketQua as TapTinDTO,
                nguoiTao = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?),
                doiTuong = layDTO<DTO>(form, "DoiTuong"),
                loaiDoiTuong = layString(form, "Loai")
            };

            ketQua = kiemTra(binhLuan);

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
