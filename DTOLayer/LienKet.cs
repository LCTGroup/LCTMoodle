using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class LienKet : Dictionary<string, LienKet>
    {
        public static bool co(LienKet lienKet, string tenLienKet)
        {
            return lienKet != null && lienKet.ContainsKey(tenLienKet);
        }

        public void Add(string key)
        {
            Add(key, null);
        }
    }
}
