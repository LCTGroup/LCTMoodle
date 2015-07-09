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
        byte[] layHinhAnh(string _Ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int _ChiSo, string _Ten);

        [OperationContract]
        clientmodel_CauHoi layTheoMa(int _Ma);

        [OperationContract]
        List<clientmodel_TraLoi> layTraLoiTheoMaCauHoi(int _Ma);

        [OperationContract]
        List<clientmodel_CauHoi> lay(int _SoPT);

        [OperationContract]
        List<clientmodel_CauHoi> timKiem(string _TuKhoa);

        [OperationContract]
        List<CauHoiDTO> timKiemTheoChuDe(int _MaChuDe, string _TuKhoa);
    }
}
