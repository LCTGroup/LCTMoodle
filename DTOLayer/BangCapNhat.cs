using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLayer
{
    public class BangCapNhat : DataTable
    {
        public BangCapNhat()
        {
            Columns.AddRange(new DataColumn[] {
                new DataColumn("TenTruong", typeof(string)),
                new DataColumn("GiaTri", typeof(string)),
                new DataColumn("LaChuoi", typeof(bool)),
            });
        }
        public BangCapNhat(Dictionary<string, string> form, Dictionary<string, bool> danhSachTruong)
        {
            Columns.AddRange(new DataColumn[] {
                new DataColumn("TenTruong", typeof(string)),
                new DataColumn("GiaTri", typeof(string)),
                new DataColumn("LaChuoi", typeof(bool)),
            });

            foreach (KeyValuePair<string, bool> truong in danhSachTruong)
            {
                Add(truong.Key, form[truong.Key], truong.Value);
            }
        }

        public void Add(string tenTruong, string giaTri, bool laChuoi)
        {
            Rows.Add(new object[]
            {
                tenTruong,
                giaTri,
                laChuoi
            });
        }
    }
}
