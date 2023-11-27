using BLL.Crypto;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NhanVienBLL
    {
        private NhanVienDAL nvDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public NhanVienBLL()
        {
            nvDAL = new NhanVienDAL();
            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public List<NhanVienDTO> getListNV()
        {
            List<NhanVienDTO> encryptedList = nvDAL.getListNV(); // Lấy danh sách đã mã hóa từ DAL
            List<NhanVienDTO> decryptedList = new List<NhanVienDTO>();
            foreach (NhanVienDTO encryptedNV in encryptedList)
            {
                NhanVienDTO decryptedNV = new NhanVienDTO();

                decryptedNV.MaNV = AES.DecryptAES(encryptedNV.MaNV, newKeyAES);
                decryptedNV.Ho = AES.DecryptAES(encryptedNV.Ho, newKeyAES);
                decryptedNV.Ten = AES.DecryptAES(encryptedNV.Ten, newKeyAES);
                //Console.WriteLine("Xin chao");
                //Console.WriteLine(decryptedNV.MaNV + decryptedNV.Ho + decryptedNV.Ten);
                decryptedList.Add(decryptedNV);
            }

            return decryptedList;
            //return nvDAL.getListNV();
        }
        public DataTable getListNhanVien()
        {
            DataTable tempNV = nvDAL.getListNhanVien();
            if (tempNV != null && tempNV.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNCC = tempNV.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtNCC = tempNV.AsEnumerable()
                    .Select(row => dtNCC.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaNCC"),

                        AES.DecryptAES(row.Field<string>("MaNV"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ho"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ten"), newKeyAES),
                        row.Field<DateTime>("NgaySinh"),
                        AES.DecryptAES(row.Field<string>("GioiTinh"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("SoDT"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("DiaChi"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                        AES.DecryptAES(row.Field<string>("MaTK"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaCV"), newKeyAES),
                        row.Field<string>("IMG"),


                    }, false))
                    .CopyToDataTable();
                dtNCC.DefaultView.Sort = "MaNV ASC";

                return dtNCC;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempNV;
            }
            //return nvDAL.getListNhanVien();
        }
        public DataTable getListNVNoHasTaiKhoan()
        {
            int trangThai = XOR.EncryptXOR(1, keyXOR);
            DataTable tempNV = nvDAL.getListNhanVienNoHasTK(trangThai);
            if (tempNV != null && tempNV.Rows.Count > 0)
            {
                DataTable dtNV = tempNV.Clone();
                dtNV = tempNV.AsEnumerable()
                    .Select(row => dtNV.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaNV"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ho"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ten"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaTK"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaCV"), newKeyAES),
                    }, false))
                    .CopyToDataTable();
                dtNV.DefaultView.Sort = "MaNV ASC";

                return dtNV;
            }
            else
            {
                return tempNV;
            }
            //return nvDAL.getListNhanVienNoHasTK(trangThai);
        }
        public DataTable getListNVHasTaiKhoan()
        {
            int trangThai = XOR.EncryptXOR(1, keyXOR);
            string maTaiKhoan = AES.EncryptAES("TK001", newKeyAES);
            DataTable tempNV = nvDAL.getListNhanVienHasTK(trangThai, maTaiKhoan);
            if (tempNV != null && tempNV.Rows.Count > 0)
            {
                DataTable dtNV = tempNV.Clone();
                dtNV = tempNV.AsEnumerable()
                    .Select(row => dtNV.LoadDataRow(new object[]
                    {
                    AES.DecryptAES(row.Field<string>("MaNV"), newKeyAES),
                    AES.DecryptAES(row.Field<string>("Ho"), newKeyAES),
                    AES.DecryptAES(row.Field<string>("Ten"), newKeyAES),
                    AES.DecryptAES(row.Field<string>("MaTK"), newKeyAES),
                    AES.DecryptAES(row.Field<string>("MaCV"), newKeyAES),
                        row.Field<DateTime>("NgaySinh"),

                        // Thêm các trường cần giải mã khác nếu có
                    }, false))
                    .CopyToDataTable();
                dtNV.DefaultView.Sort = "MaNV ASC";

                return dtNV;
            }
            else
            {
                return tempNV;
            }
            //return nvDAL.getListNhanVienHasTK();
        }
        

        public DataTable getCurrentNVHasTK(string maNV)
        {
            return nvDAL.getCurrentNVHasTK(maNV);
        }
        public bool insertNhanVien(NhanVienDTO nv)
        {
            nv.MaNV = AES.EncryptAES(nv.MaNV, newKeyAES);
            nv.Ho = AES.EncryptAES(nv.Ho, newKeyAES);
            nv.Ten = AES.EncryptAES(nv.Ten, newKeyAES);
            //nv.NgaySinh = AES.EncryptAES(nv.NgaySinh.ToStr, newKeyAES);
            nv.GioiTinh = AES.EncryptAES(nv.GioiTinh, newKeyAES);
            nv.DiaChi = AES.EncryptAES(nv.DiaChi, newKeyAES);
            nv.SoDT = AES.EncryptAES(nv.SoDT, newKeyAES);
            nv.TrangThai = XOR.EncryptXOR(nv.TrangThai, keyXOR);
            nv.MaTK = AES.EncryptAES(nv.MaTK, newKeyAES);
            nv.MaCV = AES.EncryptAES(nv.MaCV, newKeyAES);

            return nvDAL.insertNhanVien(nv);
        }
        public bool updateNhanVien(NhanVienDTO nv)
        {

            nv.MaNV = AES.EncryptAES(nv.MaNV, newKeyAES);
            nv.Ho = AES.EncryptAES(nv.Ho, newKeyAES);
            nv.Ten = AES.EncryptAES(nv.Ten, newKeyAES);
            //nv.NgaySinh = AES.EncryptAES(nv.NgaySinh.ToStr, newKeyAES);
            nv.GioiTinh = AES.EncryptAES(nv.GioiTinh, newKeyAES);
            nv.DiaChi = AES.EncryptAES(nv.DiaChi, newKeyAES);
            nv.SoDT = AES.EncryptAES(nv.SoDT, newKeyAES);
            nv.TrangThai = XOR.EncryptXOR(nv.TrangThai, keyXOR);
            if (string.IsNullOrEmpty(nv.MaTK))
            {
                nv.MaTK = string.Empty;

            }
            else
            {
                nv.MaTK = AES.EncryptAES(nv.MaTK, newKeyAES);

            }
            nv.MaCV = AES.EncryptAES(nv.MaCV, newKeyAES);

            Console.WriteLine(nv.MaTK);
            //return true;
            return nvDAL.updateNhanVien(nv);
        }
        public bool deleteNhanVien(string maNV, out bool isLoiKhoaNgoai)
        {
            maNV = AES.EncryptAES(maNV, newKeyAES);
            return nvDAL.deleteNhanVien(maNV, out isLoiKhoaNgoai);
        }
        public bool updateTrangThai(int trangThai, string maNV)
        {
            maNV = AES.EncryptAES(maNV, newKeyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);

            trangThai = (trangThai == 0) ? 1 : 0;
            return nvDAL.updateTrangThai(trangThai, maNV);
        }
        public bool updateTaiKhoan(string maTK, string maNV)
        {
            maNV = AES.EncryptAES(maNV, newKeyAES);
            maTK = AES.EncryptAES(maTK, newKeyAES);

            return nvDAL.updateTaiKhoan(maTK, maNV);
        }
    }
}
