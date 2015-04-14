using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTMoodle.Helpers
{
    public static class LCTHelper
    {
        public static Dictionary<string, string> layDanhSachPhamVi()
        {
            var danhSachPhamVi = new Dictionary<string, string>()
            {
                { "Hệ thống", "HeThong" },
                { "Khóa học", "KhoaHoc" }
            };

            return danhSachPhamVi;
        }

        public static Dictionary<string, Dictionary<string, string>> layDanhSachPhamVi_DayDu()
        {
            var danhSachPhamVi = new Dictionary<string, Dictionary<string, string>>()
            {
                {
                    "Hệ thống", new Dictionary<string, string>()
                    {
                        { "Ma", "HeThong" },
                        { "MoTa", "Những chủ đề trong phạm vi toàn hệ thống" },
                        { "Hinh", "he-thong.png" }
                    }
                },
                {
                    "Khóa học", new Dictionary<string, string>()
                    {
                        { "Ma", "KhoaHoc" },
                        { "MoTa", "Những chủ đề trong phạm vi khóa học" },
                        { "Hinh", "khoa-hoc.png" }
                    }
                }
            };

            return danhSachPhamVi;
        }
    }
}