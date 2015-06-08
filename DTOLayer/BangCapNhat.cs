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
                new DataColumn("LaChuoi"),
            });
        }

        public void Add(string tenTruong, string giaTri, bool laChuoi)
        {
            bang.Rows.Add(new object[]
            {
                tenTruong,
                string.IsNullOrEmpty(giaTri) ? null : giaTri,
                laChuoi
            });
        }
    }
}
