using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class HoiDapDiemDTO : DTO
    {
        public NguoiDungDTO maNguoiTao;
        public bool diem;
 
        //public override void gan(System.Data.SqlClient.SqlDataReader dong)
        //{
        //    for (int i = 0; i < dong.FieldCount; i++)
        //    {
        //        switch (dong.GetName(i))
        //        {
        //            case "Ma":
        //                ma = layInt(dong, i); break;
        //            case "MaNguoiTao":
        //                maNguoiTao = new NguoiDungViewDTO()
        //                {
        //                    ma = layInt(dong, i)
        //                };
        //                break;
        //            case "Diem":
        //                diem = layBool(dong, i); break;                        
        //            default:
        //                break;
        //        }
        //    }
        //}        
    }
}
