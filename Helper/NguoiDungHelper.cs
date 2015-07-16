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

        public static void guiEmail(string tieuDe, string noiDung, string diaChiNguoiNhan)
        {
                MailAddress emailNguoiGui = new MailAddress("ModeratorLCTMoodle@gmail.com");
                MailAddress emailNguoiNhan = new MailAddress(diaChiNguoiNhan);

                MailMessage mail = new MailMessage(emailNguoiNhan, emailNguoiNhan);
                mail.Subject = tieuDe;
                mail.Body = noiDung;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("moderatorlctmoodle@gmail.com", "LCTMoodle@Moderator123");
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Send(mail);            
        }                

        public static string phatSinhMaKichHoat()
        {
            Random rd = new Random();
            string maKichHoat = rd.Next(100000, 999999).ToString();
            return maKichHoat;
        }

        public static string chuyenGioiTinh(int? soGioiTinh)
        {
            switch(soGioiTinh.Value)
            {
                case 1:
                    return "Nam";
                case 2:
                    return "Nữ";
                default:
                    return "Không xác định";
            }
        }

        public static string phatSinhMatKhauMoi(int doDai)
        {
            StringBuilder str = new StringBuilder();

            Random random = new Random();

            Char ch;
            
            for (int i = 0; i < doDai; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 97)));
                str.Append(ch);
            }

            return str.ToString();
        }

        public static bool laNguoiGuiTinNhan(int maNguoiGui, int maNguoiDungHienTai)
        {
            return maNguoiGui == maNguoiDungHienTai;
        }
    }
}
