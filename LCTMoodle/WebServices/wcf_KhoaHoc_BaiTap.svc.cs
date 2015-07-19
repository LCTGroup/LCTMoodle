using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BUSLayer;
using DTOLayer;
using LCTMoodle.WebServices.Client_Model;
using Helpers;
using System.IO;
using System.Drawing;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc_BaiTap" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc_BaiTap.svc or wcf_KhoaHoc_BaiTap.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc_BaiTap : Iwcf_KhoaHoc_BaiTap
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
        /// Webservice lấy bài tập theo mã
        /// </summary>
        /// <param name="maBaiTap"></param>
        /// <returns>clientmodel_KhoaHoc_BaiTap</returns>
        public clientmodel_KhoaHoc_BaiTap layTheoMa(int maBaiTap)
        {
            KetQua ketQua = BaiVietBaiTapBUS.layTheoMa(maBaiTap, new LienKet() {"TapTin", { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            BaiVietBaiTapDTO dto_BaiTap = ketQua.ketQua as BaiVietBaiTapDTO;
            clientmodel_KhoaHoc_BaiTap cm_BaiTap = new clientmodel_KhoaHoc_BaiTap();

            if(ketQua.trangThai == 0)
            {
                if(dto_BaiTap.ma != null)
                {
                    cm_BaiTap.ma = dto_BaiTap.ma.Value;
                }

                if(dto_BaiTap.tieuDe != null)
                {
                    cm_BaiTap.tieuDe = dto_BaiTap.tieuDe;
                }

                if(dto_BaiTap.noiDung != null)
                {
                    cm_BaiTap.noiDung = dto_BaiTap.noiDung;
                }

                if(dto_BaiTap.tapTin != null)
                {
                    if (dto_BaiTap.tapTin.ma != null)
                    {
                        cm_BaiTap.maFile = dto_BaiTap.tapTin.ma.Value;
                    }

                    if (dto_BaiTap.tapTin.ten != null)
                    {
                        cm_BaiTap.tenFile = dto_BaiTap.tapTin.ten;
                    }
                }

                if(dto_BaiTap.nguoiTao.ma != null)
                {
                    cm_BaiTap.maNguoiTao = dto_BaiTap.nguoiTao.ma.Value;
                }

                if(dto_BaiTap.nguoiTao.tenTaiKhoan != null)
                {
                    cm_BaiTap.tenTaiKhoan = dto_BaiTap.nguoiTao.tenTaiKhoan;
                }

                if(dto_BaiTap.thoiDiemTao != null)
                {
                    cm_BaiTap.ngayTao = dto_BaiTap.thoiDiemTao.Value;
                }

                if(dto_BaiTap.thoiDiemHetHan != null)
                {
                    cm_BaiTap.ngayHetHan = dto_BaiTap.thoiDiemHetHan.Value;
                }

                if(dto_BaiTap.nguoiTao.hinhDaiDien.ma != null && dto_BaiTap.nguoiTao.hinhDaiDien.duoi != null)
                {
                    cm_BaiTap.hinhAnh = dto_BaiTap.nguoiTao.hinhDaiDien.ma.Value + dto_BaiTap.nguoiTao.hinhDaiDien.duoi;
                }
            }
            return cm_BaiTap;
        }

        /// <summary>
        /// Webservice lấy danh sách bài tập theo mã khóa học
        /// </summary>
        /// <param name="maKhoaHoc"></param>
        /// <returns>List<clientmodel_KhoaHoc_BaiTap></returns>
        public List<clientmodel_KhoaHoc_BaiTap> layDanhSachTheoMaKhoaHoc(int maKhoaHoc)
        {
            KetQua ketQua = BaiVietBaiTapBUS.layTheoMaKhoaHoc(maKhoaHoc, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_KhoaHoc_BaiTap> lst_BaiTap = new List<clientmodel_KhoaHoc_BaiTap>();

            if(ketQua.trangThai == 0)
            {
                foreach(var baiTap in ketQua.ketQua as List<BaiVietBaiTapDTO>)
                {
                    if(baiTap.ma != null)
                    {
                        lst_BaiTap.Add(new clientmodel_KhoaHoc_BaiTap()
                        {
                            ma = baiTap.ma.Value,
                        });
                    }

                    if(baiTap.tieuDe != null)
                    {
                        lst_BaiTap[lst_BaiTap.Count - 1].tieuDe = baiTap.tieuDe;
                    }

                    if(baiTap.noiDung != null)
                    {
                        lst_BaiTap[lst_BaiTap.Count - 1].noiDung = baiTap.noiDung;
                    }

                    if(baiTap.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_BaiTap[lst_BaiTap.Count - 1].tenTaiKhoan = baiTap.nguoiTao.tenTaiKhoan;
                    }

                    if(baiTap.thoiDiemTao != null)
                    {
                        lst_BaiTap[lst_BaiTap.Count - 1].ngayTao = baiTap.thoiDiemTao.Value;
                    }

                    if(baiTap.thoiDiemHetHan != null)
                    {
                        lst_BaiTap[lst_BaiTap.Count - 1].ngayHetHan = baiTap.thoiDiemHetHan.Value;
                    }

                    if(baiTap.nguoiTao.hinhDaiDien.ma != null && baiTap.nguoiTao.hinhDaiDien.duoi != null)
                    {
                        lst_BaiTap[lst_BaiTap.Count - 1].hinhAnh = baiTap.nguoiTao.hinhDaiDien.ma.Value + baiTap.nguoiTao.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_BaiTap;
        }


        public clientmodel_ThongBao_List themBaiTap(int maNguoiThem, int maKhoaHoc, string tieuDe, string noiDung, DateTime thoiHan, int loai = 0, int cachNop = 0, byte[] tapTin = null, string tenTapTin = null, string contenttype = null)
        {
            int maTapTin = -1;

            if (tapTin != null && tenTapTin != null && contenttype != null)
            {
                KetQua ketQuaTapTin = TapTinBUS.them(tapTin, tenTapTin, 0, contenttype);
                TapTinDTO dto_TapTin = ketQuaTapTin.ketQua as TapTinDTO;

                if (ketQuaTapTin.trangThai == 0)
                {
                    maTapTin = dto_TapTin.ma.Value;
                }
            }

            Form form;

            if (maTapTin != -1)
            {
                form = new Form()
                {
                    {"MaNguoiTao",maNguoiThem.ToString()},
                    {"MaKhoaHoc",maKhoaHoc.ToString()},
                    {"ThoiDiemHetHan",noiDung},
                    {"TieuDe",tieuDe},
                    {"Loai",loai.ToString()},
                    {"CachNop",cachNop.ToString()},
                    {"NoiDung",noiDung},
                    {"MaTapTin",maTapTin.ToString()},
                };
            }
            else
            {
                form = new Form()
                {
                    {"MaNguoiTao",maNguoiThem.ToString()},
                    {"MaKhoaHoc",maKhoaHoc.ToString()},
                    {"ThoiDiemHetHan",noiDung},
                    {"TieuDe",tieuDe},
                    {"NoiDung",noiDung},
                    {"Loai",loai.ToString()},
                    {"CachNop",cachNop.ToString()},
                };
            }

            KetQua ketQua = BaiVietBaiTapBUS.them(form);
            clientmodel_ThongBao_List cm_ThongBao = new clientmodel_ThongBao_List();
            cm_ThongBao.trangThai = ketQua.trangThai;

            return cm_ThongBao;
        }
    }
}
