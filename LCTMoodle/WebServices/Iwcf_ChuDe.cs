using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTOLayer;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_ChuDe" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_ChuDe
    {
        [OperationContract]
        Dictionary<DTOLayer.ChuDeDTO, byte[]> layChuDeTheoMaCha(int _MaCha);

        [OperationContract]
        byte[] layHinhDaiDien(string _Loai, string _Ten);
    }
}
