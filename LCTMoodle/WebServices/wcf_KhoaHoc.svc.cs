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

        

        //public byte[] layHinhAnh(string _Loai, string _Ten)
        //{
        //    string _LienKet = TapTinHelper.layDuongDan(_Loai, _Ten);
        //    System.Drawing.Image img = System.Drawing.Image.FromFile(_LienKet);
        //    using (var ms = new MemoryStream())
        //    {
        //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //        return ms.ToArray();
        //    }
        //}


        //public Dictionary<KhoaHocDTO, byte[]> layKhoaHoc15(int _Dau, int _Cuoi)
        //{
        //    Dictionary<KhoaHocDTO, byte[]> dict_KhoaHoc = new Dictionary<KhoaHocDTO, byte[]>();
        //    for(int i = _Dau ; i < _Cuoi ; i++)
        //    {
        //        KhoaHocDTO dto_KhoaHoc = layKhoaHocTheoMa(i);
        //        dict_KhoaHoc.Add(dto_KhoaHoc, layHinhAnh(_Loai, dto_KhoaHoc.hinhDaiDien.ma + dto_KhoaHoc.hinhDaiDien.duoi));
        //    }
        //    return dict_KhoaHoc;
        //}

        /// <summary>
        /// Webservice lấy khóa học theo mã
        /// </summary>
        /// <param name="_Ma"></param>
        /// <returns>KhoaHocDTO</returns>
        public KhoaHocDTO layKhoaHocTheoMa(int _Ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(_Ma);
            KhoaHocDTO dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            return dto_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học theo mã - hình đại diện
        /// với byte[] : hình đại diện
        /// </summary>
        /// <param name="_Ma"></param>
        /// <returns>Dictionary<KhoaHocDTO, byte[]></returns>
        public Dictionary<KhoaHocDTO, byte[]> layKhoaHocTheoMa_HinhDD(int _Ma)
        {
            Dictionary<KhoaHocDTO, byte[]> dict_KhoaHoc = new Dictionary<KhoaHocDTO, byte[]>();
            KetQua ketQua = KhoaHocBUS.layTheoMa(_Ma, new LienKet { "HinhDaiDien" });
            KhoaHocDTO dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            
            if(ketQua.trangThai == 0)
            {
                dict_KhoaHoc.Add(dto_KhoaHoc, layHinhDaiDien(_Loai, dto_KhoaHoc.hinhDaiDien.ma + dto_KhoaHoc.hinhDaiDien.duoi));
            }

            return dict_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học trong khoảng (_Dau , _Cuoi)
        /// với _Dau : mã khóa học bắt đầu
        /// và _Cuoi : mã khóa học kết thúc
        /// </summary>
        /// <param name="_Dau"></param>
        /// <param name="_Cuoi"></param>
        /// <returns>Dictionary<KhoaHocDTO, byte[]> </returns>
        public Dictionary<KhoaHocDTO, byte[]> layKhoaHoc(int _Dau, int _Cuoi)
        {
            Dictionary<KhoaHocDTO, byte[]> dict_KhoaHoc = new Dictionary<KhoaHocDTO, byte[]>();
            for (int i = _Dau; i <= _Cuoi; i++)
            {
                KhoaHocDTO dto_KhoaHoc = layKhoaHocTheoMa(i);
                dict_KhoaHoc.Add(dto_KhoaHoc, layHinhDaiDien(_Loai, dto_KhoaHoc.hinhDaiDien.ma + dto_KhoaHoc.hinhDaiDien.duoi));
            }
            return dict_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy ảnh đại diện
        /// với _Ten = KhoaHoc.HinhDaiDien.Ma + KhoaHoc.HinhDaiDien.Duoi
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
