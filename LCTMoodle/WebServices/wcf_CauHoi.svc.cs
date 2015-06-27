using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_CauHoi" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_CauHoi.svc or wcf_CauHoi.svc.cs at the Solution Explorer and start debugging.
    public class wcf_CauHoi : Iwcf_CauHoi
    {
        private const string _Loai = "CauHoi_HinhDaiDien";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Ten"></param>
        /// <returns></returns>
        public byte[] layHinhAnh(string _Ten)
        {
            throw new NotImplementedException();
        }

        public Client_Model.clientmodel_HinhAnh layHinhAnhChiSo(int _ChiSo, string _Ten)
        {
            throw new NotImplementedException();
        }

        public DTOLayer.CauHoiDTO layTheoMa(int _Ma)
        {
            throw new NotImplementedException();
        }

        public List<DTOLayer.CauHoiDTO> layTheoChuDe(int _MaChuDe)
        {
            throw new NotImplementedException();
        }

        public List<DTOLayer.CauHoiDTO> lay()
        {
            throw new NotImplementedException();
        }

        public List<DTOLayer.CauHoiDTO> timKiem(string _TuKhoa)
        {
            throw new NotImplementedException();
        }
    }
}
