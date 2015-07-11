using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Drawing;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;
using BUSLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_ChuDe" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_ChuDe.svc or wcf_ChuDe.svc.cs at the Solution Explorer and start debugging.
    public class wcf_ChuDe : Iwcf_ChuDe
    {
        private const string _Loai = "ChuDe_HinhDaiDien";

        /// <summary>
        /// Webservice lấy hình ảnh
        /// </summary>
        /// <param name="_Ten"></param>
        /// <returns>byte[]</returns>
        public byte[] layHinhAnh(string _Ten)
        {
            string _DuongDan = TapTinHelper.layDuongDan(_Loai, _Ten);

            if(File.Exists(@_DuongDan))
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
        /// <param name="_ChiSo"></param>
        /// <param name="_Ten"></param>
        /// <returns>clientmodel_HinhAnh</returns>
        public clientmodel_HinhAnh layHinhAnhChiSo(int _ChiSo, string _Ten)
        {
            string _DuongDan = TapTinHelper.layDuongDan(_Loai, _Ten);
            clientmodel_HinhAnh cm_HinhAnh = new clientmodel_HinhAnh();

            cm_HinhAnh._ChiSo = _ChiSo;

            if (File.Exists(@_DuongDan))
            {
                Image img = Image.FromFile(@_DuongDan);
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    cm_HinhAnh._HinhAnh = ms.ToArray();
                }
            }

            return cm_HinhAnh;
        }

        /// <summary>
        /// Webservice lấy chủ đề theo mã
        /// </summary>
        /// <param name="_Ma"></param>
        /// <returns>ChuDeDTO</returns>
        public ChuDeDTO layTheoMa(int _Ma)
        {
            KetQua ketQua = ChuDeBUS.layTheoMa(_Ma, new LienKet { "HinhDaiDien" });
            ChuDeDTO dto_ChuDe = new ChuDeDTO();
            
            if(ketQua.trangThai == 0)
            {
                dto_ChuDe = ketQua.ketQua as ChuDeDTO;
            }
            return dto_ChuDe;
        }

        /// <summary>
        /// Webservice lấy chủ đề theo mã chủ đề cha
        /// </summary>
        /// <param name="_MaChuDeCha"></param>
        /// <returns>List<ChuDeDTO></returns>
        public List<clientmodel_ChuDe> layTheoMaCha(int _MaChuDeCha)
        {
            KetQua ketQua = ChuDeBUS.layTheoMaCha(_MaChuDeCha, new LienKet { "HinhDaiDien" });
            List<clientmodel_ChuDe> lst_ChuDe = new List<clientmodel_ChuDe>();

            if(ketQua.trangThai == 0)
            {
                foreach(var chuDe in ketQua.ketQua as List<ChuDeDTO>)
                {
                    if(chuDe.ma != null)
                    {
                        lst_ChuDe.Add(new clientmodel_ChuDe()
                        {
                            Ma = chuDe.ma.Value,
                        });
                    }

                    if(chuDe.ten != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].Ten = chuDe.ten;
                    }

                    if(chuDe.thoiDiemTao != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].NgayTao = chuDe.thoiDiemTao.Value;
                    }

                    if(chuDe.moTa != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].MoTa = chuDe.moTa;
                    }

                    if (chuDe.duLieuThem != null)
                    {
                        if (chuDe.duLieuThem.ContainsKey("SLChuDeCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].SoChuDeCon = (int)chuDe.duLieuThem["SLChuDeCon"];
                        }

                        if (chuDe.duLieuThem.ContainsKey("SLKhoaHocCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].SoKhoaHocCon = (int)chuDe.duLieuThem["SLKhoaHocCon"];
                        }
                    }

                    if(chuDe.hinhDaiDien.ma != null && chuDe.hinhDaiDien.duoi != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].HinhAnh = chuDe.hinhDaiDien.ma.Value + chuDe.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_ChuDe;
        }

        /// <summary>
        /// Webservice tìm kiếm chủ đề
        /// </summary>
        /// <param name="_TuKhoa"></param>
        /// <returns>List<ChuDeDTO></returns>
        public List<clientmodel_ChuDe> timKiem(string _TuKhoa)
        {
            KetQua ketQua = ChuDeBUS.lay_TimKiem(_TuKhoa, new LienKet { "HinhDaiDien" });
            List<clientmodel_ChuDe> lst_ChuDe = new List<clientmodel_ChuDe>();

            if(ketQua.trangThai == 0)
            {
                foreach (var chuDe in ketQua.ketQua as List<ChuDeDTO>)
                {
                    if (chuDe.ma != null)
                    {
                        lst_ChuDe.Add(new clientmodel_ChuDe()
                        {
                            Ma = chuDe.ma.Value,
                        });
                    }

                    if (chuDe.ten != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].Ten = chuDe.ten;
                    }

                    if (chuDe.thoiDiemTao != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].NgayTao = chuDe.thoiDiemTao.Value;
                    }

                    if (chuDe.moTa != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].MoTa = chuDe.moTa;
                    }

                    if(chuDe.duLieuThem != null)
                    {
                        if (chuDe.duLieuThem.ContainsKey("SLChuDeCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].SoChuDeCon = (int)chuDe.duLieuThem["SLChuDeCon"];
                        }

                        if (chuDe.duLieuThem.ContainsKey("SLKhoaHocCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].SoKhoaHocCon = (int)chuDe.duLieuThem["SLKhoaHocCon"];
                        }
                    }

                    if (chuDe.hinhDaiDien.ma != null && chuDe.hinhDaiDien.duoi != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].HinhAnh = chuDe.hinhDaiDien.ma.Value + chuDe.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_ChuDe;
        }
    }
}
