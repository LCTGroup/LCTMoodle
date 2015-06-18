using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTOLayer;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_KhoaHoc" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_KhoaHoc
    {
        [OperationContract]
        List<KhoaHocDTO> layKhoaHoc();

        [OperationContract]
        List<KhoaHocDTO> layLK();

        [OperationContract]
        KhoaHocDTO layKhoaHocTheoMa(int _Ma);
    }
}
