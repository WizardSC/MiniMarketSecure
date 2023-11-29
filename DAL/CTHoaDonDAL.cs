using DTO;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CTHoaDonDAL : MSSQLConnect
    {
        public DataTable getListCTHD()
        {
            DataTable dt = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from ChiTietHoaDon";
                cmd.Connection = conn;
                
                SqlDataAdapter adt = new SqlDataAdapter(cmd);
                adt.Fill(dt);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }
        public DataTable getListCTHDByMaHD(string maHD)
        {
            DataTable dt = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select MaSP, TenSP, SoLuong, DonGiaBanDau, DonGiaDaGiam, ThanhTien from ChiTietHoaDon where MaHD = @MaHD";
                cmd.Connection = conn;
                cmd.Parameters.Add("@MaHD", SqlDbType.Char).Value = maHD;
                SqlDataAdapter adt = new SqlDataAdapter(cmd);
                adt.Fill(dt);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }
        public bool insertCTHoaDon(CTHoaDonDTO cthd)
        {
            try
            {
                Connect();
                string sql = "insert into ChiTietHoaDon values (@MaHD, @MaSP, @TenSP, @SoLuong, @DonGiaBanDau, @DonGiaDaGiam, @PhanTramKM, @ThanhTien)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@MaHD", SqlDbType.NVarChar).Value = cthd.MaHD;
                cmd.Parameters.Add("@MaSP", SqlDbType.NVarChar).Value = cthd.MaSP;
                cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar).Value = cthd.TenSP;
                cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = cthd.SoLuong;
                cmd.Parameters.Add("@DonGiaBanDau", SqlDbType.Int).Value = cthd.DonGiaBanDau;
                cmd.Parameters.Add("@DonGiaDaGiam", SqlDbType.Int).Value = cthd.DonGiaDaGiam;
                cmd.Parameters.Add("@PhanTramKM", SqlDbType.Int).Value = cthd.PhanTramKM;
                cmd.Parameters.Add("@ThanhTien", SqlDbType.Float).Value = cthd.ThanhTien;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi:" + ex.Message);
                return false;
            }
            finally
            {
                Disconnect();
            }
        }

        public bool updateChiTietHoaDon(CTHoaDonDTO cthd)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE ChiTietHoaDon SET TenSP = @TenSP, SoLuong = @SoLuong, " +
                                  "DonGiaBanDau = @DonGiaBanDau, DonGiaDaGiam = @DonGiaDaGiam, PhanTramKM = @PhanTramKM, " +
                                  "ThanhTien = @ThanhTien WHERE MaHD = @MaHD AND MaSP = @MaSP ";

                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@MaHD", cthd.MaHD).SqlDbType = SqlDbType.Char;
                cmd.Parameters.AddWithValue("@MaSP", cthd.MaSP).SqlDbType = SqlDbType.Char;
                cmd.Parameters.AddWithValue("@TenSP", cthd.TenSP).SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@SoLuong", cthd.SoLuong).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@DonGiaBanDau", cthd.DonGiaBanDau).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@DonGiaDaGiam", cthd.DonGiaDaGiam).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@PhanTramKM", cthd.PhanTramKM).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@ThanhTien", cthd.ThanhTien).SqlDbType = SqlDbType.Int;

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi:" + ex.Message);
                return false;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
