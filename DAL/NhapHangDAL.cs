﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NhapHangDAL : MSSQLConnect
    {
        public DataTable getListSPToNhapHang(int trangThai)
        {
            DataTable dt = new DataTable();
            try
            {
                Connect();
                string sql = "select MaSP, TenSP, SoLuong, DonGiaNhap, TenLoai, TenNCC from SanPham join LoaiSP on LoaiSP.MaLoai = SanPham.MaLoai join NhaCungCap on NhaCungCap.MaNCC = SanPham.MaNCC where SanPham.trangthai = @TrangThai";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@TrangThai", SqlDbType.Char).Value = trangThai;
                SqlDataAdapter adt = new SqlDataAdapter(cmd);
                adt.Fill(dt);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                
            }
            finally
            {
                Disconnect();
            }
            return dt;
        }
        
    }
}
