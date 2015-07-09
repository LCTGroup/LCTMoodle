using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using BUSLayer;
using DTOLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_CauHoi" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_CauHoi.svc or wcf_CauHoi.svc.cs at the Solution Explorer and start debugging.
    public class wcf_CauHoi : Iwcf_CauHoi
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
        /// Webservice lấy câu hỏi theo mã
        /// </summary>
        /// <param name="_Ma"></param>
        /// <returns>CauHoiDTO</returns>
        public clientmodel_CauHoi layTheoMa(int _Ma)
        {
            KetQua ketQua = CauHoiBUS.layTheoMa(_Ma, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            CauHoiDTO dto_CauHoi = ketQua.ketQua as CauHoiDTO;
            clientmodel_CauHoi cm_CauHoi = new clientmodel_CauHoi();

            if(ketQua.trangThai == 0)
            {
                cm_CauHoi.Ma = dto_CauHoi.ma.Value;
                cm_CauHoi.TieuDe = dto_CauHoi.tieuDe;
                cm_CauHoi.NoiDung = dto_CauHoi.noiDung;
                cm_CauHoi.NguoiTao = dto_CauHoi.nguoiTao.tenTaiKhoan;
                cm_CauHoi.SoTraLoi = dto_CauHoi.soLuongTraLoi.Value;
                cm_CauHoi.NgayTao = dto_CauHoi.thoiDiemTao.Value.ToString();
                cm_CauHoi.NgayCapNhat = dto_CauHoi.thoiDiemCapNhat.Value.ToString();
                cm_CauHoi.HinhAnh = dto_CauHoi.nguoiTao.hinhDaiDien.ma + dto_CauHoi.nguoiTao.hinhDaiDien.duoi;
            }
            return cm_CauHoi;
        }

        /// <summary>
        /// Webservice lấy danh sách trả lời của câu hỏi
        /// </summary>
        /// <param name="_Ma"></param>
        /// <returns>List<TraLoiDTO></returns>
        public List<clientmodel_TraLoi> layTraLoiTheoMaCauHoi(int _Ma)
        {
            KetQua ketQua = TraLoiBUS.layTheoMaCauHoi(_Ma, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_TraLoi> lst_CauHoi = new List<clientmodel_TraLoi>();

            if(ketQua.trangThai == 0)
            {
                foreach(var traLoi in ketQua.ketQua as List<TraLoiDTO>)
                {
                    lst_CauHoi.Add(new clientmodel_TraLoi()
                    {
                        Ma = traLoi.ma.Value,
                        NguoiTao = traLoi.nguoiTao.tenTaiKhoan,
                        NgayTao = traLoi.thoiDiemTao.Value.ToString(),
                        NgayCapNhat = traLoi.thoiDiemCapNhat.Value.ToString(),
                        Duyet = traLoi.duyet,
                        NoiDung = traLoi.noiDung,
                        HinhAnh = traLoi.nguoiTao.hinhDaiDien.ma + traLoi.nguoiTao.hinhDaiDien.duoi,
                    });
                }
            }
            return lst_CauHoi;
        }

        /// <summary>
        /// Webservice lấy danh sách câu hỏi
        /// </summary>
        /// <returns>List<CauHoiDTO></returns>
        public List<clientmodel_CauHoi> lay(int _SoPT)
        {
            KetQua ketQua = CauHoiBUS.layDanhSach(_SoPT, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_CauHoi> lst_CauHoi = new List<clientmodel_CauHoi>();

            if (ketQua.trangThai == 0)
            {
                foreach(var cauHoi in ketQua.ketQua as List<CauHoiDTO>)
                {
                    lst_CauHoi.Add(new clientmodel_CauHoi()
                    {
                        Ma = cauHoi.ma.Value,
                        TieuDe = cauHoi.tieuDe,
                        NoiDung = cauHoi.noiDung,
                        NguoiTao = cauHoi.nguoiTao.tenTaiKhoan,
                        SoTraLoi = cauHoi.soLuongTraLoi.Value,
                        NgayTao = cauHoi.thoiDiemTao.Value.ToString(),
                        NgayCapNhat = cauHoi.thoiDiemCapNhat.Value.ToString(),
                        HinhAnh = cauHoi.nguoiTao.hinhDaiDien.ma + cauHoi.nguoiTao.hinhDaiDien.duoi,
                    });
                }
            }
            return lst_CauHoi;
        }

        /// <summary>
        /// Webservice tìm kiếm câu hỏi
        /// </summary>
        /// <param name="_TuKhoa"></param>
        /// <returns>List<CauHoiDTO></returns>
        public List<clientmodel_CauHoi> timKiem(string _TuKhoa)
        {
            KetQua ketQua = CauHoiBUS.lay_TimKiem(_TuKhoa, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<clientmodel_CauHoi> lst_CauHoi = new List<clientmodel_CauHoi>();

            if(ketQua.trangThai == 0)
            {
                foreach(var cauHoi in ketQua.ketQua as List<CauHoiDTO>)
                {
                    lst_CauHoi.Add(new clientmodel_CauHoi()
                    {
                        Ma = cauHoi.ma.Value,
                        TieuDe = cauHoi.tieuDe,
                        NoiDung = cauHoi.noiDung,
                        NguoiTao = cauHoi.nguoiTao.tenTaiKhoan,
                        SoTraLoi = cauHoi.soLuongTraLoi.Value,
                        NgayTao = cauHoi.thoiDiemTao.Value.ToString(),
                        NgayCapNhat = cauHoi.thoiDiemCapNhat.Value.ToString(),
                        HinhAnh = cauHoi.nguoiTao.hinhDaiDien.ma + cauHoi.nguoiTao.hinhDaiDien.duoi,
                    });
                }
            }
            return lst_CauHoi;
        }

        /// <summary>
        /// Webservice tìm kiếm câu hỏi theo mã chủ đề
        /// </summary>
        /// <param name="_MaChuDe"></param>
        /// <returns>List<CauHoiDTO></returns>
        public List<CauHoiDTO> timKiemTheoChuDe(int _MaChuDe, string _TuKhoa)
        {
            KetQua ketQua = CauHoiBUS.layTheoMaChuDe_TimKiem(_MaChuDe, _TuKhoa, new LienKet() { { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            List<CauHoiDTO> lst_CauHoi = new List<CauHoiDTO>();

            if (ketQua.trangThai == 0)
            {
                lst_CauHoi = ketQua.ketQua as List<CauHoiDTO>;
            }
            return lst_CauHoi;
        }
    }
}
