using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CheDoRiengTu
    {
        public string ma;
        public string ten;
        public bool macDinh;

        public static CheDoRiengTu lay(string ma)
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

        public static List<CheDoRiengTu> layDanhSach()
        {
            return new List<CheDoRiengTu>()
            {
                new CheDoRiengTu() 
                {
                    ma = "NoiBo",
                    ten = "Nội bộ",
                    macDinh = true
                },
                new CheDoRiengTu()
                {
                    ma = "CongKhai",
                    ten = "Công khai"
                }
            };
        }
    }
}
