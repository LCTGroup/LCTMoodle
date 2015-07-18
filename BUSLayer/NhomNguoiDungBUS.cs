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
    public class NhomNguoiDungBUS : BUS
    {
        public static KetQua kiemTra(NhomNguoiDungDTO nhomNguoiDung, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrWhiteSpace(nhomNguoiDung.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (coKiemTra("PhamVi", truong, kiemTra) && nhomNguoiDung.phamVi == null)
            {
                loi.Add("Phạm vi không được bỏ trống");
            }
            if (coKiemTra("MaDoiTuong", truong, kiemTra) && nhomNguoiDung.phamVi != "HT" && nhomNguoiDung.doiTuong == null)
            {
                loi.Add("Đối tượng không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && nhomNguoiDung.nguoiTao == null)
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

        public static void gan(ref NhomNguoiDungDTO nhomNguoiDung, Form form)
        {
            if (nhomNguoiDung == null)
            {
                nhomNguoiDung = new NhomNguoiDungDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "Ten":
                        nhomNguoiDung.ten = form.layString(key);
                        break;
                    case "MoTa":
                        nhomNguoiDung.moTa = form.layString(key);
                        break;
                    case "PhamVi":
                        nhomNguoiDung.phamVi = form.layString(key);
                        break;
                    case "MaDoiTuong":
                        nhomNguoiDung.doiTuong = form.layDTO<DTO>(key);
                        break;
                    case "MaNguoiTao":
                        nhomNguoiDung.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(NhomNguoiDungDTO nhom, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "Ten":
                        bangCapNhat.Add(key, nhom.ten, 2);
                        break;
                    case "MoTa":
                        bangCapNhat.Add(key, nhom.moTa, 2);
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
                return new KetQua(3, "Người tạo không thể bỏ trống");
            }

            var phamVi = form.layString("PhamVi");
            var maDoiTuong = form.layInt("MaDoiTuong");
            if (phamVi == null || !maDoiTuong.HasValue)
            {
                return new KetQua(3, "Đối tượng không thể bỏ trống");
            }

            if (!coQuyen("QLQuyen", phamVi, maDoiTuong.Value, maNguoiTao))
            {
                return new KetQua(3, "Bạn không có quyền thêm nhóm người dùng");
            }
            #endregion

            NhomNguoiDungDTO nhomNguoiDung = new NhomNguoiDungDTO();

            gan(ref nhomNguoiDung, form);

            KetQua ketQua = kiemTra(nhomNguoiDung);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return NhomNguoiDungDAO.them(nhomNguoiDung);
        }

        public static KetQua capNhat(Form form)
        {
            #region Kiểm tra điều kiện
            var maNguoiSua = form.layInt("MaNguoiSua");
            var ma = form.layInt("Ma");
            var phamVi = form.layString("PhamVi");

            //Lấy nhóm
            var ketQua = NhomNguoiDungDAO.layTheoMa(phamVi, ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Nhóm người dùng không tồn tại");
            }
            var nhom = ketQua.ketQua as NhomNguoiDungDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLQuyen", phamVi, phamVi == "HT" ? 0 : nhom.doiTuong.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có quyền sửa nhóm người dùng");
            }
            #endregion

            gan(ref nhom, form);

            ketQua = kiemTra(nhom, form.Keys.ToArray());
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            var bang = layBangCapNhat(nhom, form.Keys.ToArray());

            if (!bang.coDuLieu())
            {
                return new KetQua(nhom);
            }

            return NhomNguoiDungDAO.capNhatTheoMa(phamVi, ma, bang);
        }

        public static KetQua layTheoMaDoiTuong(string phamVi, int maDoiTuong)
        {
            return NhomNguoiDungDAO.layTheoMaDoiTuong(phamVi, maDoiTuong);
        }

        public static KetQua xoaTheoMa(string phamVi, int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy nhóm người dùng
            var ketQua = NhomNguoiDungDAO.layTheoMa(phamVi, ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Nhóm người dùng không tồn tại");
            }
            var nhomNguoiDung = ketQua.ketQua as NhomNguoiDungDTO;

            //Kiểm tra nhóm người dùng có phải nhóm người dùng đặc biệt không
            if (nhomNguoiDung.giaTri != null)
            {
                return new KetQua(3, "Khôn thể xóa nhóm người dùng này");
            }

            if (!coQuyen("QLQuyen", phamVi, nhomNguoiDung.doiTuong == null ? 0 : nhomNguoiDung.doiTuong.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa nhóm người dùng");
            }
            #endregion

            return NhomNguoiDungDAO.xoaTheoMa(phamVi, ma);
        }

        public static KetQua themNhomMacDinh(string phamVi, int maDoiTuong)
        {
            var ketQua = NhomNguoiDungDAO.them_MacDinh(phamVi, maDoiTuong);
            if (ketQua.trangThai > 1)
            {
                return new KetQua(3, "Thêm nhóm người dùng thất bại");
            }

            return NhomNguoiDungDAO.layTheoMaDoiTuong(phamVi, maDoiTuong);
        }

        public static KetQua layTheoMa(string phamVi, int ma)
        {
            return NhomNguoiDungDAO.layTheoMa(phamVi, ma);
        }

        /// <summary>
        /// Lấy danh sách nhóm người dùng mà người dùng thuộc
        /// </summary>
        /// <param name="maNguoiDung">Mã người dùng cần lấy</param>
        /// <returns>
        /// Mảng gồm 3 phần tử: 0 - Hệ thống, 1 - Chủ đề, 2 - Khóa học ---
        /// Mỗi phần tử là 1 Dictionary với key là mã đối tượng, list nhóm
        /// </returns>
        public static KetQua layTheoMaNguoiDung(int maNguoiDung)
        {
            var dsNhom = new object[3];
            Dictionary<int, List<NhomNguoiDungDTO>> ds;

            //Lấy nhóm người dùng hệ thống
            var ketQua = NhomNguoiDungDAO.layTheoMaNguoiDung("HT", maNguoiDung);
            if (ketQua.trangThai == 0)
            {
                ds = new Dictionary<int, List<NhomNguoiDungDTO>>();
                ds.Add(0, ketQua.ketQua as List<NhomNguoiDungDTO>);

                dsNhom[0] = ds;
            }

            //Lấy nhóm người dùng chủ đề
            ketQua = NhomNguoiDungDAO.layTheoMaNguoiDung("CD", maNguoiDung);
            if (ketQua.trangThai == 0)
            {
                ds = new Dictionary<int, List<NhomNguoiDungDTO>>();
                var dsNhomKH = ketQua.ketQua as List<NhomNguoiDungDTO>;
                foreach (var nhom in dsNhomKH)
                {
                    var ma = nhom.doiTuong.ma.Value;
                    if (!ds.ContainsKey(ma))
                    {
                        ds.Add(ma, new List<NhomNguoiDungDTO>());
                    }

                    ds[ma].Add(nhom);
                }

                dsNhom[1] = ds;
            }

            //Lấy nhóm người dùng hệ thống
            ketQua = NhomNguoiDungDAO.layTheoMaNguoiDung("CD", maNguoiDung);
            if (ketQua.trangThai == 0)
            {
                ds = new Dictionary<int, List<NhomNguoiDungDTO>>();
                var dsNhomCD = ketQua.ketQua as List<NhomNguoiDungDTO>;
                foreach (var nhom in dsNhomCD)
                {
                    var ma = nhom.doiTuong.ma.Value;
                    if (!ds.ContainsKey(ma))
                    {
                        ds.Add(ma, new List<NhomNguoiDungDTO>());
                    }

                    ds[ma].Add(nhom);
                }

                dsNhom[2] = ds;
            }

            return new KetQua(dsNhom);
        }
    }
}
