using LCTMoodle.WebServices.Client_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_KhoaHoc_DienDan_BinhLuan" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_KhoaHoc_DienDan_BinhLuan
    {
        [OperationContract]
        clientmodel_KhoaHoc_DienDan_BinhLuan layTheoMa(int maBinhLuan);

        [OperationContract]
        List<clientmodel_KhoaHoc_DienDan_BinhLuan> layDanhSachTheoMaDienDan(int maDienDan);
    }
}
