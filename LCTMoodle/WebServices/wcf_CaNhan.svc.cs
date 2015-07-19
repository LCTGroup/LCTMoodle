using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using LCTMoodle.LCTView;
using DTOLayer;
using BUSLayer;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_CaNhan" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_CaNhan.svc or wcf_CaNhan.svc.cs at the Solution Explorer and start debugging.
    public class wcf_CaNhan : Iwcf_CaNhan
    {
        /// <summary>
        /// Webservice lấy hoạt động câu hỏi
        /// </summary>
        /// <param name="maNguoiDung"></param>
        /// <returns>List<clientmodel_CaNhan> </returns>
        public List<clientmodel_CaNhan> cauHoi(int maNguoiDung)
        {
            KetQua ketQuaDSMa = CauHoiBUS.layCauHoi_DanhSachMaLienQuan(maNguoiDung);
            List<clientmodel_CaNhan> lst_CaNhan = new List<clientmodel_CaNhan>();

            if(ketQuaDSMa.trangThai == 0)
            {
                KetQua ketQua = HoatDongBUS.lay_CuaDanhSachDoiTuong("CH", ketQuaDSMa.ketQua as string, 1, 3, new LienKet { "HanhDong" });

                if (ketQua.trangThai == 0)
                {
                    foreach (var cauHoi in ketQua.ketQua as List<HoatDongDTO>)
                    {
                        lst_CaNhan.Add(new clientmodel_CaNhan()
                        {
                            loai = 0,
                            noiDung = HoatDongView.text(cauHoi, maNguoiDung),
                        });

                        if (cauHoi.loaiDoiTuongTacDong == "CH")
                        {
                            lst_CaNhan[lst_CaNhan.Count - 1].ma = cauHoi.maDoiTuongTacDong.Value;
                        }

                        if (cauHoi.loaiDoiTuongBiTacDong == "CH")
                        {
                            lst_CaNhan[lst_CaNhan.Count - 1].ma = cauHoi.maDoiTuongBiTacDong.Value;
                        }
                    }
                }
            }

            return lst_CaNhan;
        }

        /// <summary>
        /// Webservice lấy hoạt động trả lời
        /// </summary>
        /// <param name="maNguoiDung"></param>
        /// <returns>List<clientmodel_CaNhan></returns>
        public List<clientmodel_CaNhan> traLoi(int maNguoiDung)
        {
            KetQua ketQuaDSMa = TraLoiBUS.layTraLoi_DanhSachMaLienQuan(maNguoiDung);
            List<clientmodel_CaNhan> lst_CaNhan = new List<clientmodel_CaNhan>();

            if (ketQuaDSMa.trangThai == 0)
            {
                KetQua ketQua = HoatDongBUS.lay_CuaDanhSachDoiTuong("TL", ketQuaDSMa.ketQua as string, 1, 3, new LienKet { "HanhDong" });

                if (ketQua.trangThai == 0)
                {
                    foreach (var cauHoi in ketQua.ketQua as List<HoatDongDTO>)
                    {
                        lst_CaNhan.Add(new clientmodel_CaNhan()
                        {
                            loai = 0,
                            noiDung = HoatDongView.text(cauHoi, maNguoiDung),
                        });

                        if (cauHoi.loaiDoiTuongTacDong == "TL")
                        {
                            lst_CaNhan[lst_CaNhan.Count - 1].ma = cauHoi.maDoiTuongTacDong.Value;
                        }

                        if (cauHoi.loaiDoiTuongBiTacDong == "TL")
                        {
                            lst_CaNhan[lst_CaNhan.Count - 1].ma = cauHoi.maDoiTuongBiTacDong.Value;
                        }
                    }
                }
            }

            return lst_CaNhan;
        }

        /// <summary>
        /// Websservice lây hành động bài tập
        /// </summary>
        /// <param name="maNguoiDung"></param>
        /// <returns></returns>
        public List<clientmodel_KhoaHoc_BaiTap> khoaHoc(int maNguoiDung)
        {
            KetQua ketQua = BaiVietBaiTapBUS.layDanhSachCanHoanThanh(maNguoiDung);
            List<clientmodel_KhoaHoc_BaiTap> lst_CaNhan = new List<clientmodel_KhoaHoc_BaiTap>();

            if(ketQua.trangThai == 0)
            {
                foreach(var baiTap in ketQua.ketQua as List<KhoaHocDTO>)
                {

                }
            }

            return lst_CaNhan;
        }
    }
}
