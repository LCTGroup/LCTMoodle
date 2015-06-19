using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Helpers;
using BUSLayer;
using DTOLayer;
using System.IO;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_ChuDe" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_ChuDe.svc or wcf_ChuDe.svc.cs at the Solution Explorer and start debugging.
    public class wcf_ChuDe : Iwcf_ChuDe
    {
        private string _Loai = "ChuDe_HinhDaiDien";

        /// <summary>
        /// Webservice lấy mã chủ đề cha
        /// với byte[] : hình đại diện
        /// </summary>
        /// <param name="_MaCha"></param>
        /// <returns>Dictionary<DTOLayer.ChuDeDTO, byte[]></returns>
        public Dictionary<DTOLayer.ChuDeDTO, byte[]> layChuDeCha(int _MaCha)
        {
            Dictionary<ChuDeDTO, byte[]> dict_ChuDe = new Dictionary<ChuDeDTO, byte[]>();
            KetQua ketQua = ChuDeBUS.layTheoMaCha(_MaCha, new LienKet() { "HinhDaiDien" });
            List<ChuDeDTO> lst_ChuDe = ketQua.ketQua as List<ChuDeDTO>;

            if (ketQua.trangThai == 0)
            {
                foreach (ChuDeDTO chuDe in lst_ChuDe)
                {
                    dict_ChuDe.Add(chuDe, layHinhDaiDien(_Loai, chuDe.hinhDaiDien.ma + chuDe.hinhDaiDien.duoi));
                }
            }
            return dict_ChuDe;
        }

        /// <summary>
        /// Webservice lấy ảnh đại điện
        /// với _Ten = ChuDe.HinhDaiDien.Ma + ChuDe.HinhDaiDien.Duoi
        /// </summary>
        /// <param name="_Loai"></param>
        /// <param name="_Ten"></param>
        /// <returns>byte[]</returns>
        public byte[] layHinhDaiDien(string _Loai, string _Ten)
        {
            string _LienKet = TapTinHelper.layDuongDan(_Loai, _Ten);
            System.Drawing.Image img = System.Drawing.Image.FromFile(@_LienKet);
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
