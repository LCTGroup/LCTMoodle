using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;

namespace BUSLayer
{
    public class ChuDeBUS : BUS
    {
        public static KetQua themChuDe(Dictionary<string, string> form)
        {            
            return ChuDeDAO.themChuDe(new ChuDeDataDTO() 
            {
                ten = form["Ten"],
                moTa = form["MoTa"],
                maHinhDaiDien = int.Parse(form["MaHinhDaiDien"]),
                maChuDeCha = int.Parse(form["MaChuDeCha"]),
                phamVi = form["PhamVi"]
            });
        }
    }
}
