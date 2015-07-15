using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Data;
using System.Data.OleDb;
using System.Data;

namespace LCTMoodle.Controllers
{
    public class TrangChuController : Controller
    {
        public ActionResult Index()
        {
            string a = "";
            string con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + Server.MapPath("~/") + "\"; Extended Properties=\"text;HDR=Yes;FMT=Delimited\";";
            //string con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~/") + ";Extended Properties=\"Text;HDR=YES;FMT=Delimited\"";
            
            using(OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [a.csv]", connection); 
                //using(OleDbDataReader dr = command.ExecuteReader())
                //{
                //     while(dr.Read())
                //     {
                //         a += dr[0] + "|";
                //         a += dr[1] + "|";
                //         a += dr[2] + "|";
                //     }
                //}
                using (OleDbDataAdapter adp = new OleDbDataAdapter(command))
                {
                    DataTable tbl = new DataTable("MyTable");
                    adp.Fill(tbl);

                    foreach (DataRow row in tbl.Rows)
                    {
                        a += row[0] + "|";
                        a += row[1] + "|";
                        a += row[2] + "|";
                    }
                }
            }

            return null;
            var ketQua = KhoaHocBUS.timKiemPhanTrang(1, 8, null, null, new LienKet() { "GiangVien" });
            if (ketQua.trangThai == 0)
            {
                ViewData["KhoaHoc"] = ketQua.ketQua as List<KhoaHocDTO>;
            }

            ketQua = CauHoiBUS.layDanhSach(10, new LienKet() { "NguoiTao", "HinhDaiDien" });
            if (ketQua.trangThai == 0)
            {
                ViewData["CauHoi"] = ketQua.ketQua as List<CauHoiDTO>;
            }

            ketQua = ChuDeBUS.timKiemPhanTrang(1, 20);

            return View();
        }

        public ActionResult FormMau()
        {
            return View();
        }

        public ActionResult CayMau()
        {
            return View();
        }

        public ActionResult TrangMau()
        {
            return View();
        }
	}
}