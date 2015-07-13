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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_CauHoi" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_CauHoi
    {
        [OperationContract]
        byte[] layHinhAnh(string ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int chiSo, string ten);

        [OperationContract]
        clientmodel_CauHoi layTheoMa(int ma);

        [OperationContract]
        List<clientmodel_CauHoi> layTheoMaNguoiTao(int maNguoiTao);

        [OperationContract]
        List<clientmodel_CauHoi> layMoiNhat(int soPT);

        [OperationContract]
        List<clientmodel_CauHoi> layTheoTieuChi(string tieuChi, int soCauHoi);

        [OperationContract]
        List<clientmodel_CauHoi> layDanhSachDuyet();

        [OperationContract]
        List<clientmodel_CauHoi> timKiem(string tuKhoa);

        [OperationContract]
        List<CauHoiDTO> timKiemTheoChuDe(int maChuDe, string tuKhoa);

        [OperationContract]
        int themCauHoi(int maNguoiTao, int maChuDe, string tieuDe, string noiDung);

        [OperationContract]
        clientmodel_ThongBao xoaCauHoi(int maCauHoi, int maNguoiDung);

        [OperationContract]
        clientmodel_ThongBao suaCauHoi(int maNguoiTao, int maCauHoi, string tieuDe, string noiDung);
    }
}
