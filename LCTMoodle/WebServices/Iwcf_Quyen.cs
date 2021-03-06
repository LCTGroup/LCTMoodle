﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_Quyen" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_Quyen
    {
        [OperationContract]
        string[] layQuyenNguoiDungTheoDoiTuong(int maNguoiDung, string phamVi, int maDoiTuong);

        [OperationContract]
        bool coQuyen(int maNguoiDung, string phamVi, int maDoiTuong, string quyen);
    }
}
