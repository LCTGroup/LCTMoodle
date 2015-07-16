using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using BUSLayer;
using DTOLayer;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_TinNhan" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_TinNhan.svc or wcf_TinNhan.svc.cs at the Solution Explorer and start debugging.
    public class wcf_TinNhan : Iwcf_TinNhan
    {
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
                        noiDung = tinNhan.noiDung,
                        ngayGui = tinNhan.thoiDiemGui.Value,
                        tenTKNguoiGui = tinNhan.nguoiGui.tenTaiKhoan,
                        gioiTinhNguoiGui = tinNhan.nguoiGui.gioiTinh.Value,
                        hinhAnhNguoiGui = tinNhan.nguoiGui.hinhDaiDien.ma.Value + tinNhan.nguoiGui.hinhDaiDien.duoi,
                        tenTKNguoiNhan = tinNhan.nguoiNhan.tenTaiKhoan,
                        gioiTinhNguoiNhan = tinNhan.nguoiNhan.gioiTinh.Value,
                        hinhAnhNguoiNhan = tinNhan.nguoiNhan.hinhDaiDien.ma.Value + tinNhan.nguoiNhan.hinhDaiDien.duoi,
                    });

                    if(tinNhan.nguoiGui.tenLot != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1} {2}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.tenLot, tinNhan.nguoiGui.ten);
                    }
                    else
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.ten);
                    }

                    if (tinNhan.nguoiNhan.tenLot != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan= string.Format("{0} {1} {2}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.tenLot, tinNhan.nguoiNhan.ten);
                    }
                    else
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan = string.Format("{0} {1}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.ten);
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
                        noiDung = tinNhan.noiDung,
                        ngayGui = tinNhan.thoiDiemGui.Value,
                        tenTKNguoiGui = tinNhan.nguoiGui.tenTaiKhoan,
                        gioiTinhNguoiGui = tinNhan.nguoiGui.gioiTinh.Value,
                        hinhAnhNguoiGui = tinNhan.nguoiGui.hinhDaiDien.ma.Value + tinNhan.nguoiGui.hinhDaiDien.duoi,
                        tenTKNguoiNhan = tinNhan.nguoiNhan.tenTaiKhoan,
                        gioiTinhNguoiNhan = tinNhan.nguoiNhan.gioiTinh.Value,
                        hinhAnhNguoiNhan = tinNhan.nguoiNhan.hinhDaiDien.ma.Value + tinNhan.nguoiNhan.hinhDaiDien.duoi,
                    });

                    if (tinNhan.nguoiGui.tenLot != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1} {2}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.tenLot, tinNhan.nguoiGui.ten);
                    }
                    else
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiGui = string.Format("{0} {1}", tinNhan.nguoiGui.ho, tinNhan.nguoiGui.ten);
                    }

                    if (tinNhan.nguoiNhan.tenLot != null)
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan = string.Format("{0} {1} {2}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.tenLot, tinNhan.nguoiNhan.ten);
                    }
                    else
                    {
                        lst_TinNhan[lst_TinNhan.Count - 1].hoTenNguoiNhan = string.Format("{0} {1}", tinNhan.nguoiNhan.ho, tinNhan.nguoiNhan.ten);
                    }
                }
            }

            return lst_TinNhan;
        }

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
            return cm_ThongBao;
        }
    }
}
