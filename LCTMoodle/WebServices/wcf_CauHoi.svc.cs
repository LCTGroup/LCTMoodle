using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using BUSLayer;
using DTOLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_CauHoi" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_CauHoi.svc or wcf_CauHoi.svc.cs at the Solution Explorer and start debugging.
    public class wcf_CauHoi : Iwcf_CauHoi
    {
        private const string _Loai = "NguoiDung_HinhDaiDien";

       /// <summary>
        /// Webservice lấy hình ảnh
       /// </summary>
       /// <param name="ten"></param>
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
        /// <param name="chiSo"></param>
        /// <param name="ten"></param>
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

        /// <summary>
        /// Webservice lấy câu hỏi theo mã
        /// </summary>
        /// <param name="ma"></param>
        /// <returns>clientmodel_CauHoi</returns>
        public clientmodel_CauHoi layTheoMa(int ma)
        {
            KetQua ketQua = CauHoiBUS.layTheoMa(ma, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            CauHoiDTO dto_CauHoi = ketQua.ketQua as CauHoiDTO;
            clientmodel_CauHoi cm_CauHoi = new clientmodel_CauHoi();

            if(ketQua.trangThai == 0)
            {
                if(dto_CauHoi.ma != null)
                {
                    cm_CauHoi.ma = dto_CauHoi.ma.Value;
                }

                if(dto_CauHoi.tieuDe != null)
                {
                    cm_CauHoi.tieuDe = dto_CauHoi.tieuDe;
                }

                if(dto_CauHoi.noiDung != null)
                {
                    cm_CauHoi.noiDung = dto_CauHoi.noiDung;
                }

                if(dto_CauHoi.nguoiTao.tenTaiKhoan != null)
                {
                    cm_CauHoi.nguoiTao = dto_CauHoi.nguoiTao.tenTaiKhoan;
                }

                if(dto_CauHoi.soLuongTraLoi != null)
                {
                    cm_CauHoi.soTraLoi = dto_CauHoi.soLuongTraLoi.Value;
                }

                if(dto_CauHoi.thoiDiemTao != null)
                {
                    cm_CauHoi.ngayTao = dto_CauHoi.thoiDiemTao.Value;
                }

                if(dto_CauHoi.thoiDiemCapNhat != null)
                {
                    cm_CauHoi.ngayCapNhat = dto_CauHoi.thoiDiemCapNhat.Value;
                }

                if(dto_CauHoi.nguoiTao.hinhDaiDien.ma != null && dto_CauHoi.nguoiTao.hinhDaiDien.duoi != null)
                {
                    cm_CauHoi.hinhAnh = dto_CauHoi.nguoiTao.hinhDaiDien.ma.Value + dto_CauHoi.nguoiTao.hinhDaiDien.duoi;
                }
            }
            return cm_CauHoi;
        }       

        /// <summary>
        /// Webservice lấy danh sách câu hỏi
        /// </summary>
        /// <param name="soPT"></param>
        /// <returns>List<clientmodel_CauHoi></returns>
        public List<clientmodel_CauHoi> layMoiNhat(int soPT)
        {
            KetQua ketQua = CauHoiBUS.layDanhSach(soPT, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_CauHoi> lst_CauHoi = new List<clientmodel_CauHoi>();

            if (ketQua.trangThai == 0)
            {
                foreach(var cauHoi in ketQua.ketQua as List<CauHoiDTO>)
                {
                    if(cauHoi.ma != null)
                    {
                        lst_CauHoi.Add(new clientmodel_CauHoi()
                        {
                            ma = cauHoi.ma.Value,
                        });
                    }

                    if (cauHoi.tieuDe != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].tieuDe = cauHoi.tieuDe;
                    }

                    if (cauHoi.noiDung != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].noiDung = cauHoi.noiDung;
                    }

                    if (cauHoi.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].nguoiTao = cauHoi.nguoiTao.tenTaiKhoan;
                    }

                    if (cauHoi.soLuongTraLoi != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].soTraLoi = cauHoi.soLuongTraLoi.Value;
                    }

                    if (cauHoi.thoiDiemTao != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].ngayTao = cauHoi.thoiDiemTao.Value;
                    }

                    if (cauHoi.thoiDiemCapNhat != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].ngayCapNhat = cauHoi.thoiDiemCapNhat.Value;
                    }

                    if (cauHoi.nguoiTao.hinhDaiDien.ma != null && cauHoi.nguoiTao.hinhDaiDien.duoi != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].hinhAnh = cauHoi.nguoiTao.hinhDaiDien.ma.Value + cauHoi.nguoiTao.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_CauHoi;
        }

        public List<clientmodel_CauHoi> layTheoTieuChi(string tieuChi, int soCauHoi)
        {
            KetQua ketQua = CauHoiBUS.layDanhSach(soCauHoi, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } }, tieuChi);
            List<clientmodel_CauHoi> lst_CauHoi = new List<clientmodel_CauHoi>();
            
            if(ketQua.trangThai == 0)
            {
                foreach(var cauHoi in ketQua.ketQua as List<CauHoiDTO>)
                {
                    if (cauHoi.ma != null)
                    {
                        lst_CauHoi.Add(new clientmodel_CauHoi()
                        {
                            ma = cauHoi.ma.Value,
                        });
                    }

                    if (cauHoi.tieuDe != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].tieuDe = cauHoi.tieuDe;
                    }

                    if (cauHoi.noiDung != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].noiDung = cauHoi.noiDung;
                    }

                    if (cauHoi.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].nguoiTao = cauHoi.nguoiTao.tenTaiKhoan;
                    }

                    if (cauHoi.soLuongTraLoi != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].soTraLoi = cauHoi.soLuongTraLoi.Value;
                    }

                    if (cauHoi.thoiDiemTao != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].ngayTao = cauHoi.thoiDiemTao.Value;
                    }

                    if (cauHoi.thoiDiemCapNhat != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].ngayCapNhat = cauHoi.thoiDiemCapNhat.Value;
                    }

                    if (cauHoi.nguoiTao.hinhDaiDien.ma != null && cauHoi.nguoiTao.hinhDaiDien.duoi != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].hinhAnh = cauHoi.nguoiTao.hinhDaiDien.ma.Value + cauHoi.nguoiTao.hinhDaiDien.duoi;
                    }
                }
            }

            return lst_CauHoi;
        }

        /// <summary>
        /// Webservice tìm kiếm câu hỏi
        /// </summary>
        /// <param name="tuKhoa"></param>
        /// <returns>List<clientmodel_CauHoi></returns>
        public List<clientmodel_CauHoi> timKiem(string tuKhoa)
        {
            KetQua ketQua = CauHoiBUS.lay_TimKiem(tuKhoa, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_CauHoi> lst_CauHoi = new List<clientmodel_CauHoi>();

            if(ketQua.trangThai == 0)
            {
                foreach(var cauHoi in ketQua.ketQua as List<CauHoiDTO>)
                {
                    if (cauHoi.ma != null)
                    {
                        lst_CauHoi.Add(new clientmodel_CauHoi()
                        {
                            ma = cauHoi.ma.Value,
                        });
                    }

                    if (cauHoi.tieuDe != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].tieuDe = cauHoi.tieuDe;
                    }

                    if (cauHoi.noiDung != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].noiDung = cauHoi.noiDung;
                    }

                    if (cauHoi.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].nguoiTao = cauHoi.nguoiTao.tenTaiKhoan;
                    }

                    if (cauHoi.soLuongTraLoi != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].soTraLoi = cauHoi.soLuongTraLoi.Value;
                    }

                    if (cauHoi.thoiDiemTao != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].ngayTao = cauHoi.thoiDiemTao.Value;
                    }

                    if (cauHoi.thoiDiemCapNhat != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].ngayCapNhat = cauHoi.thoiDiemCapNhat.Value;
                    }

                    if (cauHoi.nguoiTao.hinhDaiDien.ma != null && cauHoi.nguoiTao.hinhDaiDien.duoi != null)
                    {
                        lst_CauHoi[lst_CauHoi.Count - 1].hinhAnh = cauHoi.nguoiTao.hinhDaiDien.ma.Value + cauHoi.nguoiTao.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_CauHoi;
        }

        /// <summary>
        /// Webservice tìm kiếm câu hỏi theo mã chủ đề
        /// </summary>
        /// <param name="maChuDe"></param>
        /// <param name="tuKhoa"></param>
        /// <returns>List<CauHoiDTO></returns>
        public List<CauHoiDTO> timKiemTheoChuDe(int maChuDe, string tuKhoa)
        {
            KetQua ketQua = CauHoiBUS.layTheoMaChuDe_TimKiem(maChuDe, tuKhoa, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<CauHoiDTO> lst_CauHoi = new List<CauHoiDTO>();

            if (ketQua.trangThai == 0)
            {
                lst_CauHoi = ketQua.ketQua as List<CauHoiDTO>;
            }
            return lst_CauHoi;
        }
    }
}
