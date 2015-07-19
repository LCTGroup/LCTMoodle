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
        /// <param name="ten"></param>
        /// <returns>byte[]</returns>
        public byte[] layHinhAnh(string ten)
        {
            string _DuongDan = TapTinHelper.layDuongDan(_Loai, ten);

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
        /// Webservice lấy chủ đề theo mã
        /// </summary>
        /// <param name="ma"></param>
        /// <returns>ChuDeDTO</returns>
        public ChuDeDTO layTheoMa(int ma)
        {
            KetQua ketQua = ChuDeBUS.layTheoMa(ma, new LienKet { "HinhDaiDien" });
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
        /// <param name="maChuDeCha"></param>
        /// <returns>List<clientmodel_ChuDe></returns>
        public List<clientmodel_ChuDe> layTheoMaCha(int maChuDeCha)
        {
            KetQua ketQua = ChuDeBUS.layTheoMaCha(maChuDeCha, new LienKet { "HinhDaiDien" });
            List<clientmodel_ChuDe> lst_ChuDe = new List<clientmodel_ChuDe>();

            if(ketQua.trangThai == 0)
            {
                foreach(var chuDe in ketQua.ketQua as List<ChuDeDTO>)
                {
                    if(chuDe.ma != null)
                    {
                        lst_ChuDe.Add(new clientmodel_ChuDe()
                        {
                            ma = chuDe.ma.Value,
                        });
                    }

                    if(chuDe.ten != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].ten = chuDe.ten;
                    }

                    if(chuDe.thoiDiemTao != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].ngayTao = chuDe.thoiDiemTao.Value;
                    }

                    if(chuDe.moTa != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].moTa = chuDe.moTa;
                    }

                    if (chuDe.duLieuThem != null)
                    {
                        if (chuDe.duLieuThem.ContainsKey("SLChuDeCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].soChuDeCon = (int)chuDe.duLieuThem["SLChuDeCon"];
                        }

                        if (chuDe.duLieuThem.ContainsKey("SLKhoaHocCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].soKhoaHocCon = (int)chuDe.duLieuThem["SLKhoaHocCon"];
                        }
                    }

                    if(chuDe.hinhDaiDien != null)
                    {
                        if (chuDe.hinhDaiDien.ma != null && chuDe.hinhDaiDien.duoi != null)
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].hinhAnh = chuDe.hinhDaiDien.ma.Value + chuDe.hinhDaiDien.duoi;
                        }
                    }
                }
            }
            return lst_ChuDe;
        }

        /// <summary>
        /// Webservice tìm kiếm chủ đề
        /// </summary>
        /// <param name="tuKhoa"></param>
        /// <returns>List<clientmodel_ChuDe></returns>
        public List<clientmodel_ChuDe> timKiem(string tuKhoa)
        {
            KetQua ketQua = ChuDeBUS.lay_TimKiem(tuKhoa, new LienKet { "HinhDaiDien" });
            List<clientmodel_ChuDe> lst_ChuDe = new List<clientmodel_ChuDe>();

            if(ketQua.trangThai == 0)
            {
                foreach (var chuDe in ketQua.ketQua as List<ChuDeDTO>)
                {
                    if (chuDe.ma != null)
                    {
                        lst_ChuDe.Add(new clientmodel_ChuDe()
                        {
                           ma = chuDe.ma.Value,
                        });
                    }

                    if (chuDe.ten != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].ten = chuDe.ten;
                    }

                    if (chuDe.thoiDiemTao != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].ngayTao = chuDe.thoiDiemTao.Value;
                    }

                    if (chuDe.moTa != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].moTa = chuDe.moTa;
                    }

                    if(chuDe.duLieuThem != null)
                    {
                        if (chuDe.duLieuThem.ContainsKey("SLChuDeCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].soChuDeCon = (int)chuDe.duLieuThem["SLChuDeCon"];
                        }

                        if (chuDe.duLieuThem.ContainsKey("SLKhoaHocCon"))
                        {
                            lst_ChuDe[lst_ChuDe.Count - 1].soKhoaHocCon = (int)chuDe.duLieuThem["SLKhoaHocCon"];
                        }
                    }

                    if (chuDe.hinhDaiDien.ma != null && chuDe.hinhDaiDien.duoi != null)
                    {
                        lst_ChuDe[lst_ChuDe.Count - 1].hinhAnh = chuDe.hinhDaiDien.ma.Value + chuDe.hinhDaiDien.duoi;
                    }
                }
            }
            return lst_ChuDe;
        }
    }
}
