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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_TinNhan" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_TinNhan.svc or wcf_TinNhan.svc.cs at the Solution Explorer and start debugging.
    public class wcf_TinNhan : Iwcf_TinNhan
    {
        private const string _Loai = "NguoiDung_HinhDaiDien";

        /// <summary>
        /// Webservice lấy hình ảnh
        /// </summary>
        /// <param name="ten"></param>
        /// <returns>byte[] </returns>
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
        /// Webservice lấy danh sách tin nhắn theo người dùng
        /// </summary>
        /// <param name="maNguoiDung"></param>
        /// <returns>List<clientmodel_TinNhan></returns>
        public List<clientmodel_TinNhan> layDanhSachTheoMaNguoiDung(int maNguoiDung)
        {
            KetQua ketQua = TinNhanBUS.layDanhSachTinNhanTheoMaNguoiDung(maNguoiDung, new LienKet() { { "NguoiDung", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_TinNhan> lst_TinNhan = new List<clientmodel_TinNhan>();

            if(ketQua.trangThai == 0)
            {
                foreach(var tinNhan in ketQua.ketQua as List<TinNhanDTO>)
                {
                    lst_TinNhan.Add(new clientmodel_TinNhan()
                    {
                        ma = tinNhan.ma.Value,
                        daDoc = tinNhan.daDoc,
                    });

                    if(tinNhan.noiDung != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].noiDung = tinNhan.noiDung;
                    }

                    if(tinNhan.thoiDiemGui != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].ngayGui = tinNhan.thoiDiemGui.Value;
                    }

                    if(tinNhan.nguoiGui != null)
                    {
                        if(tinNhan.nguoiGui.ma != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].maNguoiGui = tinNhan.nguoiGui.ma.Value;
                        }

                        if(tinNhan.nguoiGui.tenLot != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1} {2}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.tenLot, tinNhan.nguoiGui.ten);
                        }
                        else
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.ten);
                        }

                        if(tinNhan.nguoiGui.tenTaiKhoan != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].tenTKNguoiGui = tinNhan.nguoiGui.tenTaiKhoan;
                        }

                        if(tinNhan.nguoiGui.gioiTinh != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].gioiTinhNguoiGui = tinNhan.nguoiGui.gioiTinh.Value;
                        }

                        if(tinNhan.nguoiGui.hinhDaiDien != null)
                        {
                            if(tinNhan.nguoiGui.hinhDaiDien.ma != null && tinNhan.nguoiGui.hinhDaiDien.duoi != null)
                            {
                                lst_TinNhan[lst_TinNhan.Count - 1].hinhAnhNguoiGui = tinNhan.nguoiGui.hinhDaiDien.ma.Value + tinNhan.nguoiGui.hinhDaiDien.duoi;
                            }
                        }
                    }

                    if(tinNhan.nguoiNhan != null)
                    {
                        if (tinNhan.nguoiNhan.ma != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].maNguoiNhan = tinNhan.nguoiNhan.ma.Value;
                        }

                        if (tinNhan.nguoiNhan.tenLot != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan = string.Format("{0} {1} {2}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.tenLot, tinNhan.nguoiNhan.ten);
                        }
                        else
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan = string.Format("{0} {1}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.ten);
                        }

                        if (tinNhan.nguoiNhan.tenTaiKhoan != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].tenTKNguoiNhan = tinNhan.nguoiNhan.tenTaiKhoan;
                        }

                        if (tinNhan.nguoiNhan.gioiTinh != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].gioiTinhNguoiNhan = tinNhan.nguoiNhan.gioiTinh.Value;
                        }

                        if (tinNhan.nguoiNhan.hinhDaiDien != null)
                        {
                            if (tinNhan.nguoiNhan.hinhDaiDien.ma != null && tinNhan.nguoiNhan.hinhDaiDien.duoi != null)
                            {
                                lst_TinNhan[lst_TinNhan.Count - 1].hinhAnhNguoiNhan = tinNhan.nguoiNhan.hinhDaiDien.ma.Value + tinNhan.nguoiNhan.hinhDaiDien.duoi;
                            }
                        }
                    }
                }
            }

            return lst_TinNhan;
        }

        /// <summary>
        /// Webservice lấy tin nhắn đã gửi cho người dùng
        /// </summary>
        /// <param name="maNguoiGui"></param>
        /// <param name="maNguoiNhan"></param>
        /// <returns>List<clientmodel_TinNhan></returns>
        public List<clientmodel_TinNhan> layTinNhanChoNguoiDung(int maNguoiGui, int maNguoiNhan)
        {
            KetQua ketQua = TinNhanBUS.lay(maNguoiGui, maNguoiNhan, new LienKet() { { "NguoiDung", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_TinNhan> lst_TinNhan = new List<clientmodel_TinNhan>();

            if (ketQua.trangThai == 0)
            {
                foreach (var tinNhan in ketQua.ketQua as List<TinNhanDTO>)
                {
                    lst_TinNhan.Add(new clientmodel_TinNhan()
                    {
                        ma = tinNhan.ma.Value,
                        daDoc = tinNhan.daDoc,
                    });

                    if (tinNhan.noiDung != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].noiDung = tinNhan.noiDung;
                    }

                    if (tinNhan.thoiDiemGui != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].ngayGui = tinNhan.thoiDiemGui.Value;
                    }

                    if (tinNhan.nguoiGui != null)
                    {
                        if (tinNhan.nguoiGui.ma != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].maNguoiGui = tinNhan.nguoiGui.ma.Value;
                        }

                        if (tinNhan.nguoiGui.tenLot != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1} {2}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.tenLot, tinNhan.nguoiGui.ten);
                        }
                        else
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.ten);
                        }

                        if (tinNhan.nguoiGui.tenTaiKhoan != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].tenTKNguoiGui = tinNhan.nguoiGui.tenTaiKhoan;
                        }

                        if (tinNhan.nguoiGui.gioiTinh != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].gioiTinhNguoiGui = tinNhan.nguoiGui.gioiTinh.Value;
                        }

                        if (tinNhan.nguoiGui.hinhDaiDien != null)
                        {
                            if (tinNhan.nguoiGui.hinhDaiDien.ma != null && tinNhan.nguoiGui.hinhDaiDien.duoi != null)
                            {
                                lst_TinNhan[lst_TinNhan.Count - 1].hinhAnhNguoiGui = tinNhan.nguoiGui.hinhDaiDien.ma.Value + tinNhan.nguoiGui.hinhDaiDien.duoi;
                            }
                        }
                    }

                    if (tinNhan.nguoiNhan != null)
                    {
                        if (tinNhan.nguoiNhan.ma != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].maNguoiNhan = tinNhan.nguoiNhan.ma.Value;
                        }

                        if (tinNhan.nguoiNhan.tenLot != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan = string.Format("{0} {1} {2}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.tenLot, tinNhan.nguoiNhan.ten);
                        }
                        else
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan = string.Format("{0} {1}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.ten);
                        }

                        if (tinNhan.nguoiNhan.tenTaiKhoan != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].tenTKNguoiNhan = tinNhan.nguoiNhan.tenTaiKhoan;
                        }

                        if (tinNhan.nguoiNhan.gioiTinh != null)
                        {
                            lst_TinNhan[lst_TinNhan.Count - 1].gioiTinhNguoiNhan = tinNhan.nguoiNhan.gioiTinh.Value;
                        }

                        if (tinNhan.nguoiNhan.hinhDaiDien != null)
                        {
                            if (tinNhan.nguoiNhan.hinhDaiDien.ma != null && tinNhan.nguoiNhan.hinhDaiDien.duoi != null)
                            {
                                lst_TinNhan[lst_TinNhan.Count - 1].hinhAnhNguoiNhan = tinNhan.nguoiNhan.hinhDaiDien.ma.Value + tinNhan.nguoiNhan.hinhDaiDien.duoi;
                            }
                        }
                    }
                }
            }

            return lst_TinNhan;
        }

        /// <summary>
        /// Webservice thêm tin nhắn
        /// </summary>
        /// <param name="maNguoiGui"></param>
        /// <param name="maNguoiNhan"></param>
        /// <param name="noiDung"></param>
        /// <returns>clientmodel_ThongBao</returns>
        public clientmodel_ThongBao themTinNhan(int maNguoiGui, int maNguoiNhan, string noiDung)
        {
            Form form = new Form()
            {
                {"MaNguoiGui",maNguoiGui.ToString()},
                {"MaNguoiNhan",maNguoiNhan.ToString()},
                {"NoiDung",noiDung.ToString()},
            };

            KetQua ketQua = TinNhanBUS.them(form, new LienKet { "NguoiDung" });
            clientmodel_ThongBao cm_ThongBao = new clientmodel_ThongBao();
            cm_ThongBao.trangThai = ketQua.trangThai;
            cm_ThongBao.thongBao = ketQua.ketQua as string;
            return cm_ThongBao;
        }
    }
}
