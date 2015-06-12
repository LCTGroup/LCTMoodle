using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;

namespace Helpers
{
    public static class NguoiDungHelper
    {
        /// <summary>
        /// Lấy mã MD5
        /// </summary>
        /// <param name="chuoiCanMaHoa"></param>
        /// <returns>Trả về chuỗi đã được mã hóa MD5</returns>
        public static string layMaMD5(string chuoiCanMaHoa)
        {
            //Tạo instance MD5
            MD5 md5 = MD5.Create();

            //Chuyển chuỗi truyền vào thành mảng byte
            byte[] hashData = md5.ComputeHash(Encoding.UTF8.GetBytes(chuoiCanMaHoa));

            StringBuilder giaTriTraVe = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                giaTriTraVe.Append(hashData[i].ToString("x2"));
            }

            return giaTriTraVe.ToString();
        }
        public static bool chuaDangNhap()
        {
            if (HttpContext.Current.Session["NguoiDung"] == null)
            {
                return true;
            }
            return false;
        }
    }
}
