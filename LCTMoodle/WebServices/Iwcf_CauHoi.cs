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
        CauHoiDTO layTheoMa(int _Ma);

        [OperationContract]
        List<CauHoiDTO> layTheoChuDe(int _MaChuDe);

        [OperationContract]
        List<CauHoiDTO> lay();

        [OperationContract]
        List<CauHoiDTO> timKiem(string _TuKhoa);
    }
}
