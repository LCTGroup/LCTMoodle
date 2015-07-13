using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_TraLoi" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_CauHoi_TraLoi
    {
        [OperationContract]
        byte[] layHinhAnh(string ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int chiSo, string ten);

        [OperationContract]
        List<clientmodel_CauHoi_TraLoi> layTheoMaCauHoi(int ma);
    }
}
