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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc_DienDan" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc_DienDan.svc or wcf_KhoaHoc_DienDan.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc_DienDan : Iwcf_KhoaHoc_DienDan
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

        /// <summary>
        /// Webservice lấy bài viết diễn dàn theo mã
        /// </summary>
        /// <param name="maDienDan"></param>
        /// <returns>clientmodel_KhoaHoc_DienDan</returns>
        public clientmodel_KhoaHoc_DienDan layTheoMa(int maDienDan)
        {
            KetQua ketQua = BaiVietDienDanBUS.layTheoMa(maDienDan, new LienKet { "TapTin", { "NguoiTao", new LienKet { "HinhDaiDien" } } });
            BaiVietDienDanDTO dto_DienDan = ketQua.ketQua as BaiVietDienDanDTO;
            clientmodel_KhoaHoc_DienDan cm_DienDan = new clientmodel_KhoaHoc_DienDan();

            if(ketQua.trangThai == 0)
            {
                if(dto_DienDan.ma != null)
                {
                    cm_DienDan.ma = dto_DienDan.ma.Value;
                }

                if(dto_DienDan.tieuDe != null)
                {
                    cm_DienDan.tieuDe = dto_DienDan.tieuDe;
                }

                if(dto_DienDan.noiDung != null)
                {
                    cm_DienDan.noiDung = dto_DienDan.noiDung;
                }

                if(dto_DienDan.diem != null)
                {
                    cm_DienDan.diemCong = dto_DienDan.diem.Value;
                }

                if(dto_DienDan.thoiDiemTao != null)
                {
                    cm_DienDan.ngayTao = dto_DienDan.thoiDiemTao.Value;
                }

                if(dto_DienDan.nguoiTao != null)
                {
                    if(dto_DienDan.nguoiTao.ma != null)
                    {
                        cm_DienDan.maNguoiTao = dto_DienDan.nguoiTao.ma.Value;
                    }
                    
                    if(dto_DienDan.nguoiTao.tenLot != null)
                    {
                        cm_DienDan.tenNguoiTao = string.Format("{0} {1} {2}", dto_DienDan.nguoiTao.ho, dto_DienDan.nguoiTao.tenLot, dto_DienDan.nguoiTao.ten);
                    }
                    else
                    {
                        cm_DienDan.tenNguoiTao = string.Format("{0} {1}", dto_DienDan.nguoiTao.ho, dto_DienDan.nguoiTao.ten);
                    }

                    if(dto_DienDan.nguoiTao.tenTaiKhoan != null)
                    {
                        cm_DienDan.tenTKNguoiTao = dto_DienDan.nguoiTao.tenTaiKhoan;
                    }

                    if(dto_DienDan.nguoiTao.hinhDaiDien != null)
                    {
                        if(dto_DienDan.nguoiTao.hinhDaiDien.ma != null && dto_DienDan.nguoiTao.hinhDaiDien.duoi != null)
                        {
                            cm_DienDan.hinhAnh = dto_DienDan.nguoiTao.hinhDaiDien.ma.Value + dto_DienDan.nguoiTao.hinhDaiDien.duoi;
                        }
                    }
                }

                if(dto_DienDan.tapTin != null)
                {
                    if(dto_DienDan.tapTin.ma != null)
                    {
                        cm_DienDan.maTapTin = dto_DienDan.tapTin.ma.Value;
                    }
                    
                    if(dto_DienDan.tapTin.ten != null)
                    {
                        cm_DienDan.tenTapTin = dto_DienDan.tapTin.ten;
                    }
                }
            }

            return cm_DienDan;
        }

        /// <summary>
        /// Webservice lấy danh sách bài viết diễn đàn theo mã khóa học
        /// </summary>
        /// <param name="maKhoaHoc"></param>
        /// <returns>List<clientmodel_KhoaHoc_DienDan></returns>
        public List<clientmodel_KhoaHoc_DienDan> layDanhSachTheoMaKhoaHoc(int maKhoaHoc)
        {
            KetQua ketQua = BaiVietDienDanBUS.layTheoMaKhoaHoc(maKhoaHoc, new LienKet { "TapTin", { "NguoiTao", new LienKet { "HinhDaiDien" } } });
            List<clientmodel_KhoaHoc_DienDan> lst_DienDan = new List<clientmodel_KhoaHoc_DienDan>();

            if(ketQua.trangThai == 0)
            {
                foreach(var dienDan in ketQua.ketQua as List<BaiVietDienDanDTO>)
                {
                    if(dienDan.ma != null)
                    {
                        lst_DienDan.Add(new clientmodel_KhoaHoc_DienDan()
                        {
                            ma = dienDan.ma.Value,
                        });
                    }

                    if(dienDan.tieuDe != null)
                    {
                        lst_DienDan[lst_DienDan.Count - 1].tieuDe = dienDan.tieuDe;
                    }

                    if(dienDan.noiDung != null)
                    {
                        lst_DienDan[lst_DienDan.Count - 1].noiDung = dienDan.noiDung;
                    }

                    if(dienDan.diem != null)
                    {
                        lst_DienDan[lst_DienDan.Count - 1].diemCong = dienDan.diem.Value;
                    }

                    if(dienDan.thoiDiemTao != null)
                    {
                        lst_DienDan[lst_DienDan.Count - 1].ngayTao = dienDan.thoiDiemTao.Value;
                    }

                    if(dienDan.nguoiTao != null)
                    {
                        if(dienDan.nguoiTao.ma != null)
                        {
                            lst_DienDan[lst_DienDan.Count - 1].maNguoiTao = dienDan.nguoiTao.ma.Value;
                        }

                        if(dienDan.nguoiTao.tenLot != null)
                        {
                            lst_DienDan[lst_DienDan.Count - 1].tenNguoiTao = string.Format("{0} {1} {2}", dienDan.nguoiTao.ho, dienDan.nguoiTao.tenLot, dienDan.nguoiTao.ten);
                        }
                        else
                        {
                            lst_DienDan[lst_DienDan.Count - 1].tenNguoiTao = string.Format("{0} {1}", dienDan.nguoiTao.ho, dienDan.nguoiTao.ten);
                        }

                        if(dienDan.nguoiTao.tenTaiKhoan != null)
                        {
                            lst_DienDan[lst_DienDan.Count - 1].tenTKNguoiTao = dienDan.nguoiTao.tenTaiKhoan;
                        }

                        if(dienDan.nguoiTao.hinhDaiDien != null)
                        {
                            if(dienDan.nguoiTao.hinhDaiDien.ma != null && dienDan.nguoiTao.hinhDaiDien.duoi != null)
                            {
                                lst_DienDan[lst_DienDan.Count - 1].hinhAnh = dienDan.nguoiTao.hinhDaiDien.ma.Value + dienDan.nguoiTao.hinhDaiDien.duoi;
                            }
                        }
                    }

                    if(dienDan.tapTin != null)
                    {
                        if(dienDan.tapTin.ma != null)
                        {
                            lst_DienDan[lst_DienDan.Count - 1].maTapTin = dienDan.tapTin.ma.Value;
                        }

                        if(dienDan.tapTin.ten != null)
                        {
                            lst_DienDan[lst_DienDan.Count - 1].tenTapTin = dienDan.tapTin.ten;
                        }
                    }
                }
            }

            return lst_DienDan;
        }


        public clientmodel_ThongBao themBaiVietDienDan(int maNguoiThem, int maKhoaHoc, string tieuDe, string noiDung, byte[] tapTin = null, string tenTapTin = null, string contenttype = null)
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

            if(maTapTin != -1)
            {
                form = new Form()
                {
                    {"MaNguoiTao",maNguoiThem.ToString()},
                    {"MaKhoaHoc",maKhoaHoc.ToString()},
                    {"TieuDe",tieuDe},
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
                    {"TieuDe",tieuDe},
                    {"NoiDung",noiDung},
                };
            }

            KetQua ketQua = BaiVietDienDanBUS.them(form);
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();
            cm_ThongBao.trangThai = ketQua.trangThai;

            if(ketQua.trangThai == 0)
            {
                cm_ThongBao.thongBao = "Bài viết của bạn đã được đưa vào danh sách duyệt";
            }
            else
            {
                cm_ThongBao.thongBao = ketQua.ketQua as string;
            }

            return cm_ThongBao;
        }
    }
}
