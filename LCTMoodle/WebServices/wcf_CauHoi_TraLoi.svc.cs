using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using BUSLayer;
using DTOLayer;
using Helpers;
using System.IO;
using System.Drawing;

namespace LCTMoodle.WebServices
{
    public class wcf_TraLoi : Iwcf_CauHoi_TraLoi
    {
        private const string _Loai = "NguoiDung_HinhDaiDien";

        /// <summary>
        /// Webservice lấy hình ảnh
        /// </summary>
        /// <param name="_Ten"></param>
        /// <returns>byte[]</returns>
        public byte[] layHinhAnh(string ten)
        {
            string _DuongDan = TapTinHelper.layDuongDan(_Loai, ten);

            if (File.Exists(@_DuongDan))
            {
                Image img = Image.FromFile(@_DuongDan);
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// Webservice lấy ảnh và chỉ số
        /// </summary>
        /// <param name="_ChiSo"></param>
        /// <param name="_Ten"></param>
        /// <returns>clientmodel_HinhAnh</returns>
        public clientmodel_HinhAnh layHinhAnhChiSo(int chiSo, string ten)
        {
            string _DuongDan = TapTinHelper.layDuongDan(_Loai, ten);
            clientmodel_HinhAnh cm_HinhAnh = new clientmodel_HinhAnh();

            cm_HinhAnh.chiSo = chiSo;

            if (File.Exists(@_DuongDan))
            {
                Image img = Image.FromFile(@_DuongDan);
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    cm_HinhAnh.hinhAnh = ms.ToArray();
                }
            }

            return cm_HinhAnh;
        }

        public clientmodel_CauHoi_TraLoi layTheoMa(int maTraLoi)
        {
            KetQua ketQua = TraLoiBUS.layTheoMa(maTraLoi, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            TraLoiDTO dto_TraLoi = ketQua.ketQua as TraLoiDTO;
            clientmodel_CauHoi_TraLoi cm_TraLoi = new clientmodel_CauHoi_TraLoi();

            if(ketQua.trangThai == 0)
            {
                if(dto_TraLoi.ma != null)
                {
                    cm_TraLoi.ma = dto_TraLoi.ma.Value;
                }
                
                if(dto_TraLoi.noiDung != null)
                {
                    cm_TraLoi.noiDung = dto_TraLoi.noiDung;
                }

                if(dto_TraLoi.nguoiTao.tenTaiKhoan != null)
                {
                    cm_TraLoi.nguoiTao = dto_TraLoi.nguoiTao.tenTaiKhoan;
                }

                if(dto_TraLoi.thoiDiemTao != null)
                {
                    cm_TraLoi.ngayTao = dto_TraLoi.thoiDiemTao.Value;
                }

                if(dto_TraLoi.thoiDiemCapNhat != null)
                {
                    cm_TraLoi.ngayCapNhat = dto_TraLoi.thoiDiemTao.Value;
                }

                if(dto_TraLoi.nguoiTao.hinhDaiDien.ma != null && dto_TraLoi.nguoiTao.hinhDaiDien.duoi != null)
                {
                    cm_TraLoi.hinhAnh = dto_TraLoi.nguoiTao.hinhDaiDien.ma.Value + dto_TraLoi.nguoiTao.hinhDaiDien.duoi;
                }
            }
            return cm_TraLoi;
        }

        /// <summary>
        /// Webservice lấy trả lời theo mã câu hỏi
        /// </summary>
        /// <param name="ma"></param>
        /// <returns>List<clientmodel_CauHoi_TraLoi></returns>
        public List<clientmodel_CauHoi_TraLoi> layTheoMaCauHoi(int ma)
        {
            KetQua ketQua = TraLoiBUS.layTheoMaCauHoi(ma, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_CauHoi_TraLoi> lst_TraLoi = new List<clientmodel_CauHoi_TraLoi>();

            if(ketQua.trangThai == 0)
            {
                foreach(var traLoi in ketQua.ketQua as List<TraLoiDTO>)
                {
                    if(traLoi.ma != null)
                    {
                        lst_TraLoi.Add(new clientmodel_CauHoi_TraLoi()
                        {
                            ma = traLoi.ma.Value,
                            duyet = traLoi.duyet,
                        });
                    }

                    if(traLoi.noiDung != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].noiDung = traLoi.noiDung;
                    }

                    if(traLoi.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].nguoiTao = traLoi.nguoiTao.tenTaiKhoan;
                    }

                    if(traLoi.thoiDiemTao != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].ngayTao = traLoi.thoiDiemTao.Value;
                    }

                    if(traLoi.thoiDiemCapNhat != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].ngayCapNhat = traLoi.thoiDiemCapNhat.Value;
                    }


                    if(traLoi.nguoiTao.hinhDaiDien.ma != null && traLoi.nguoiTao.hinhDaiDien.duoi != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].hinhAnh = traLoi.nguoiTao.hinhDaiDien.ma.Value + traLoi.nguoiTao.hinhDaiDien.duoi;
                    }
                }
            }

            return lst_TraLoi;
        }

        /// <summary>
        /// Webservice thêm trả lời
        /// </summary>
        /// <param name="maNguoiTao"></param>
        /// <param name="maCauHoi"></param>
        /// <param name="noiDung"></param>
        /// <returns>clientmodel_ThongBao</returns>
        public clientmodel_ThongBao themTraLoi(int maNguoiTao, int maCauHoi, string noiDung)
        {
            Form form = new Form()
            {
                {"MaNguoiTao",maNguoiTao.ToString()},
                {"MaCauHoi",maCauHoi.ToString()},
                {"NoiDung",noiDung.ToString()},
            };

            KetQua ketQua = TraLoiBUS.them(form);
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();

            if(ketQua.trangThai == 0)
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = "Trả lời đã được đưa vào danh sách duyệt";
            }
            else
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = ketQua.ketQua as string;
            }
            return cm_ThongBao;
        }

        /// <summary>
        /// Webservice xóa trả lời
        /// </summary>
        /// <param name="maNguoiXoa"></param>
        /// <param name="maTraLoi"></param>
        /// <returns>clientmodel_ThongBao</returns>
        public clientmodel_ThongBao xoaTraLoi(int maNguoiXoa, int maTraLoi)
        {
            KetQua ketQua = TraLoiBUS.xoaTheoMa(maTraLoi, maNguoiXoa);
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();

            if(ketQua.trangThai == 0)
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = "Xóa trả lời thành công";
            }
            else
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = ketQua.ketQua as string;
            }
            return cm_ThongBao;
        }

        /// <summary>
        /// Webservice sửa trả lời
        /// </summary>
        /// <param name="maNguoiSua"></param>
        /// <param name="maTraLoi"></param>
        /// <param name="noiDung"></param>
        /// <returns>clientmodel_ThongBao</returns>
        public clientmodel_ThongBao suaTraLoi(int maNguoiSua, int maTraLoi, string noiDung)
        {
            Form form = new Form()
            {
                {"MaNguoiSua",maNguoiSua.ToString()},
                {"Ma",maTraLoi.ToString()},
                {"NoiDung",noiDung.ToString()},
            };

            KetQua ketQua = TraLoiBUS.capNhat(form);
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();

            if(ketQua.trangThai == 0)
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = "Cập nhật trả lời thành công";
            }
            else
            {
                cm_ThongBao.trangThai = ketQua.trangThai;
                cm_ThongBao.thongBao = ketQua.ketQua as string;
            }

            return cm_ThongBao;
        }
    }
}
