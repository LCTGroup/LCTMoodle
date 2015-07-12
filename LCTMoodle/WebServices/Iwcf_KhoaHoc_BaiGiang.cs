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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_KhoaHoc_BaiGiang" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_KhoaHoc_BaiGiang
    {
        [OperationContract]
        clientmodel_KhoaHoc_BaiGiang layTheoMa(int maBaiGiang);
        [OperationContract]
        List<clientmodel_KhoaHoc_BaiGiang> layDanhSachTheoMaKhoaHoc(int maKhoaHoc);
    }
}
