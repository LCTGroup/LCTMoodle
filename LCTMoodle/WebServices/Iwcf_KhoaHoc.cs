using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;

namespace LCTMoodle.WebServices
{
    [ServiceContract]
    public interface Iwcf_KhoaHoc
    {
        [OperationContract]
        byte[] layHinhAnh(string _Ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int _ChiSo, string _Ten);

        [OperationContract]
        KhoaHocDTO layTheoMa(int _Ma);

        [OperationContract]
        List<KhoaHocDTO> lay();

        [OperationContract]
        List<KhoaHocDTO> layTheoMaChuDe(int _MaChuDe);

        [OperationContract]
        List<KhoaHocDTO> layTheoMaNguoiDung(int _MaNguoiDung);

        [OperationContract]
        List<KhoaHocDTO> layTheoMaNguoiDungVaTrangThai(int _MaNguoiDung, int _TrangThai);

        [OperationContract]
        List<KhoaHocDTO> timKiem(string _TuKhoa);

        [OperationContract]
        List<KhoaHocDTO> timKiemTheoMaChuDe(int _MaChuDe, string _TuKhoa);
    }
}
