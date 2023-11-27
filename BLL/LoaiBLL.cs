using BLL.Crypto;
using DAO;
using DTO;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LoaiBLL
    {
        private LoaiDAL loaiDAL;

        
        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        
        private string keyVigenere;
        public LoaiBLL()
        {
            loaiDAL = new LoaiDAL();
           
            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));


        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }

        public string decryptKeyAES(string keyAES)
        {
            return Vigenere.DecryptVigenere(keyAES, keyVigenere);
        }
        public DataTable getListLoai()
        {

            DataTable tempLoai = loaiDAL.getListLoai();
            if (tempLoai != null && tempLoai.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtLoai = tempLoai.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtLoai = tempLoai.AsEnumerable()
                    .Select(row => dtLoai.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaLoai"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenLoai"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),


                    }, false))
                    .CopyToDataTable();
                dtLoai.DefaultView.Sort = "MaLoai ASC";

                return dtLoai;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempLoai;
            }
            //return loaiDAL.getListLoai();
        }
        public DataTable getListLoaiMini()
        {
            int trangThai = XOR.EncryptXOR(1, keyXOR);
            DataTable tempTable = loaiDAL.getListLoaiMini(trangThai);
            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNhaSanXuat = tempTable.Clone();

                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới

                dtNhaSanXuat = tempTable.AsEnumerable()
                    .Select(row => dtNhaSanXuat.LoadDataRow(new object[]
                    {
                       AES.DecryptAES(row.Field<string>("MaLoai"), newKeyAES),
                       AES.DecryptAES(row.Field<string>("TenLoai"), newKeyAES),
                    }, false))
                    .CopyToDataTable();
                dtNhaSanXuat.DefaultView.Sort = "MaLoai ASC";

                return dtNhaSanXuat;
            }
            else
            {
                return tempTable;
            }
            //return loaiDAL.getListLoaiMini(trangThai);
        }
        public bool insert_LoaiSP(LoaiDTO loai)
        {
            loai.MaLoai = AES.EncryptAES(loai.MaLoai, keyAES);
            loai.TenLoai = AES.EncryptAES(loai.TenLoai, keyAES);
            loai.TrangThaiLoai = XOR.EncryptXOR(loai.TrangThaiLoai, keyXOR);
            return loaiDAL.insertLoaiSP(loai);
        }
        public bool delete_LoaiSP(string MaLoai, out bool isLoiKhoaNgoai)
        {
            MaLoai = AES.EncryptAES(MaLoai, keyAES);
            return loaiDAL.delete_LoaiSP(MaLoai, out isLoiKhoaNgoai);
        }
        public bool update_LoaiSP(int Trangthai, string MaLoai)
        {
            MaLoai = AES.EncryptAES(MaLoai, keyAES);
            Trangthai = XOR.EncryptXOR(Trangthai, keyXOR);
            return loaiDAL.update_LoaiSP(Trangthai, MaLoai);
        }
        public bool update_LoaiSP(LoaiDTO loai)
        {
            loai.MaLoai = AES.EncryptAES(loai.MaLoai, newKeyAES);
            loai.TenLoai = AES.EncryptAES(loai.TenLoai, newKeyAES);
            loai.TrangThaiLoai = XOR.EncryptXOR(loai.TrangThaiLoai, keyXOR);
            return loaiDAL.update_LoaiSP(loai);
        }
    }
}
