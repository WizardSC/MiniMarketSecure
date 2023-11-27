using BLL.Crypto;
using DAL;
using DTO;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NhaSanXuatBLL
    {
        private NhaSanXuatDAL nsxDAL;
        private List<NhaSanXuatDTO> listNhaSanXuat = null;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;


        public NhaSanXuatBLL()
        {
            nsxDAL = new NhaSanXuatDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public DataTable getListNSX()
        {
            DataTable tempTable = nsxDAL.getListNhaSanXuat();
            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNhaSanXuat = tempTable.Clone();

                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới

                dtNhaSanXuat = tempTable.AsEnumerable()
                    .Select(row => dtNhaSanXuat.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaNSX"),

                        AES.DecryptAES(row.Field<string>("MaNSX"),newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenNSX"),newKeyAES),
                        AES.DecryptAES(row.Field<string>("DiaChi"),newKeyAES),
                        AES.DecryptAES(row.Field<string>("SoDT"),newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                    }, false))
                    .CopyToDataTable();
                dtNhaSanXuat.DefaultView.Sort = "MaNSX ASC";

                return dtNhaSanXuat;
            }
            else
            {

                return tempTable;
            }
            //return nsxDAL.getListNhaSanXuat();
        }

        public DataTable getListNSXMini()
        {
            int trangThai = XOR.EncryptXOR(1, keyXOR);
            DataTable tempTable = nsxDAL.getListNhaSanXuatMini(trangThai);
            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNhaSanXuat = tempTable.Clone();

                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới

                dtNhaSanXuat = tempTable.AsEnumerable()
                    .Select(row => dtNhaSanXuat.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaNSX"),newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenNSX"),newKeyAES),
                    }, false))
                    .CopyToDataTable();
                dtNhaSanXuat.DefaultView.Sort = "MaNSX ASC";

                return dtNhaSanXuat;
            }
            else
            {
                return tempTable;
            }
            //return nsxDAL.getListNhaSanXuatMini(trangThai);
        }
        public bool insertNhaSanXuat(NhaSanXuatDTO nsx)
        {
            nsx.MaNSX = AES.EncryptAES(nsx.MaNSX, newKeyAES);
            nsx.TenNSX = AES.EncryptAES(nsx.TenNSX, newKeyAES);
            nsx.DiaChi = AES.EncryptAES(nsx.DiaChi, newKeyAES);
            nsx.SoDT = AES.EncryptAES(nsx.SoDT, newKeyAES);
            nsx.TrangThaiNSX = XOR.EncryptXOR(nsx.TrangThaiNSX, keyXOR);
            return nsxDAL.insertNhaSanXuat(nsx);
        }

        public bool update_nhasanxuat(NhaSanXuatDTO nsx)
        {
            nsx.MaNSX = AES.EncryptAES(nsx.MaNSX, newKeyAES);
            nsx.TenNSX = AES.EncryptAES(nsx.TenNSX, newKeyAES);
            nsx.DiaChi = AES.EncryptAES(nsx.DiaChi, newKeyAES);
            nsx.SoDT = AES.EncryptAES(nsx.SoDT, newKeyAES);
            nsx.TrangThaiNSX = XOR.EncryptXOR(nsx.TrangThaiNSX, keyXOR);

            return nsxDAL.update_nhasanxuat(nsx);
        }
        //public bool delete_nhasanxuat(NhaSanXuatDTO nsx)maSP, out isLoiKhoaNgoai //string maSP, out bool isLoiKhoaNgoai
        public bool delete_nhasanxuat(string MaNSX, out bool isLoiKhoaNgoai)
        {
            MaNSX = AES.EncryptAES(MaNSX, newKeyAES);
            return nsxDAL.delete_nhasanxuat(MaNSX, out isLoiKhoaNgoai);
        }
        public bool update_nhasanxuat(int trangThai, string MaNSX)
        {
            MaNSX = AES.EncryptAES(MaNSX, newKeyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);
            trangThai = (trangThai == 0) ? 1 : 0;
            return nsxDAL.update_nhasanxuat(trangThai, MaNSX);
        }
    }
}
