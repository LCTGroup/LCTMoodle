using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace DTOLayer
{
    public class CauHoiDataDTO : DTO
    {
        public string tieuDe;
        public string noiDung;        
        public DateTime? thoiDiemTao;
        public int maNguoiTao;

        /// <summary>
        /// Kiểm tra
        /// </summary>
        /// <returns>ketQua.TrangThai != 3 : dữ liệu hợp lệ</returns>
        //public override KetQua kiemTra()
        //{            
            
        //}
    }
}
