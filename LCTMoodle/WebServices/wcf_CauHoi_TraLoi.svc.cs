using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Helpers;
using DTOLayer;
using BUSLayer;
using LCTMoodle.WebServices.Client_Model;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_CauHoi_TraLoi" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_CauHoi_TraLoi.svc or wcf_CauHoi_TraLoi.svc.cs at the Solution Explorer and start debugging.
    public class wcf_CauHoi_TraLoi : Iwcf_CauHoi_TraLoi
    {
        private const string _Loai = "NguoiDung_HinhDaiDien";

        /// <summary>
        /// Webservice lấy hình ảnh
        /// </summary>
        /// <param name="_Ten"></param>
        /// <returns>byte[]</returns>
        public byte[] layHinhAnh(string _Ten)
        {
            string _DuongDan = TapTinHelper.layDuongDan(_Loai, _Ten);

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
        /// Webservice lấy trả lời theo mã câu hỏi
        /// </summary>
        /// <param name="ma"></param>
        /// <returns>List<clientmodel_CauHoi_TraLoi></returns>
        public List<clientmodel_CauHoi_TraLoi> layTheoMaCauHoi(int ma)
        {
            KetQua ketQua = TraLoiBUS.layTheoMaCauHoi(ma, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_CauHoi_TraLoi> lst_CauHoi_TraLoi = new List<clientmodel_CauHoi_TraLoi>();
 
            if(ketQua.trangThai == 0)
            {
                foreach(var traLoi in ketQua.ketQua as List<TraLoiDTO>)
                {
                    if(traLoi.ma != null)
                    {
                        lst_CauHoi_TraLoi.Add(new clientmodel_CauHoi_TraLoi()
                        {
                            ma = traLoi.ma.Value,
                        });
                    }
                    
                    if(traLoi.noiDung != null)
                    {
                        lst_CauHoi_TraLoi[lst_CauHoi_TraLoi.Count - 1].noiDung = traLoi.noiDung;
                    }

                    if(traLoi.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_CauHoi_TraLoi[lst_CauHoi_TraLoi.Count - 1].nguoiTao = traLoi.nguoiTao.tenTaiKhoan;
                    }

                    if(traLoi.thoiDiemTao != null)
                    {
                        lst_CauHoi_TraLoi[lst_CauHoi_TraLoi.Count - 1].ngayTao = traLoi.thoiDiemTao.Value;
                    }

                    if(traLoi.thoiDiemCapNhat != null)
                    {
                        lst_CauHoi_TraLoi[lst_CauHoi_TraLoi.Count - 1].ngayCapNhat = traLoi.thoiDiemCapNhat.Value;
                    }

                    if(traLoi.nguoiTao.hinhDaiDien.ma != null && traLoi.nguoiTao.hinhDaiDien.duoi != null)
                    {
                        lst_CauHoi_TraLoi[lst_CauHoi_TraLoi.Count - 1].hinhAnh = traLoi.nguoiTao.hinhDaiDien.ma.Value + traLoi.nguoiTao.hinhDaiDien.duoi;
                    }
                }
            }

            return lst_CauHoi_TraLoi;
        }
    }
}
