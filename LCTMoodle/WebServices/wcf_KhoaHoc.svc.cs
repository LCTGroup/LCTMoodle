using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;
using BUSLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    public class wcf_KhoaHoc : Iwcf_KhoaHoc
    {
        private const string _Loai = "KhoaHoc_HinhDaiDien";

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
        /// Webservice lấy khóa học theo mã
        /// </summary>
        /// <param name="_Ma"></param>
        /// <returns>KhoaHocDTO</returns>
        public KhoaHocDTO layTheoMa(int _Ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(_Ma, new LienKet { "HinhDaiDien" });
            KhoaHocDTO dto_KhoaHoc = new KhoaHocDTO();

            if(ketQua.trangThai == 0)
            {
                dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            }
            return dto_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy tất cả khóa học
        /// </summary>
        /// <returns>List<KhoaHocDTO></returns>
        public List<KhoaHocDTO> lay()
        {
            KetQua ketQua = KhoaHocBUS.lay(new LienKet { "HinhDaiDien" });
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học theo mã chủ đề
        /// </summary>
        /// <param name="_MaChuDe"></param>
        /// <returns>List<KhoaHocDTO> </returns>
        public List<KhoaHocDTO> layTheoMaChuDe(int _MaChuDe)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMaChuDe(_MaChuDe, new LienKet { "HinhDaiDien" });
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học theo mã người dùng
        /// </summary>
        /// <param name="_MaNguoiDung"></param>
        /// <returns>List<KhoaHocDTO> </returns>
        public List<KhoaHocDTO> layTheoMaNguoiDung(int _MaNguoiDung)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMaNguoiDung(_MaNguoiDung);
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice lấy khóa học theo mã người dùng và trạng thái
        /// </summary>
        /// <param name="_MaNguoiDung"></param>
        /// <param name="_TrangThai"></param>
        /// <returns>List<KhoaHocDTO></returns>
        public List<KhoaHocDTO> layTheoMaNguoiDungVaTrangThai(int _MaNguoiDung, int _TrangThai)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMaNguoiDungVaTrangThai(_MaNguoiDung, _TrangThai);
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice tìm kiếm khóa học
        /// </summary>
        /// <param name="_TuKhoa"></param>
        /// <returns>List<KhoaHocDTO> </returns>
        public List<KhoaHocDTO> timKiem(string _TuKhoa)
        {
            KetQua ketQua = KhoaHocBUS.lay_TimKiem(_TuKhoa, new LienKet { "HinhDaiDien" });
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

        /// <summary>
        /// Webservice tìm kiếm theo mã chủ đề
        /// </summary>
        /// <param name="_TuKhoa"></param>
        /// <returns>List<KhoaHocDTO> </returns>
        public List<KhoaHocDTO> timKiemTheoMaChuDe(int _MaChuDe,string _TuKhoa)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMaChuDe_TimKiem(_MaChuDe, _TuKhoa, new LienKet { "HinhDaiDien" });
            List<KhoaHocDTO> lst_KhoaHoc = new List<KhoaHocDTO>();

            if(ketQua.trangThai == 0)
            {
                lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            }
            return lst_KhoaHoc;
        }

    }
}
