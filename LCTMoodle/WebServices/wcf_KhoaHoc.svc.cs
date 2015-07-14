using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;
using BUSLayer;
using Helpers;


namespace LCTMoodle.WebServices
{
    public class wcf_KhoaHoc : Iwcf_KhoaHoc
    {
        private const string _Loai = "KhoaHoc_HinhDaiDien";

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
        /// Webservice lấy hình ảnh và trả về chỉ số
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
        /// Webservice lấy khóa học theo mã
        /// </summary>
        /// <param name="ma"></param>
        /// <returns>clientmodel_KhoaHoc</returns>
        public clientmodel_KhoaHoc layTheoMa(int ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(ma, new LienKet { "HinhDaiDien" , "NguoiTao" });
            KhoaHocDTO dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            clientmodel_KhoaHoc cm_KhoaHoc = new clientmodel_KhoaHoc();

            if(ketQua.trangThai == 0)
            {
                if(dto_KhoaHoc.ma != null)
                {
                    cm_KhoaHoc.ma = dto_KhoaHoc.ma.Value;
                }

                if(dto_KhoaHoc.ten != null)
                {
                    cm_KhoaHoc.ten = dto_KhoaHoc.ten;
                }

                if(dto_KhoaHoc.moTa != null)
                {
                    cm_KhoaHoc.moTa = dto_KhoaHoc.moTa;
                }

                if(dto_KhoaHoc.thoiDiemTao != null)
                {
                    cm_KhoaHoc.ngayTao = dto_KhoaHoc.thoiDiemTao.Value;
                }

                if(dto_KhoaHoc.thoiDiemHetHan != null)
                {
                    cm_KhoaHoc.ngayHetHan = dto_KhoaHoc.thoiDiemHetHan.Value;
                }

                if(dto_KhoaHoc.nguoiTao.tenTaiKhoan != null)
                {
                    cm_KhoaHoc.nguoiTao = dto_KhoaHoc.nguoiTao.tenTaiKhoan;
                }

                if(dto_KhoaHoc.hinhDaiDien.ma != null && dto_KhoaHoc.hinhDaiDien.duoi != null)
                {
                    cm_KhoaHoc.hinhAnh = dto_KhoaHoc.hinhDaiDien.ma.Value + dto_KhoaHoc.hinhDaiDien.duoi;
                }
            }
            return cm_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy tất cả khóa học
        /// </summary>
        /// <returns>List<KhoaHocDTO></returns>
        public List<KhoaHocDTO> lay()
        {
            KetQua ketQua = KhoaHocBUS.lay(new LienKet { "HinhDaiDien" });
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học theo mã chủ đề
        /// </summary>
        /// <param name="maChuDe"></param>
        /// <returns>List<clientmodel_KhoaHoc></returns>
        public List<clientmodel_KhoaHoc> layTheoMaChuDe(int maChuDe)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMaChuDe(maChuDe, new LienKet { "HinhDaiDien" , "NguoiTao"});
            List<clientmodel_KhoaHoc> lst_KhoaHoc = new List<clientmodel_KhoaHoc>();

            if(ketQua.trangThai == 0)
            {
                foreach(var khoaHoc in ketQua.ketQua as List<KhoaHocDTO>)
                {
                    if(khoaHoc.ma != null)
                    {
                        lst_KhoaHoc.Add(new clientmodel_KhoaHoc()
                        {
                            ma = khoaHoc.ma.Value,
                        });
                    }
                    
                    if(khoaHoc.ten != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].ten = khoaHoc.ten;
                    }

                    if(khoaHoc.moTa != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].moTa = khoaHoc.moTa;
                    }

                    if(khoaHoc.nguoiTao.ten != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].nguoiTao = khoaHoc.nguoiTao.tenTaiKhoan;
                    }

                    if(khoaHoc.thoiDiemTao != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayTao = khoaHoc.thoiDiemTao.Value;
                    }

                    if(khoaHoc.thoiDiemHetHan != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayHetHan = khoaHoc.thoiDiemHetHan.Value;
                    }

                    if(khoaHoc.hinhDaiDien.ma != null && khoaHoc.hinhDaiDien.ten != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].hinhAnh = khoaHoc.hinhDaiDien.ma.Value + khoaHoc.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học theo mã người dùng
        /// </summary>
        /// <param name="maNguoiDung"></param>
        /// <param name="soKhoaHoc"></param>
        /// <returns>List<clientmodel_KhoaHoc></returns>
        public List<clientmodel_KhoaHoc> layKhoaHocThamGiaTheoMaNguoiDung(int maNguoiDung , int soKhoaHoc)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMaNguoiDung(maNguoiDung, new LienKet { "HinhDaiDien", "NguoiTao" });
            List<KhoaHocDTO>[] lst_KhoaHocTongHop = ketQua.ketQua as List<KhoaHocDTO>[];
            List<clientmodel_KhoaHoc> lst_KhoaHoc = new List<clientmodel_KhoaHoc>();
            
            if(ketQua.trangThai == 0)
            {
                if(lst_KhoaHocTongHop[0].Count > 0)
                {
                    if(soKhoaHoc != 0)
                    {
                        int dem = 0;
                        foreach (var khoaHoc in lst_KhoaHocTongHop[0] as List<KhoaHocDTO>)
                        {
                            if(dem < soKhoaHoc)
                            {
                                if (khoaHoc.ma != null)
                                {
                                    lst_KhoaHoc.Add(new clientmodel_KhoaHoc()
                                    {
                                        ma = khoaHoc.ma.Value,
                                    });
                                }

                                if (khoaHoc.ten != null)
                                {
                                    lst_KhoaHoc[lst_KhoaHoc.Count - 1].ten = khoaHoc.ten;
                                }

                                if (khoaHoc.moTa != null)
                                {
                                    lst_KhoaHoc[lst_KhoaHoc.Count - 1].moTa = khoaHoc.moTa;
                                }

                                if (khoaHoc.nguoiTao.tenTaiKhoan != null)
                                {
                                    lst_KhoaHoc[lst_KhoaHoc.Count - 1].nguoiTao = khoaHoc.nguoiTao.tenTaiKhoan;
                                }

                                if (khoaHoc.thoiDiemTao != null)
                                {
                                    lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayTao = khoaHoc.thoiDiemTao.Value;
                                }

                                if (khoaHoc.thoiDiemHetHan != null)
                                {
                                    lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayHetHan = khoaHoc.thoiDiemHetHan.Value;
                                }

                                if (khoaHoc.hinhDaiDien.ma != null && khoaHoc.hinhDaiDien.duoi != null)
                                {
                                    lst_KhoaHoc[lst_KhoaHoc.Count - 1].hinhAnh = khoaHoc.hinhDaiDien.ma.Value + khoaHoc.hinhDaiDien.duoi;
                                }
                                dem++;
                            }
                        }
                    }
                    else
                    {
                        foreach (var khoaHoc in lst_KhoaHocTongHop[0] as List<KhoaHocDTO>)
                        {
                            if (khoaHoc.ma != null)
                            {
                                lst_KhoaHoc.Add(new clientmodel_KhoaHoc()
                                {
                                    ma = khoaHoc.ma.Value,
                                });
                            }

                            if (khoaHoc.ten != null)
                            {
                                lst_KhoaHoc[lst_KhoaHoc.Count - 1].ten = khoaHoc.ten;
                            }

                            if (khoaHoc.moTa != null)
                            {
                                lst_KhoaHoc[lst_KhoaHoc.Count - 1].moTa = khoaHoc.moTa;
                            }

                            if (khoaHoc.nguoiTao.tenTaiKhoan != null)
                            {
                                lst_KhoaHoc[lst_KhoaHoc.Count - 1].nguoiTao = khoaHoc.nguoiTao.tenTaiKhoan;
                            }

                            if (khoaHoc.thoiDiemTao != null)
                            {
                                lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayTao = khoaHoc.thoiDiemTao.Value;
                            }

                            if (khoaHoc.thoiDiemHetHan != null)
                            {
                                lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayHetHan = khoaHoc.thoiDiemHetHan.Value;
                            }

                            if (khoaHoc.hinhDaiDien.ma != null && khoaHoc.hinhDaiDien.duoi != null)
                            {
                                lst_KhoaHoc[lst_KhoaHoc.Count - 1].hinhAnh = khoaHoc.hinhDaiDien.ma.Value + khoaHoc.hinhDaiDien.duoi;
                            }
                        }
                    }
                }
            }

            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học theo tiêu chí và số khóa học truyền vào
        /// </summary>
        /// <param name="soKhoaHoc"></param>
        /// <param name="tieuChi"></param>
        /// <returns>List<clientmodel_KhoaHoc></returns>
        public List<clientmodel_KhoaHoc> layTheoTieuChi(int soKhoaHoc, string tieuChi)
        {
            KetQua ketQua = KhoaHocBUS.timKiemPhanTrang(1, soKhoaHoc, null, tieuChi, new LienKet { "HinhDaiDien", "NguoiTao" });
            List<clientmodel_KhoaHoc> lst_KhoaHoc = new List<clientmodel_KhoaHoc>();

            if(ketQua.trangThai == 0)
            {
                foreach(var khoaHoc in ketQua.ketQua as List<KhoaHocDTO>)
                {
                    if (khoaHoc.ma != null)
                    {
                        lst_KhoaHoc.Add(new clientmodel_KhoaHoc()
                        {
                            ma = khoaHoc.ma.Value,
                        });
                    }

                    if (khoaHoc.ten != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].ten = khoaHoc.ten;
                    }

                    if (khoaHoc.moTa != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].moTa = khoaHoc.moTa;
                    }

                    if (khoaHoc.nguoiTao.ten != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].nguoiTao = khoaHoc.nguoiTao.tenTaiKhoan;
                    }

                    if (khoaHoc.thoiDiemTao != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayTao = khoaHoc.thoiDiemTao.Value;
                    }

                    if (khoaHoc.thoiDiemHetHan != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayHetHan = khoaHoc.thoiDiemHetHan.Value;
                    }

                    if (khoaHoc.hinhDaiDien.ma != null && khoaHoc.hinhDaiDien.ten != null)
                    {
                        lst_KhoaHoc[lst_KhoaHoc.Count - 1].hinhAnh = khoaHoc.hinhDaiDien.ma.Value + khoaHoc.hinhDaiDien.duoi;
                    }
                }
            }

            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice tìm kiếm khóa học
        /// </summary>
        /// <param name="tuKhoa"></param>
        /// <returns></returns>
        public List<clientmodel_KhoaHoc> timKiem(string tuKhoa)
        {
            KetQua ketQua = KhoaHocBUS.lay_TimKiem(tuKhoa, new LienKet { "HinhDaiDien", "NguoiTao" });
            List<clientmodel_KhoaHoc> lst_KhoaHoc = new List<clientmodel_KhoaHoc>();

            if(ketQua.trangThai == 0)
            {
               foreach(var khoaHoc in ketQua.ketQua as List<KhoaHocDTO>)
               {
                   if (khoaHoc.ma != null)
                   {
                       lst_KhoaHoc.Add(new clientmodel_KhoaHoc()
                       {
                           ma = khoaHoc.ma.Value,
                       });
                   }

                   if (khoaHoc.ten != null)
                   {
                       lst_KhoaHoc[lst_KhoaHoc.Count - 1].ten = khoaHoc.ten;
                   }

                   if (khoaHoc.moTa != null)
                   {
                       lst_KhoaHoc[lst_KhoaHoc.Count - 1].moTa = khoaHoc.moTa;
                   }

                   if (khoaHoc.nguoiTao.ten != null)
                   {
                       lst_KhoaHoc[lst_KhoaHoc.Count - 1].nguoiTao = khoaHoc.nguoiTao.tenTaiKhoan;
                   }

                   if (khoaHoc.thoiDiemTao != null)
                   {
                       lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayTao = khoaHoc.thoiDiemTao.Value;
                   }

                   if (khoaHoc.thoiDiemHetHan != null)
                   {
                       lst_KhoaHoc[lst_KhoaHoc.Count - 1].ngayHetHan = khoaHoc.thoiDiemHetHan.Value;
                   }

                   if (khoaHoc.hinhDaiDien.ma != null && khoaHoc.hinhDaiDien.ten != null)
                   {
                       lst_KhoaHoc[lst_KhoaHoc.Count - 1].hinhAnh = khoaHoc.hinhDaiDien.ma.Value + khoaHoc.hinhDaiDien.duoi;
                   }
               }
           }

           return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice tìm kiếm theo mã chủ đề
        /// </summary>
        /// <param name="maChuDe"></param>
        /// <param name="tuKhoa"></param>
        /// <returns>List<KhoaHocDTO></returns>
        public List<KhoaHocDTO> timKiemTheoMaChuDe(int maChuDe,string tuKhoa)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMaChuDe_TimKiem(maChuDe, tuKhoa, new LienKet { "HinhDaiDien" });
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservie kiểm tra khóa học được tham gia hay chưa
        /// </summary>
        /// <param name="maNguoiDung"></param>
        /// <param name="maKhoaHoc"></param>
        /// <returns></returns>
        public int kiemTraThamGia(int maNguoiDung, int maKhoaHoc)
        {
            KetQua ketQua = KhoaHoc_NguoiDungBUS.layTheoMaKhoaHocVaMaNguoiDung(maKhoaHoc, maNguoiDung);
            return (int)ketQua.ketQua;
        }
    }
}
