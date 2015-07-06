using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BangCapNhat
    {
        public DataTable bang;

        public BangCapNhat()
        {
            bang = new DataTable();
            bang.Columns.AddRange(new DataColumn[] {
                new DataColumn("TenTruong"),
                new DataColumn("GiaTri"),
                new DataColumn("Loai"),
            });
        }

        /// <summary>
        /// 1: Không có dấu nháy (số) ---
        /// 2: Có dấu nháy (chuỗi) ---
        /// 3: Datetime
        /// </summary>
        /// <param name="loai">1, 2, 3</param>
        public void Add(string tenTruong, string giaTri, int loai)
        {
            bang.Rows.Add(new object[]
            {
                tenTruong,
                string.IsNullOrWhiteSpace(giaTri) ? null : giaTri,
                loai
            });
        }

        public bool coDuLieu()
        {
            return bang.Rows.Count > 0;
        }
    }
}
