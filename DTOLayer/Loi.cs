using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class DuLieuLoi 
    {
        public string truong;
        /// <summary>
        /// 1: Ràng buộc ---
        /// 2: Trùng
        /// </summary>
        public int loai;
        public string thongBao;
    }

    public class Loi : Dictionary<string, DuLieuLoi>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">key là trường trong dữ liệu lỗi</param>
        /// <param name="loai">
        /// 1: Ràng buộc ---
        /// 2: Trùng
        /// </param>
        /// <param name="thongBao"></param>
        public void Add(string key, int loai, string thongBao)
        {
            this.Add(key, new DuLieuLoi()
                {
                    truong = key,
                    loai = loai,
                    thongBao = thongBao
                });
        }
    }
}
