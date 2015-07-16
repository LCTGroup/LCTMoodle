using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;
using System.Net;
using System.Net.Mail;
using DTOLayer;

namespace Helpers
{
    public static class CauHoiHelper
    {
        public static bool coCauTraLoiDung(CauHoiDTO cauHoi)
        {
            if (cauHoi.danhSachTraLoi == null)
            {
                return false;
            }

            foreach(var traLoi in cauHoi.danhSachTraLoi)
            {
                if (traLoi.duyet == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
