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
    public class KhoaHocBUS : BUS
    {
        public static KetQua kiemTra(KhoaHocDTO khoaHoc, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrEmpty(khoaHoc.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (coKiemTra("MoTa", truong, kiemTra) && string.IsNullOrEmpty(khoaHoc.moTa))
            {
                loi.Add("Mô tả không được bỏ trống");
            }
            if (coKiemTra("ChuDe", truong, kiemTra) && khoaHoc.chuDe == null)
            {
                loi.Add("Chủ đề không được bỏ trống");
            }
            if (coKiemTra("HinhDaiDien", truong, kiemTra) && khoaHoc.hinhDaiDien == null)
            {
                loi.Add("Hình đại diện không được bỏ trống");
            }
            if (coKiemTra("NguoiTao", truong, kiemTra) && khoaHoc.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (coKiemTra("CheDoRiengTu", truong, kiemTra) && khoaHoc.cheDoRiengTu == null)
            {
                loi.Add("Chế độ riêng tư không được bỏ trống");
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
            KetQua ketQua = TapTinBUS.chuyen("KhoaHoc_HinhDaiDien", layInt(form, "HinhDaiDien"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            //Chưa xử lý quản lý
            KhoaHocDTO khoaHoc  = new KhoaHocDTO()
            {
                ten = layString(form, "Ten"),
                moTa = layString(form, "MoTa"),
                chuDe = layDTO<ChuDeDTO>(form, "ChuDe"),
                nguoiTao = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?),
                hinhDaiDien = ketQua.ketQua as TapTinDTO,
                canDangKy = layBool(form, "CanDangKy"),
                phiThamGia = layInt(form, "PhiThamGia"),
                cheDoRiengTu = CheDoRiengTu.lay(layString(form, "CheDoRiengTu"))
            };

            if (layBool(form, "CoHan"))
            {
                khoaHoc.thoiDiemHetHan = layDateTime(form, "ThoiDiemHetHan");
            }

            if (khoaHoc.canDangKy && layBool(form, "CoHanDangKy"))
            {
                khoaHoc.hanDangKy = layDateTime(form, "HanDangKy");
            }

            ketQua = kiemTra(khoaHoc);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return KhoaHocDAO.them(khoaHoc);
        }

        public static KetQua layTheoMa(int ma)
        {
            return KhoaHocDAO.layTheoMa(ma);
        }
    }
}
