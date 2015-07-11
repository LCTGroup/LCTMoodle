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
        List<clientmodel_CauHoi> lay(int soPT);

        [OperationContract]
        List<clientmodel_CauHoi> timKiem(string tuKhoa);

        [OperationContract]
        List<CauHoiDTO> timKiemTheoChuDe(int maChuDe, string tuKhoa);
    }
}
