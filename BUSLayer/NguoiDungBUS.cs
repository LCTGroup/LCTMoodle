using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using Helpers;
using System.Data.OleDb;
using System.Data;

namespace BUSLayer
{
    public class NguoiDungBUS : BUS
    {
        public static void gan(ref NguoiDungDTO nguoiDung, Form form)
        {
            if (nguoiDung == null)
            {
                nguoiDung = new NguoiDungDTO();
            }
            foreach(var key in form.Keys.ToArray())
            {
                switch(key)
                {
                    case "TenTaiKhoan":
                        nguoiDung.tenTaiKhoan = form.layString(key);
                        break;
                    case "MatKhau":
                        nguoiDung.matKhau = NguoiDungHelper.layMaMD5(form.layString(key));
                        break;
                    case "Email":
                        nguoiDung.email = form.layString(key);
                        break;
                    case "GioiTinh":
                        nguoiDung.gioiTinh = form.layInt(key);
                        break;
                    case "Ho":
                        nguoiDung.ho = form.layString(key);
                        break;
                    case "TenLot":
                        nguoiDung.tenLot = form.layString(key);
                        break;
                    case "Ten":
                        nguoiDung.ten = form.layString(key);
                        break;
                    case "NgaySinh":
                        nguoiDung.ngaySinh = form.layDate(key);
                        break;
                    case "DiaChi":
                        nguoiDung.diaChi = form.layString(key);
                        break;
                    case "SoDienThoai":
                        nguoiDung.soDienThoai = form.layString(key);
                        break;
                    case "MaHinhDaiDien":
                        nguoiDung.hinhDaiDien = TapTinBUS.chuyen("NguoiDung_HinhDaiDien", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    default: 
                        break;
                }
            }
        }

        public static KetQua kiemTra(NguoiDungDTO nguoiDung, string[] truong = null, bool kiemTra = true)
        {
            List<string> thongBao = new List<string>();

            #region Kiểm tra Valid

            if (coKiemTra("TenTaiKhoan", truong, kiemTra) && NguoiDungBUS.tonTaiTenTaiKhoan(nguoiDung.tenTaiKhoan))
            {
                thongBao.Add("Tên tài khoản bị trùng");
            }
            if (coKiemTra("TenTaiKhoan", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.tenTaiKhoan))
            {
                thongBao.Add("Tên tài khoản không được bỏ trống");
            }
            if (coKiemTra("MatKhau", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.matKhau))
            {
                thongBao.Add("Mật khẩu không được bỏ trống");
            }
            if (coKiemTra("NgaySinh", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.ngaySinh.ToString()))
            {
                thongBao.Add("Ngày sinh không được bỏ trống");

                var namSinh = Convert.ToDateTime(nguoiDung.ngaySinh).Year;
                var namHienTai = DateTime.Now.Year;
                if (namHienTai - namSinh < 13)
                {
                    thongBao.Add("Bạn chưa đủ tuổi để trở thành viên LCTMoodle");
                }
            }
            if (coKiemTra("Email", truong, kiemTra))
            {
                if (string.IsNullOrWhiteSpace(nguoiDung.email))
                {
                    thongBao.Add("Email không được bỏ trống");
                }
                else if (!LCTHelper.laEmail(nguoiDung.email))
                {
                    thongBao.Add("Email không hợp lệ");
                }
                else if (NguoiDungBUS.tonTaiEmail(nguoiDung.email))
                {
                    thongBao.Add("Email đã tồn tại. Vui lòng chọn email khác");
                }
            }                       
            if (coKiemTra("Ho", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.ho))
            {
                thongBao.Add("Họ không được bỏ trống");
            }
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.ho))
            {
                thongBao.Add("Tên không được bỏ trống");
            }
            #endregion

            KetQua ketQua = new KetQua();
            ketQua.trangThai = (thongBao.Count > 0) ? 3 : 0;
            ketQua.ketQua = thongBao;
            return ketQua;
        }

        public static BangCapNhat layBangCapNhat(NguoiDungDTO nguoiDung, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "MatKhau":
                        bangCapNhat.Add("MatKhau", nguoiDung.matKhau.ToString(), 2);
                        break;
                    case "GioiTinh":
                        bangCapNhat.Add("GioiTinh", nguoiDung.gioiTinh.ToString(), 1);
                        break;
                    case "Ho":
                        bangCapNhat.Add("Ho", nguoiDung.ho, 2);
                        break;
                    case "TenLot":
                        bangCapNhat.Add("TenLot", nguoiDung.tenLot, 2);
                        break;
                    case "Ten":
                        bangCapNhat.Add("Ten", nguoiDung.ten, 2);
                        break;
                    case "NgaySinh":
                        bangCapNhat.Add("NgaySinh", nguoiDung.ngaySinh.HasValue ? nguoiDung.ngaySinh.Value.ToString("d/M/yyyy") : null, 3);
                        break;
                    case "DiaChi":
                        bangCapNhat.Add("DiaChi", nguoiDung.diaChi, 2);
                        break;
                    case "SoDienThoai":
                        bangCapNhat.Add("SoDienThoai", nguoiDung.soDienThoai, 2);
                        break;
                    case "MaHinhDaiDien":
                        bangCapNhat.Add("MaHinhDaiDien", layMa_String(nguoiDung.hinhDaiDien), 2);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        #region Thêm

        public static KetQua them(Form form)
        {
            bool chapNhanQuyDinh = form.layBool("ChapNhanQuyDinh");

            if (!chapNhanQuyDinh)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Chấp nhận quy định của LCTMoodle bạn mới có thể trở thành thành viên của hệ thống"
                };
            }

            string maKichHoat = NguoiDungHelper.phatSinhMaKichHoat();
            NguoiDungDTO nguoiDung = new NguoiDungDTO()
            {
                maKichHoat = maKichHoat
            };
            gan(ref nguoiDung, form);

            KetQua ketQua = kiemTra(nguoiDung);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = NguoiDungDAO.them(nguoiDung);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            string duongDanKichHoat = @"http://moodle.lctgroup.org/NguoiDung/KichHoat?tenTaiKhoan=" + form.layString("TenTaiKhoan");
            string mailDangKy = form.layString("Email");
            string tieuDe = "Kích hoạt tài khoản LCTMoodle";
            string noiDung = "Mã kích hoạt tài khoản của bạn là: <b>" + maKichHoat +
                "</b><br><br>Đường dẫn kích hoạt: <a href=\"" + duongDanKichHoat + "\">Nhấp vào đây</a>";

            NguoiDungHelper.guiEmail(tieuDe, noiDung, mailDangKy);

            HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = nguoiDung.ma,
                    loaiDoiTuongBiTacDong = "HT",
                    maDoiTuongBiTacDong = nguoiDung.ma,
                    hanhDong = layDTO<HanhDongDTO>(100),
                    duongDan = "/NguoiDung/" + nguoiDung.ma
                });

            return ketQua;
        }

        #endregion

        #region Sửa

        public static KetQua capNhat(Form form)
        {
            int? ma = form.layInt("Ma");

            KetQua ketQua = NguoiDungBUS.layTheoMa(ma.Value);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;

            gan(ref nguoiDung, form);

            ketQua = NguoiDungBUS.kiemTra(nguoiDung, form.Keys.ToArray());
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = NguoiDungDAO.capNhat(ma, layBangCapNhat(nguoiDung, form.Keys.ToArray()));
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = nguoiDung.ma,
                    loaiDoiTuongBiTacDong = "ND",
                    maDoiTuongBiTacDong = nguoiDung.ma,
                    hanhDong = layDTO<HanhDongDTO>(200),
                    duongDan = "/NguoiDung/" + nguoiDung.ma
                });
            }
            return ketQua;
        }

        public static KetQua phucHoiMatKhau(Form form)
        {
            string email = form.layString("Email");

            KetQua ketQua = NguoiDungDAO.layTheoEmail(email);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            if (ketQua.trangThai == 1)
            {
                return new KetQua(1, "Email không tồn tại trong hệ thống");
            }

            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;

            DateTime ngayHienTai = DateTime.Now;
            DateTime ngayPhucHoi = Convert.ToDateTime(nguoiDung.thoiDiemPhucHoiMatKhau);
            TimeSpan NgayConLai = ngayHienTai.Subtract(ngayPhucHoi);

            if (Math.Floor(NgayConLai.TotalDays) < 10)
            {
                return new KetQua(3, "Còn " + (10 - Math.Floor(NgayConLai.TotalDays)) + " ngày nữa bạn mới có thể phục hồi mật khẩu.");
            }

            if (nguoiDung.email == email)
            {
                string matKhauMoi = NguoiDungHelper.phatSinhMatKhauMoi(6);

                ketQua = NguoiDungBUS.xuLyDoiMatKhau(nguoiDung.tenTaiKhoan, nguoiDung.matKhau, matKhauMoi, false);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }

                ketQua = NguoiDungDAO.capNhatTheoMaNguoiDung_ThoiDiemPhucHoiMatKhau(nguoiDung.ma);
                if (ketQua.trangThai != 0)
                {
                    return new KetQua("Không thể cập nhật thời điểm phục hồi mật khẩu");
                }

                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = nguoiDung.ma,
                    loaiDoiTuongBiTacDong = "ND",
                    maDoiTuongBiTacDong = nguoiDung.ma,
                    hanhDong = layDTO<HanhDongDTO>(201),
                    duongDan = "/NguoiDung/" + nguoiDung.ma
                });

                string tieuDe = "Phục hồi mật khẩu tài khoản LCTMoodle";
                string noiDung = "Thông tin tài khoản được phục hồi:<br> Tên tài khoản: " + nguoiDung.tenTaiKhoan + "<br>Mật khẩu mới: " + matKhauMoi;
                NguoiDungHelper.guiEmail(tieuDe, noiDung, email);

                return ketQua;
            }
            else
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Thông tin xác nhận chưa đúng"
                };
            }
        }

        public static KetQua chanNguoiDung(int maNguoiDung, bool trangThai, int? maNguoiChan)
        {
            #region Kiểm tra điều kiện

            if (!maNguoiChan.HasValue)
            {
                return new KetQua(4, "Bạn chưa đăng nhập");
            }

            if (!QuyenBUS.coQuyen("QLNguoiDung", "ND", 0, maNguoiChan))
            {
                return new KetQua(3, "Bạn không có quyền chặn người dùng");
            }

            #endregion

            var ketQua = NguoiDungDAO.capNhatTheoMa_Chan(maNguoiDung, trangThai);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiChan,
                    loaiDoiTuongBiTacDong = "ND",
                    maDoiTuongBiTacDong = maNguoiDung,
                    hanhDong = layDTO<HanhDongDTO>(202),
                    duongDan = "/NguoiDung/" + maNguoiDung
                });
            }

            return ketQua;
        }

        public static KetQua kichHoatTaiKhoan(Form form)
        {
            string tenTaiKhoan = form.layString("TenTaiKhoan");
            string matKhau = form.layString("MatKhau");
            string maKichHoat = form.layString("MaKichHoat");

            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;
            if (nguoiDung.maKichHoat == null)
            {
                ketQua.trangThai = 5;
                ketQua.ketQua = "Tài khoản đã được kích hoạt. Bạn đã có thể đăng nhập vào LCTMoodle";
                return ketQua;
            }
            if (nguoiDung.maKichHoat != maKichHoat)
            {
                ketQua.trangThai = 3;
                ketQua.ketQua = "Mã kích hoạt không đúng";
                return ketQua;
            }

            ketQua = NguoiDungDAO.capNhatTheoTenTaiKhoan_KichHoat(tenTaiKhoan, null);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = nguoiDung.ma,
                    loaiDoiTuongBiTacDong = "ND",
                    maDoiTuongBiTacDong = nguoiDung.ma,
                    hanhDong = layDTO<HanhDongDTO>(203),
                    duongDan = "/NguoiDung/" + nguoiDung.ma
                });
            }

            ketQua = NguoiDungBUS.xuLyDangNhap(tenTaiKhoan, matKhau, false);
            if (ketQua.trangThai != 0)
            {
                //Cập nhật lại mã kích hoạt cũ để trạng thái tài khoản trở thành như ban đầu
                NguoiDungDAO.capNhatTheoTenTaiKhoan_KichHoat(tenTaiKhoan, maKichHoat);

                ketQua.trangThai = 3;
                ketQua.ketQua = "Sai Mật khẩu";
                return ketQua;
            }

            return ketQua;
        }

        #endregion

        #region Lấy

        public static KetQua layTheoTenTaiKhoan(string tenTaiKhoan)
        {
            return NguoiDungDAO.layTheoTenTaiKhoan(tenTaiKhoan);
        }

        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return NguoiDungDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua layTheoMaNhomNguoiDung(string phamVi, int maNhomNguoiDung)
        {
            return NguoiDungDAO.layTheoMaNhomNguoiDung(phamVi, maNhomNguoiDung);
        }

        public static KetQua timKiem(string tuKhoa, LienKet lienKet = null)
        {
            return NguoiDungDAO.lay_TimKiem(tuKhoa, lienKet);
        }

        public static KetQua docTapTin_xls(string duongDan)
        {
            try
            {
                string con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + duongDan + "';Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
                List<NguoiDungDTO> dsNguoiDung = new List<NguoiDungDTO>();
                using (OleDbConnection ketNoi = new OleDbConnection(con))
                {
                    ketNoi.Open();
                    string tenBang = ketNoi.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();

                    OleDbCommand command = new OleDbCommand("SELECT * FROM [" + tenBang + "]", ketNoi);
                    using (OleDbDataReader dong = command.ExecuteReader())
                    {
                        KetQua ketQua;
                        while (dong.Read())
                        {
                            NguoiDungDTO nguoiDung = new NguoiDungDTO();

                            for (int i = 0; i < dong.FieldCount; i++)
                            {
                                switch (dong.GetName(i))
                                {
                                    case "TenTaiKhoan":
                                        bool a = dong.IsDBNull(i);
                                        object b = dong[i];
                                        nguoiDung.tenTaiKhoan = dong.IsDBNull(i) ? null : dong[i].ToString(); break;
                                    case "MatKhau":
                                        nguoiDung.matKhau = dong.IsDBNull(i) ? null : dong[i].ToString(); break;
                                    case "Email":
                                        nguoiDung.email = dong.IsDBNull(i) ? null : dong[i].ToString(); break;
                                    case "Ho":
                                        nguoiDung.ho = dong.IsDBNull(i) ? null : dong[i].ToString(); break;
                                    case "TenLot":
                                        nguoiDung.tenLot = dong.IsDBNull(i) ? null : dong[i].ToString(); break;
                                    case "Ten":
                                        nguoiDung.ten = dong.IsDBNull(i) ? null : dong[i].ToString(); break;
                                    default:
                                        break;
                                }
                            }

                            nguoiDung.ngaySinh = new DateTime(1997, 1, 1);

                            ketQua = kiemTra2(nguoiDung);
                            if (ketQua.trangThai != 0)
                            {
                                nguoiDung.duLieuThem = new Dictionary<string, object>()
                                { 
                                    { "Loi", ketQua.ketQua }
                                };
                            }

                            dsNguoiDung.Add(nguoiDung);
                        }
                    }
                    ketNoi.Close();
                }
                return new KetQua(dsNguoiDung);
            }
            catch (Exception ex)
            {
                return new KetQua(2, ex.Message);
            }
        }

        #endregion

        #region Kiểm tra

        public static bool tonTaiTenTaiKhoan(string tenTaiKhoan)
        {
            KetQua ketQua = NguoiDungDAO.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai == 0)
            {
                return true;
            }
            return false;
        }

        public static bool tonTaiEmail(string email)
        {
            KetQua ketQua = NguoiDungDAO.layTheoEmail(email);
            if (ketQua.trangThai == 0)
            {
                return true;
            }
            return false;
        }

        public static KetQua kiemTraDangNhap()
        {
            if (Session["NguoiDung"] == null)
            {
                return new KetQua()
                {
                    trangThai = 4,
                    ketQua = "Chưa đăng nhập"
                };
            }
            return new KetQua()
            {
                trangThai = 0,
                ketQua = "Đã đăng nhập"
            };
        }

        public static KetQua kiemTraCookie()
        {
            HttpCookie ckNguoiDung = HttpContext.Current.Request.Cookies.Get("NguoiDung");

            if (ckNguoiDung == null)
            {
                return new KetQua()
                {
                    trangThai = 4,
                    ketQua = "Không tồn tại Cookie"
                };
            }

            Form formNguoiDung = new Form() 
            {
                { "TenTaiKhoan", ckNguoiDung["TenTaiKhoan"] },
                { "MatKhau", ckNguoiDung["MatKhau"] },
                { "GhiNho", "1" }
            };

            return NguoiDungBUS.xuLyDangNhap(formNguoiDung);
        }

        public static KetQua kiemTra2(NguoiDungDTO nguoiDung, string[] truong = null, bool kiemTra = true)
        {
            Loi loi = new Loi();

            #region Kiểm tra Valid

            if (coKiemTra("Ho", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.ho))
            {
                loi.Add("Ho", 1, "Họ không được bỏ trống");
            }
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.ho))
            {
                loi.Add("Ten", 1, "Tên không được bỏ trống");
            }
            if (coKiemTra("TenTaiKhoan", truong, kiemTra))
            {
                if (string.IsNullOrWhiteSpace(nguoiDung.tenTaiKhoan))
                {
                    loi.Add("TenTaiKhoan", 1, "Tên tài khoản không được bỏ trống");
                }
                else if (NguoiDungBUS.tonTaiTenTaiKhoan(nguoiDung.tenTaiKhoan))
                {
                    loi.Add("TenTaiKhoan", 2, "Tên tài khoản bị trùng");
                }
            }
            if (coKiemTra("MatKhau", truong, kiemTra) && string.IsNullOrWhiteSpace(nguoiDung.matKhau))
            {
                loi.Add("MatKhau", 1, "Mật khẩu không được bỏ trống");
            }
            if (coKiemTra("NgaySinh", truong, kiemTra))
            {
                if (!nguoiDung.ngaySinh.HasValue)
                {
                    loi.Add("NgaySinh", 1, "Ngày sinh không được bỏ trống");
                }
                else
                {
                    var namSinh = Convert.ToDateTime(nguoiDung.ngaySinh).Year;
                    var namHienTai = DateTime.Now.Year;
                    if (namHienTai - namSinh < 13)
                    {
                        loi.Add("NgaySinh", 1, "Bạn chưa đủ tuổi để trở thành viên LCTMoodle");
                    }
                }
            }
            if (coKiemTra("Email", truong, kiemTra))
            {
                if (string.IsNullOrWhiteSpace(nguoiDung.email))
                {
                    loi.Add("Email", 1, "Email không được bỏ trống");
                }
                else if (!LCTHelper.laEmail(nguoiDung.email))
                {
                    loi.Add("Email", 1, "Email không hợp lệ");
                }
                else if (NguoiDungBUS.tonTaiEmail(nguoiDung.email))
                {
                    loi.Add("Email", 2, "Email đã tồn tại");
                }
            }
            #endregion

            KetQua ketQua = new KetQua();
            ketQua.trangThai = (loi.Count > 0) ? 3 : 0;
            ketQua.ketQua = loi;
            return ketQua;
        }

        #endregion

        #region Xử lý

        public static KetQua xuLyDangNhap(string tenTaiKhoan, string matKhau, bool ghiNho = false)
        {
            if (!tonTaiTenTaiKhoan(tenTaiKhoan))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Tài khoản không tồn tại"
                };
            }

            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;
            if (nguoiDung.daDuyet == false)
            {
                return new KetQua(3, "Tài khoản đã bị chặn");
            }

            if (nguoiDung.maKichHoat != null)
            {
                return new KetQua()
                {
                    trangThai = 5,
                    ketQua = "Tài khoản chưa được kích hoạt. Bạn vui lòng kiểm tra email để kích hoạt tài khoản."
                };
            }

            if ((NguoiDungHelper.layMaMD5(matKhau) == nguoiDung.matKhau) || (matKhau == nguoiDung.matKhau))
            {
                //Lưu mã người dùng vào Session
                HttpContext.Current.Session["NguoiDung"] = nguoiDung.ma;

                if (ghiNho)
                {
                    //Lưu Cookie
                    HttpCookie cookieNguoiDung = new HttpCookie("NguoiDung");
                    cookieNguoiDung.Values.Add("TenTaiKhoan", nguoiDung.tenTaiKhoan);
                    cookieNguoiDung.Values.Add("MatKhau", nguoiDung.matKhau);
                    cookieNguoiDung.Expires = DateTime.Now.AddDays(7);

                    HttpContext.Current.Response.Cookies.Add(cookieNguoiDung);
                }

                ketQua.ketQua = nguoiDung;
                return ketQua;
            }

            ketQua.trangThai = 3;
            ketQua.ketQua = "Mật khẩu không đúng";

            return ketQua;
        }

        //Hàm cùng tên
        public static KetQua xuLyDangNhap(Form form)
        {
            string tenTaiKhoan = form.layString("TenTaiKhoan");
            string matKhau = form.layString("MatKhau");
            bool ghiNho = form.layBool("GhiNho");

            return NguoiDungBUS.xuLyDangNhap(tenTaiKhoan, matKhau, ghiNho);
        }

        public static void xuLyDangXuat()
        {
            //Xóa Cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies["NguoiDung"];

            if (cookie != null)
            {
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            //Xóa session            
            HttpContext.Current.Session["NguoiDung"] = null;
        }

        public static KetQua xuLyDoiMatKhau(string tenTaiKhoan, string matKhauCu, string MatKhauMoi, bool maHoaMatKhauCu = true)
        {
            KetQua ketQua = NguoiDungBUS.layTheoTenTaiKhoan(tenTaiKhoan);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            NguoiDungDTO nguoiDung = ketQua.ketQua as NguoiDungDTO;
            if (maHoaMatKhauCu)
            {
                if (nguoiDung.matKhau != NguoiDungHelper.layMaMD5(matKhauCu))
                {
                    return new KetQua()
                    {
                        trangThai = 3,
                        ketQua = "Nhập sai mật khẩu"
                    };
                }
            }
            else
            {
                if (nguoiDung.matKhau != matKhauCu)
                {
                    return new KetQua()
                    {
                        trangThai = 3,
                        ketQua = "Nhập sai mật khẩu"
                    };
                }
            }

            Form form = new Form()
            {
                { "MatKhau", MatKhauMoi }
            };
            gan(ref nguoiDung, form);

            ketQua = NguoiDungBUS.kiemTra(nguoiDung, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = NguoiDungDAO.capNhat(nguoiDung.ma, layBangCapNhat(nguoiDung, form.Keys.ToArray()));
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maDoiTuongTacDong = nguoiDung.ma,
                    loaiDoiTuongBiTacDong = "ND",
                    maDoiTuongBiTacDong = nguoiDung.ma,
                    hanhDong = layDTO<HanhDongDTO>(205),
                    duongDan = "/NguoiDung/" + nguoiDung.ma
                });
            }

            return ketQua;
        }

        public static KetQua layTheoEmail(string email, LienKet lienKet = null)
        {
            return NguoiDungDAO.layTheoEmail(email, lienKet);
        }
            
        private static DataTable toTable(List<NguoiDungDTO> dsNguoiDung)
        {
            DataTable bang = new DataTable();
            bang.Columns.AddRange(new DataColumn[] {
                new DataColumn("TenTaiKhoan"),
                new DataColumn("MatKhau"),
                new DataColumn("Email"),
                new DataColumn("Ho"),
                new DataColumn("TenLot"),
                new DataColumn("Ten"),
                new DataColumn("MaKichHoat")
            });

            foreach (var nguoiDung in dsNguoiDung)
            {
                bang.Rows.Add(new object[]
                {
                    nguoiDung.tenTaiKhoan,
                    nguoiDung.matKhau,
                    nguoiDung.email,
                    nguoiDung.ho,
                    nguoiDung.tenLot,
                    nguoiDung.ten,
                    nguoiDung.maKichHoat
                });
            }

            return bang;
        }

        public static KetQua them(List<NguoiDungDTO> dsNguoiDung)
        {
            int slNguoiDung = dsNguoiDung.Count;
            KetQua ketQua;
            for (var i = 0; i < slNguoiDung; i++)
            {
                dsNguoiDung[i].maKichHoat = NguoiDungHelper.phatSinhMaKichHoat();
                dsNguoiDung[i].matKhau = NguoiDungHelper.layMaMD5(dsNguoiDung[i].matKhau);
                dsNguoiDung[i].ngaySinh = new DateTime(1997, 1, 1);
                ketQua = kiemTra(dsNguoiDung[i]);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }
            }

            ketQua = NguoiDungDAO.them(toTable(dsNguoiDung));
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            foreach (var nguoiDung in dsNguoiDung)
            {
                string duongDanKichHoat = @"http://moodle.lctgroup.org/NguoiDung/KichHoat?tenTaiKhoan=" + nguoiDung.tenTaiKhoan;
                string mailDangKy = nguoiDung.email;
                string tieuDe = "Kích hoạt tài khoản LCTMoodle";
                string noiDung = "Mã kích hoạt tài khoản của bạn là: <b>" + nguoiDung.maKichHoat +
                    "</b><br><br>Đường dẫn kích hoạt: <a href=\"" + duongDanKichHoat + "\">Nhấp vào đây</a>";

                NguoiDungHelper.guiEmail(tieuDe, noiDung, mailDangKy);
            }

            return ketQua;
        }

        public static KetQua xuLyDoiMatKhau(Form form)
        {
            string tenTaiKhoan = form.layString("TenTaiKhoan");
            string matKhauCu = form.layString("MatKhauCu");
            string matKhauMoi = form.layString("MatKhauMoi");

            return xuLyDoiMatKhau(tenTaiKhoan, matKhauCu, matKhauMoi);
        }

        #endregion
    }
}