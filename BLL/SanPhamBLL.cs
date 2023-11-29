using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using BLL.Crypto;

namespace BLL
{
    public class SanPhamBLL
    {
        private SanPhamDAL spDAL;
        private List<SanPhamDTO> listSanPham;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;
        public SanPhamBLL()
        {
            spDAL = new SanPhamDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));
        }

        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }

        public DataTable getListSanPham()
        {
            DataTable tempTable = spDAL.getListSanPham();
            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtSanPham = tempTable.Clone();

                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới

                dtSanPham = tempTable.AsEnumerable()
                    .Select(row => dtSanPham.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaSP"),
                        AES.DecryptAES(row.Field<string>("MaSP"),newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenSP"),newKeyAES),
                        row.Field<int>("SoLuong"),
                        row.Field<int>("DonGiaNhap"),
                        row.Field<int>("DonGiaBan"),
                        AES.DecryptAES(row.Field<string>("DonViTinh"),newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                        AES.DecryptAES(row.Field<string>("MaLoai"),newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaNSX"),newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaNCC"),newKeyAES),
                        row.Field<string>("IMG"),
                    }, false))
                    .CopyToDataTable();
                dtSanPham.DefaultView.Sort = "MaSP ASC";

                return dtSanPham;
            }
            else
            {

                return tempTable;
            }
            //return spDAL.getListSanPham();
        }
        public List<SanPhamDTO> getListSP()
        {

            List<SanPhamDTO> encryptedList = spDAL.getListSP(); // Lấy danh sách đã mã hóa từ DAL
            List<SanPhamDTO> decryptedList = new List<SanPhamDTO>();
            foreach (SanPhamDTO encryptedSP  in encryptedList)
            {
                SanPhamDTO sp = new SanPhamDTO();
                sp.MaSP = AES.DecryptAES(encryptedSP.MaSP, newKeyAES);
                sp.TenSP = AES.DecryptAES(encryptedSP.TenSP, newKeyAES);
                sp.MaLoai = AES.DecryptAES(encryptedSP.MaLoai, newKeyAES);
                sp.MaNSX = AES.DecryptAES(encryptedSP.MaNSX, newKeyAES);
                sp.MaNCC = AES.DecryptAES(encryptedSP.MaNCC, newKeyAES);
                sp.SoLuong = encryptedSP.SoLuong;
                sp.DonGiaBan = encryptedSP.DonGiaBan;
                sp.DonGiaNhap = encryptedSP.DonGiaNhap;
                sp.TrangThaiSP = XOR.DecryptXOR(encryptedSP.TrangThaiSP, keyXOR);
                sp.DonViTinh = AES.DecryptAES(encryptedSP.DonViTinh, newKeyAES);

                decryptedList.Add(sp);


            }
            return decryptedList;
            //return spDAL.getListSP();
        }
        public bool insertSanPham(SanPhamDTO sp)
        {
            sp.MaSP = AES.EncryptAES(sp.MaSP, newKeyAES);
            sp.TenSP = AES.EncryptAES(sp.TenSP, newKeyAES);
            sp.MaLoai = AES.EncryptAES(sp.MaLoai, newKeyAES);
            sp.MaNSX = AES.EncryptAES(sp.MaNSX, newKeyAES);
            sp.MaNCC = AES.EncryptAES(sp.MaNCC, newKeyAES);
            //sp.SoLuong = XOR.EncryptXOR(sp.SoLuong, keyXOR);
            //sp.DonGiaBan = XOR.EncryptXOR(sp.DonGiaBan, keyXOR);
            //sp.DonGiaNhap = XOR.EncryptXOR(sp.DonGiaNhap, keyXOR);
            sp.TrangThaiSP = XOR.EncryptXOR(sp.TrangThaiSP, keyXOR);
            sp.DonViTinh = AES.EncryptAES(sp.DonViTinh, newKeyAES);
            return spDAL.insertSanPham(sp);
        }
        public bool updateSanPham(SanPhamDTO sp)
        {
            sp.MaSP = AES.EncryptAES(sp.MaSP, newKeyAES);
            sp.TenSP = AES.EncryptAES(sp.TenSP, newKeyAES);
            sp.MaLoai = AES.EncryptAES(sp.MaLoai, newKeyAES);
            sp.MaNSX = AES.EncryptAES(sp.MaNSX, newKeyAES);
            sp.MaNCC = AES.EncryptAES(sp.MaNCC, newKeyAES);
            //sp.SoLuong = XOR.EncryptXOR(sp.SoLuong, keyXOR);
            //sp.DonGiaBan = XOR.EncryptXOR(sp.DonGiaBan, keyXOR);
            //sp.DonGiaNhap = XOR.EncryptXOR(sp.DonGiaNhap, keyXOR);
            sp.TrangThaiSP = XOR.EncryptXOR(sp.TrangThaiSP, keyXOR);
            sp.DonViTinh = AES.EncryptAES(sp.DonViTinh, newKeyAES);
            return spDAL.updateSanPham(sp);
        }
        public bool deleteSanPham(string maSP, out bool isLoiKhoaNgoai)
        {
            maSP = AES.EncryptAES(maSP, newKeyAES);

            return spDAL.deleteSanPham(maSP, out isLoiKhoaNgoai);
        }
        public bool updateTrangThai(int trangThai, string maSP)
        {
            maSP = AES.EncryptAES(maSP, newKeyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);


            trangThai = (trangThai == 0) ? 1 : 0;
            return spDAL.updateTrangThai(trangThai, maSP);
        }

        public bool updateTonKho(string maSP, int soLuong)
        {
            //soLuong = XOR.EncryptXOR(soLuong, keyXOR);
            maSP = AES.EncryptAES(maSP, newKeyAES);

            return spDAL.updateTonKho(maSP, soLuong);
        }
    }
}
