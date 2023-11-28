using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class TestHoaDon : Form
    {
        private DataTable dtHoaDon;
        private DataTable dtCTHoaDon;
        private HoaDonBLL hdBLL;
        private CTHoaDonBLL cthdBLL;
        public TestHoaDon()
        {
            InitializeComponent();
            hdBLL = new HoaDonBLL();
            cthdBLL = new CTHoaDonBLL();
            dtHoaDon = hdBLL.getListHoaDon();
            dtCTHoaDon = cthdBLL.getListCTHD();
            //foreach (DataRow row in dtHoaDon.Rows)
            //{
            //    Console.WriteLine("MaHD: " + row["MaHD"]);
            //    Console.WriteLine("NgayLapHD: " + row["NgayLapHD"]);
            //    Console.WriteLine("TongTienTT: " + row["TongTienTT"]);
            //    Console.WriteLine("DiemSuDung: " + row["DiemSuDung"]);
            //    Console.WriteLine("TongTien: " + row["TongTien"]);
            //    Console.WriteLine("DiemNhanDuoc: " + row["DiemNhanDuoc"]);
            //    Console.WriteLine("MaKM: " + row["MaKM"]);
            //    Console.WriteLine("MaNV: " + row["MaNV"]);
            //    Console.WriteLine("MaKH: " + row["MaKH"]);
            //    Console.WriteLine("------------------------------");
            //}
            //updateHoaDon();
        }
        public void updateCTHD()
        {
            foreach (DataRow row in dtCTHoaDon.Rows)
            {
                CTHoaDonDTO cthd = new CTHoaDonDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    MaSP = row["MaSP"].ToString(),
                    TenSP = row["TenSP"].ToString(),
                    SoLuong = Convert.ToInt32(row["SoLuong"]),
                    DonGiaBanDau = Convert.ToInt32(row["DonGiaBanDau"]),
                    DonGiaDaGiam = Convert.ToInt32(row["DonGiaDaGiam"]),
                    PhanTramKM = Convert.ToInt32(row["PhanTramKM"]),
                    ThanhTien = Convert.ToInt32(row["ThanhTien"])
                    // Thêm các trường cần cập nhật khác nếu có
                };

                // In thông tin chi tiết hóa đơn trước khi cập nhật
                Console.WriteLine("Before Update CTHD:");
                Console.WriteLine("MaHD: " + cthd.MaHD);
                Console.WriteLine("MaSP: " + cthd.MaSP);
                Console.WriteLine("TenSP: " + cthd.TenSP);
                Console.WriteLine("SoLuong: " + cthd.SoLuong);
                Console.WriteLine("DonGiaBanDau: " + cthd.DonGiaBanDau);
                Console.WriteLine("DonGiaDaGiam: " + cthd.DonGiaDaGiam);
                Console.WriteLine("PhanTramKM: " + cthd.PhanTramKM);
                Console.WriteLine("ThanhTien: " + cthd.ThanhTien);

                // Thực hiện cập nhật chi tiết hóa đơn
                cthdBLL.updateCTHoaDon(cthd);

                // In thông tin chi tiết hóa đơn sau khi cập nhật
                Console.WriteLine("After Update CTHD:");
                Console.WriteLine("MaHD: " + cthd.MaHD);
                Console.WriteLine("MaSP: " + cthd.MaSP);
                Console.WriteLine("TenSP: " + cthd.TenSP);
                Console.WriteLine("SoLuong: " + cthd.SoLuong);
                Console.WriteLine("DonGiaBanDau: " + cthd.DonGiaBanDau);
                Console.WriteLine("DonGiaDaGiam: " + cthd.DonGiaDaGiam);
                Console.WriteLine("PhanTramKM: " + cthd.PhanTramKM);
                Console.WriteLine("ThanhTien: " + cthd.ThanhTien);

                Console.WriteLine("------------------------------");
            }
        }
        public void updateHoaDon()
        {
            foreach (DataRow row in dtHoaDon.Rows)
            {
                HoaDonDTO hd = new HoaDonDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    NgayLapHD = (DateTime)row["NgayLapHD"],
                    TongTienTT = Convert.ToInt32(row["TongTienTT"]),
                    DiemSuDung = Convert.ToInt32(row["DiemSuDung"]),
                    TongTien = Convert.ToSingle(row["TongTien"]),
                    DiemNhanDuoc = Convert.ToInt32(row["DiemNhanDuoc"]),
                    MaKM = (row["MaKM"] == DBNull.Value) ? "" : row["MaKM"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    MaKH = row["MaKH"].ToString()
                };

                // In thông tin hóa đơn trước khi cập nhật
                Console.WriteLine("Before Update:");
                Console.WriteLine("MaHD: " + hd.MaHD);
                Console.WriteLine("NgayLapHD: " + hd.NgayLapHD);
                Console.WriteLine("TongTienTT: " + hd.TongTienTT);
                Console.WriteLine("DiemSuDung: " + hd.DiemSuDung);
                Console.WriteLine("TongTien: " + hd.TongTien);
                Console.WriteLine("DiemNhanDuoc: " + hd.DiemNhanDuoc);
                Console.WriteLine("MaKM: " + hd.MaKM);
                Console.WriteLine("MaNV: " + hd.MaNV);
                Console.WriteLine("MaKH: " + hd.MaKH);

                // Thực hiện cập nhật
                hdBLL.updateHoaDon(hd);

                // In thông tin hóa đơn sau khi cập nhật
                Console.WriteLine("After Update:");
                Console.WriteLine("MaHD: " + hd.MaHD);
                Console.WriteLine("NgayLapHD: " + hd.NgayLapHD);
                Console.WriteLine("TongTienTT: " + hd.TongTienTT);
                Console.WriteLine("DiemSuDung: " + hd.DiemSuDung);
                Console.WriteLine("TongTien: " + hd.TongTien);
                Console.WriteLine("DiemNhanDuoc: " + hd.DiemNhanDuoc);
                Console.WriteLine("MaKM: " + hd.MaKM);
                Console.WriteLine("MaNV: " + hd.MaNV);
                Console.WriteLine("MaKH: " + hd.MaKH);

                Console.WriteLine("------------------------------");
            }
        }
    }
}
