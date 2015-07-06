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
    public class BinhLuanBaiVietDienDanBUS : BUS
    {
        public static KetQua kiemTra(BinhLuanBaiVietDienDanDTO binhLuan, string[] truong = null, bool kiemTra = true)
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
            if (coKiemTra("MaBaiVietDienDan", truong, kiemTra) && binhLuan.baiVietDienDan == null)
            {
                loi.Add("Bài viết diễn đàn không được bỏ trống");
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

        public static void gan(ref BinhLuanBaiVietDienDanDTO binhLuan, Form form)
        {
            if (binhLuan == null)
            {
                binhLuan = new BinhLuanBaiVietDienDanDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "NoiDung":
                        binhLuan.noiDung = form.layString(key);
                        break;
                    case "MaTapTin":
                        binhLuan.tapTin = TapTinBUS.chuyen("BinhLuanBaiVietDienDan_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "MaNguoiTao":
                        binhLuan.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaBaiVietDienDan":
                        binhLuan.baiVietDienDan = form.layDTO<BaiVietDienDanDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(BinhLuanBaiVietDienDanDTO binhLuan, string[] keys)
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
            var binhLuan = new BinhLuanBaiVietDienDanDTO();
            gan(ref binhLuan, form);

            KetQua ketQua = kiemTra(binhLuan);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BinhLuanBaiVietDienDanDAO.them(binhLuan, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoMaBaiVietDienDan(int maBaiVietDienDan)
        {
            return BinhLuanBaiVietDienDanDAO.layTheoMaBaiVietDienDan(maBaiVietDienDan, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
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

            KetQua ketQua = BinhLuanBaiVietDienDanDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BinhLuanBaiVietDienDanDTO binhLuan = ketQua.ketQua as BinhLuanBaiVietDienDanDTO;

            gan(ref binhLuan, form);

            ketQua = kiemTra(binhLuan, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BangCapNhat bang = layBangCapNhat(binhLuan, form.Keys.ToArray());
            if (!bang.coDuLieu())
            {
                return new KetQua(1);
            }

            return BinhLuanBaiVietDienDanDAO.capNhatTheoMa(ma, bang, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoMa(int ma)
        {
            return BinhLuanBaiVietDienDanDAO.layTheoMa(ma);
        }

        public static KetQua capNhatDiem(int ma, int diem, int maNguoiSua)
        {
            #region Kiểm tra điều kiện
            //Lấy bình luận, bài viết
            var ketQua = BinhLuanBaiVietDienDanDAO.layTheoMa(ma, new LienKet()
                {
                    "BaiVietDienDan"
                });
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bình luận không tồn tại");
            }
            var binhLuan = ketQua.ketQua as BinhLuanBaiVietDienDanDTO;

            //Kiểm tra quyền
            if (!coQuyen("DD_QLDiem", "KH", binhLuan.baiVietDienDan.khoaHoc.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có quyền cho điểm bình luận");
            }
            #endregion

            return BinhLuanBaiVietDienDanDAO.capNhatTheoMa_Diem(ma, diem);
        }
    }
}
