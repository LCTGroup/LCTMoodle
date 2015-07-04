using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace DTOLayer
{
    public class DTO
    {
        public int? ma;
        /// <summary>
        /// Khi select dữ liệu, trường nào không có sẽ đưa vào field này
        /// </summary>
        public Dictionary<string, object> duLieuThem;
    }
}
