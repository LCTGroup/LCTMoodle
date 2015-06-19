using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using BUSLayer;
using DTOLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc.svc or wcf_KhoaHoc.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc : Iwcf_KhoaHoc
    {
        private string _Loai = "KhoaHoc_HinhDaiDien";
   
        public List<KhoaHocDTO> layKhoaHoc()
        {
            KetQua ketQua = KhoaHocBUS.lay();
            List<KhoaHocDTO> lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            return lst_KhoaHoc;
        }

        public KhoaHocDTO layKhoaHocTheoMa(int _Ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(_Ma, new LienKet() { "HinhDaiDien" });
            KhoaHocDTO dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            return dto_KhoaHoc;
        }


        public string layLK(int _Ma)
        {
            //return TapTinHelper.layDuongDan("KhoaHoc_HinhDaiDien", "1 jpg");

            //KetQua ketQua = KhoaHocBUS.lay(new LienKet() { "HinhDaiDien" });
            //List<KhoaHocDTO> lstKhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            //return lstKhoaHoc;

            KetQua ketQua = KhoaHocBUS.layTheoMa(_Ma, new LienKet() { "HinhDaiDien" });
            KhoaHocDTO dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            string lk;
            lk = TapTinHelper.layDuongDan("KhoaHoc_HinhDaiDien", dto_KhoaHoc.hinhDaiDien.ma + dto_KhoaHoc.hinhDaiDien.duoi);
            return lk;
        }


        public byte[] layHinhAnh(string _Loai, string _Ten)
        {
            string _LienKet = TapTinHelper.layDuongDan(_Loai, _Ten);
            System.Drawing.Image img = System.Drawing.Image.FromFile(_LienKet);
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }


        public Dictionary<KhoaHocDTO, byte[]> layKhoaHoc15(int _Dau, int _Cuoi)
        {
            Dictionary<KhoaHocDTO, byte[]> dict_KhoaHoc = new Dictionary<KhoaHocDTO, byte[]>();
            for(int i = _Dau ; i < _Cuoi ; i++)
            {
                KhoaHocDTO dto_KhoaHoc = layKhoaHocTheoMa(i);
                dict_KhoaHoc.Add(dto_KhoaHoc, layHinhAnh(_Loai, dto_KhoaHoc.hinhDaiDien.ma + dto_KhoaHoc.hinhDaiDien.duoi));
            }
            return dict_KhoaHoc;
        }
    }
}
