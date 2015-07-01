using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class KetQua
    {
        /// <summary>
        /// 0: Thành công ---
        /// 1: Không có dòng dữ liệu nào ---
        /// 2: Lỗi truy vấn / Lỗi xử lý ---
        /// 3: Lỗi ràng buộc ---
        /// 4: Chưa đăng nhập ---
        /// 5: Chưa kích hoạt ---
        /// 6: Trạng thái đặc biệt của nghiệp vụ, kết quả có thể là loại
        /// </summary>
        public int trangThai;
        public object ketQua;

        public KetQua()
        {

        }

        public KetQua(int trangThai)
        {
            this.trangThai = trangThai;
        }

        public KetQua(object ketQua)
        {
            this.trangThai = 0;
            this.ketQua = ketQua;
        }

        public KetQua(int trangThai, object ketQua)
        {
            this.trangThai = trangThai;
            this.ketQua = ketQua;
        }
    }
}
