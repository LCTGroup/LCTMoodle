using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PhamVi
    {
        public string ma;
        public string ten;
        public string moTa;
        public string hinh;

        public static PhamVi lay(string ma)
        {
            var danhSach = layDanhSach();
            int soLuong = danhSach.Count;

            for (int i = 0; i < soLuong; i++)
            {
                if (danhSach[i].ma.Equals(ma))
                {
                    return danhSach[i];
                }
            }

            return null;
        }

        public static List<PhamVi> layDanhSach()
        {
            return new List<PhamVi>()
            {
                new PhamVi() 
                {
                    ma = "HoiDap",
                    ten = "Hỏi đáp",
                    moTa = "Những chủ đề trong phạm vi hỏi đáp",
                    hinh = "he-thong.png"
                },
                new PhamVi() 
                {
                    ma = "KhoaHoc",
                    ten = "Khóa học",
                    moTa = "Những chủ đề trong phạm vi khóa học",
                    hinh = "khoa-hoc.png"
                }
            };
        }
    }
}
