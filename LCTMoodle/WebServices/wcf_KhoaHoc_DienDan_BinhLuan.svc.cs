using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;
using BUSLayer;
using Helpers;
using System.IO;
using System.Drawing;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc_DienDan_BinhLuan" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc_DienDan_BinhLuan.svc or wcf_KhoaHoc_DienDan_BinhLuan.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc_DienDan_BinhLuan : Iwcf_KhoaHoc_DienDan_BinhLuan
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
        /// Webservice lấy bình luận diễn đàn theo mã
        /// </summary>
        /// <param name="maDienDan"></param>
        /// <returns>clientmodel_KhoaHoc_DienDan_BinhLuan</returns>
        public clientmodel_KhoaHoc_DienDan_BinhLuan layTheoMa(int maDienDan)
        {
            KetQua ketQua = BinhLuanBaiVietDienDanBUS.layTheoMa(maDienDan, new LienKet() { "TapTin", { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            BinhLuanBaiVietDienDanDTO dto_BinhLuan = ketQua.ketQua as BinhLuanBaiVietDienDanDTO;
            clientmodel_KhoaHoc_DienDan_BinhLuan cm_BinhLuan = new clientmodel_KhoaHoc_DienDan_BinhLuan();

            if(ketQua.trangThai == 0)
            {
                if(dto_BinhLuan.ma != null)
                {
                    cm_BinhLuan.ma = dto_BinhLuan.ma.Value;
                }


                if(dto_BinhLuan.diem != null)
                {
                    cm_BinhLuan.diem = dto_BinhLuan.diem.Value;
                }

                if(dto_BinhLuan.noiDung != null)
                {
                    cm_BinhLuan.noiDung = dto_BinhLuan.noiDung;
                }

                if(dto_BinhLuan.thoiDiemTao != null)
                {
                    cm_BinhLuan.ngayTao = dto_BinhLuan.thoiDiemTao.Value;
                }

                if(dto_BinhLuan.nguoiTao != null)
                {
                    if(dto_BinhLuan.nguoiTao.ma != null)
                    {
                        cm_BinhLuan.maNguoiTao = dto_BinhLuan.nguoiTao.ma.Value;
                    }

                    if(dto_BinhLuan.nguoiTao.tenLot != null)
                    {
                        cm_BinhLuan.tenNguoiTao = string.Format("{0} {1} {2}", dto_BinhLuan.nguoiTao.ho, dto_BinhLuan.nguoiTao.tenLot, dto_BinhLuan.nguoiTao.ten);
                    }
                    else
                    {
                        cm_BinhLuan.tenNguoiTao = string.Format("{0} {1}", dto_BinhLuan.nguoiTao.ho, dto_BinhLuan.nguoiTao.ten);
                    }

                    if(dto_BinhLuan.nguoiTao.tenTaiKhoan != null)
                    {
                        cm_BinhLuan.tenTKNguoiTao = dto_BinhLuan.nguoiTao.tenTaiKhoan;
                    }

                    if(dto_BinhLuan.nguoiTao.hinhDaiDien != null)
                    {
                        if(dto_BinhLuan.nguoiTao.hinhDaiDien.ma != null && dto_BinhLuan.nguoiTao.hinhDaiDien.duoi != null)
                        {
                            cm_BinhLuan.hinhAnh = dto_BinhLuan.nguoiTao.hinhDaiDien.ma.Value + dto_BinhLuan.nguoiTao.hinhDaiDien.duoi;
                        }
                    }
                }

                if(dto_BinhLuan.tapTin != null)
                {
                    if(dto_BinhLuan.tapTin.ma != null)
                    {
                        cm_BinhLuan.maTapTin = dto_BinhLuan.tapTin.ma.Value;
                    }

                    if(dto_BinhLuan.tapTin.ten != null)
                    {
                        cm_BinhLuan.tenTapTin = dto_BinhLuan.tapTin.ten;
                    }
                }
            }

            return cm_BinhLuan;
        }

        /// <summary>
        /// Webservice lấy danh sách bình luận theo mã bài viết diễn dàn
        /// </summary>
        /// <param name="maDienDan"></param>
        /// <returns>List<clientmodel_KhoaHoc_DienDan_BinhLuan></returns>
        public List<clientmodel_KhoaHoc_DienDan_BinhLuan> layDanhSachTheoMaDienDan(int maDienDan)
        {
            KetQua ketQua = BinhLuanBaiVietDienDanBUS.layTheoMaBaiVietDienDan(maDienDan, new LienKet() { "TapTin", { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_KhoaHoc_DienDan_BinhLuan> lst_BinhLuan = new List<clientmodel_KhoaHoc_DienDan_BinhLuan>();

            if(ketQua.trangThai == 0)
            {
                foreach(var binhLuan in ketQua.ketQua as List<BinhLuanBaiVietDienDanDTO>)
                {
                    if(binhLuan.ma != null)
                    {
                        lst_BinhLuan.Add(new clientmodel_KhoaHoc_DienDan_BinhLuan()
                        {
                            ma = binhLuan.ma.Value,
                        });
                    }

                    if(binhLuan.diem != null)
                    {
                        lst_BinhLuan[lst_BinhLuan.Count - 1].diem = binhLuan.diem.Value;
                    }

                    if(binhLuan.noiDung != null)
                    {
                        lst_BinhLuan[lst_BinhLuan.Count - 1].noiDung = binhLuan.noiDung;
                    }

                    if(binhLuan.thoiDiemTao != null)
                    {
                        lst_BinhLuan[lst_BinhLuan.Count - 1].ngayTao = binhLuan.thoiDiemTao.Value;
                    }

                    if(binhLuan.nguoiTao != null)
                    {
                        if(binhLuan.nguoiTao.ma != null)
                        {
                            lst_BinhLuan[lst_BinhLuan.Count - 1].maNguoiTao = binhLuan.nguoiTao.ma.Value;
                        }

                        if(binhLuan.nguoiTao.tenLot != null)
                        {
                            lst_BinhLuan[lst_BinhLuan.Count - 1].tenNguoiTao = string.Format("{0} {1} {2}", binhLuan.nguoiTao.ho, binhLuan.nguoiTao.tenLot, binhLuan.nguoiTao.ten);
                        }
                        else
                        {
                            lst_BinhLuan[lst_BinhLuan.Count - 1].tenNguoiTao = string.Format("{0} {1}", binhLuan.nguoiTao.ho, binhLuan.nguoiTao.ten);
                        }

                        if(binhLuan.nguoiTao.tenTaiKhoan != null)
                        {
                            lst_BinhLuan[lst_BinhLuan.Count - 1].tenTKNguoiTao = binhLuan.nguoiTao.tenTaiKhoan;
                        }

                        if(binhLuan.nguoiTao.hinhDaiDien != null)
                        {
                            if(binhLuan.nguoiTao.hinhDaiDien.ma != null && binhLuan.nguoiTao.hinhDaiDien.duoi != null)
                            {
                                lst_BinhLuan[lst_BinhLuan.Count - 1].hinhAnh = binhLuan.nguoiTao.hinhDaiDien.ma.Value + binhLuan.nguoiTao.hinhDaiDien.duoi;
                            }
                        }
                    }

                    if(binhLuan.tapTin != null)
                    {
                        if(binhLuan.tapTin.ma != null)
                        {
                            lst_BinhLuan[lst_BinhLuan.Count - 1].maTapTin = binhLuan.tapTin.ma.Value;
                        }

                        if(binhLuan.tapTin.ten != null)
                        {
                            lst_BinhLuan[lst_BinhLuan.Count - 1].tenTapTin = binhLuan.tapTin.ten;
                        }
                    }
                }
            }
            return lst_BinhLuan;
        }

        /// <summary>
        /// Webservice thêm bình luận bài viết diễn đàn
        /// </summary>
        /// <param name="maNguoiThem"></param>
        /// <param name="maDienDan"></param>
        /// <param name="noiDung"></param>
        /// <param name="tapTin"></param>
        /// <param name="tenTapTin"></param>
        /// <param name="contenttype"></param>
        /// <returns>clientmodel_ThongBao</returns>
        public clientmodel_ThongBao themBinhLuan(int maNguoiThem, int maDienDan, string noiDung, byte[] tapTin = null, string tenTapTin = null, string contenttype = null)
        {
            int maTapTin = -1;

            if(tapTin != null && tenTapTin != null && contenttype != null)
            {
                KetQua ketQuaTapTin = TapTinBUS.them(tapTin, tenTapTin, 0, contenttype);
                TapTinDTO dto_TapTin = ketQuaTapTin.ketQua as TapTinDTO;

                if(ketQuaTapTin.trangThai == 0)
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
                    {"MaBaiVietDienDan",maDienDan.ToString()},
                    {"NoiDung",noiDung},
                    {"MaTapTin",maTapTin.ToString()},
                };
            }
            else
            {
                form = new Form()
                {
                    {"MaNguoiTao",maNguoiThem.ToString()},
                    {"MaBaiVietDienDan",maDienDan.ToString()},
                    {"NoiDung",noiDung},
                };
            }

            KetQua ketQua = BinhLuanBaiVietDienDanBUS.them(form);
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();
            cm_ThongBao.trangThai = ketQua.trangThai;

            if(ketQua.trangThai != 0)
            {
                cm_ThongBao.thongBao = ketQua.ketQua as string;
            }

            return cm_ThongBao;
        }

        /// <summary>
        /// Webservice sauwr thông tin bình luận
        /// </summary>
        /// <param name="maNguoiSua"></param>
        /// <param name="maBinhLuan"></param>
        /// <param name="noiDung"></param>
        /// <param name="tapTin"></param>
        /// <param name="tenTapTin"></param>
        /// <param name="contenttype"></param>
        /// <returns>clientmodel_ThongBao</returns>
        public clientmodel_ThongBao suaBinhLuan(int maNguoiSua, int maBinhLuan, string noiDung, byte[] tapTin = null, string tenTapTin = null, string contenttype = null)
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
                    {"Ma",maBinhLuan.ToString()},
                    {"MaNguoiTao",maNguoiSua.ToString()},
                    {"NoiDung",noiDung},
                    {"MaTapTin",maTapTin.ToString()},
                };
            }
            else
            {
                form = new Form()
                {
                    {"Ma",maBinhLuan.ToString()},
                    {"MaNguoiTao",maNguoiSua.ToString()},
                    {"NoiDung",noiDung},
                };
            }

            KetQua ketQua = BinhLuanBaiVietDienDanBUS.capNhatTheoMa(form, new LienKet { "TapTin" });
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();
            cm_ThongBao.trangThai = ketQua.trangThai;

            if(ketQua.trangThai == 0)
            {
                cm_ThongBao.thongBao = ketQua.ketQua as string;
            }

            return cm_ThongBao;
        }
    }
}
