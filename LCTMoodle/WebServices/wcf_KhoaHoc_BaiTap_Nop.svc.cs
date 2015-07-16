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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc_BaiTap_Nop" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc_BaiTap_Nop.svc or wcf_KhoaHoc_BaiTap_Nop.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc_BaiTap_Nop : Iwcf_KhoaHoc_BaiTap_Nop
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
        /// Lấy danh sách bài tập đã nộp
        /// </summary>
        /// <param name="maBaiTap"></param>
        /// <returns>List<clientmodel_KhoaHoc_BaiTap_Nop></returns>
        public List<clientmodel_KhoaHoc_BaiTap_Nop> layDanhSachTheoMaBaiTap(int maBaiTap)
        {
            KetQua ketQua = BaiTapNopBUS.layTheoMaBaiVietBaiTap(maBaiTap, new LienKet() { "TapTin", { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_KhoaHoc_BaiTap_Nop> lst_Nop = new List<clientmodel_KhoaHoc_BaiTap_Nop>();
            
            if(ketQua.trangThai == 0)
            {
                foreach(var nop in ketQua.ketQua as List<BaiTapNopDTO>)
                {
                    if(nop.ma != null)
                    {
                        lst_Nop.Add(new clientmodel_KhoaHoc_BaiTap_Nop(){
                            ma = nop.ma.Value,
                        });
                    }

                    if(nop.nguoiTao.ma != null)
                    {
                        lst_Nop[lst_Nop.Count - 1].maNguoitao = nop.nguoiTao.ma.Value;
                    }

                    if (nop.nguoiTao.tenLot != null)
                    {
                        lst_Nop[lst_Nop.Count - 1].tenNguoiNop = string.Format("{0} {1} {2}", nop.nguoiTao.ho, nop.nguoiTao.tenLot, nop.nguoiTao.ten);
                    }
                    else
                    {
                        lst_Nop[lst_Nop.Count - 1].tenNguoiNop = string.Format("{0} {1}", nop.nguoiTao.ho, nop.nguoiTao.ten);
                    }

                    if(nop.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_Nop[lst_Nop.Count - 1].tenTaiKhoan = nop.nguoiTao.tenTaiKhoan;
                    }
                    
                    if(nop.nguoiTao.hinhDaiDien != null)
                    {
                        if (nop.nguoiTao.hinhDaiDien.ma != null && nop.nguoiTao.hinhDaiDien.duoi != null)
                        {
                            lst_Nop[lst_Nop.Count - 1].anhNguoiTao = nop.nguoiTao.hinhDaiDien.ma.Value + nop.nguoiTao.hinhDaiDien.duoi;
                        }
                    }

                    if(nop.tapTin != null)
                    {
                        if (nop.tapTin.ten != null)
                        {
                            lst_Nop[lst_Nop.Count - 1].tenFile = nop.tapTin.ten;
                        }

                        if(nop.tapTin.ma != null)
                        {
                            lst_Nop[lst_Nop.Count - 1].maFile = nop.tapTin.ma.Value;
                        }
                    }

                    if(nop.thoiDiemTao != null)
                    {
                        lst_Nop[lst_Nop.Count - 1].ngayTao = nop.thoiDiemTao.Value;
                    }

                    if(nop.diem != null)
                    {
                        lst_Nop[lst_Nop.Count - 1].diem = nop.diem.Value;
                    }

                    if(nop.duongDan != null)
                    {
                        lst_Nop[lst_Nop.Count - 1].duongDan = nop.duongDan;
                    }

                    if(nop.ghiChu != null)
                    {
                        lst_Nop[lst_Nop.Count - 1].ghiChu = nop.ghiChu;
                    }
                }
            }
            return lst_Nop;
        }
    }
}
