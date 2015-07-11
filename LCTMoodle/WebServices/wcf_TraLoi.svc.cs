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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_TraLoi" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_TraLoi.svc or wcf_TraLoi.svc.cs at the Solution Explorer and start debugging.
    public class wcf_TraLoi : Iwcf_TraLoi
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

        public List<clientmodel_TraLoi> layTheoMaCauHoi(int ma)
        {
            KetQua ketQua = TraLoiBUS.layTheoMaCauHoi(ma, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_TraLoi> lst_TraLoi = new List<clientmodel_TraLoi>();

            if(ketQua.trangThai == 0)
            {
                foreach(var traLoi in ketQua.ketQua as List<TraLoiDTO>)
                {
                    if(traLoi.ma != null)
                    {
                        lst_TraLoi.Add(new clientmodel_TraLoi()
                        {
                            ma = traLoi.ma.Value,
                            duyet = traLoi.duyet,
                        });
                    }

                    if(traLoi.noiDung != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].noiDung = traLoi.noiDung;
                    }

                    if(traLoi.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].nguoiTao = traLoi.nguoiTao.tenTaiKhoan;
                    }

                    if(traLoi.thoiDiemTao != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].ngayTao = traLoi.thoiDiemTao.Value;
                    }

                    if(traLoi.thoiDiemCapNhat != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].ngayCapNhat = traLoi.thoiDiemCapNhat.Value;
                    }


                    if(traLoi.nguoiTao.hinhDaiDien.ma != null && traLoi.nguoiTao.hinhDaiDien.duoi != null)
                    {
                        lst_TraLoi[lst_TraLoi.Count - 1].hinhAnh = traLoi.nguoiTao.hinhDaiDien.ma.Value + traLoi.nguoiTao.hinhDaiDien.duoi;
                    }
                }
            }

            return lst_TraLoi;
        }
    }
}
