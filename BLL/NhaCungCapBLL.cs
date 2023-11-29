using BLL.Crypto;
using DAL;
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
    public class NhaCungCapBLL
    {
        private NhaCungCapDAL nccDAL;


        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public NhaCungCapBLL()
        {
            nccDAL = new NhaCungCapDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));
        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }

        public DataTable getListNCC()
        {
            DataTable tempNCC = nccDAL.getListNhaCC();
            if (tempNCC != null && tempNCC.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNCC = tempNCC.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtNCC = tempNCC.AsEnumerable()
                    .Select(row => dtNCC.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaNCC"),

                        AES.DecryptAES(row.Field<string>("MaNCC"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenNCC"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("DiaChi"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("SoDT"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("SoFAX"), newKeyAES),

                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                        row.Field<string>("IMG"),


                    }, false))
                    .CopyToDataTable();
                dtNCC.DefaultView.Sort = "MaNCC ASC";

                return dtNCC;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempNCC;
            }
            return nccDAL.getListNhaCC();
        }

        public DataTable getListNCCMini()
        {
            int trangThai = XOR.EncryptXOR(1, keyXOR);
            DataTable tempNCC = nccDAL.getListNhaCCMini(trangThai);
            if (tempNCC != null && tempNCC.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNCC = tempNCC.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtNCC = tempNCC.AsEnumerable()
                    .Select(row => dtNCC.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaNCC"),

                        AES.DecryptAES(row.Field<string>("MaNCC"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenNCC"), newKeyAES),



                    }, false))
                    .CopyToDataTable();
                dtNCC.DefaultView.Sort = "MaNCC ASC";

                return dtNCC;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempNCC;
            }
            return nccDAL.getListNhaCCMini(trangThai);
        }
        public NhaCungCapDTO getNhaCungCapbyMaNCC(string maNCC)
        {
            maNCC = AES.EncryptAES(maNCC, newKeyAES);

            NhaCungCapDTO ncc = nccDAL.getNhaCungCapbyMaNCC(maNCC);
            ncc.TenNCC = AES.DecryptAES(ncc.TenNCC, newKeyAES);
            ncc.DiaChi= AES.DecryptAES(ncc.DiaChi, newKeyAES);
            ncc.SoDT = AES.DecryptAES(ncc.SoDT, newKeyAES);
            ncc.SoFAX = AES.DecryptAES(ncc.SoFAX, newKeyAES);

            return ncc;
        }
        public bool insertNhaCungCap(NhaCungCapDTO ncc)
        {
            ncc.MaNCC = AES.EncryptAES(ncc.MaNCC, newKeyAES);
            ncc.TenNCC = AES.EncryptAES(ncc.TenNCC, newKeyAES);
            ncc.DiaChi = AES.EncryptAES(ncc.DiaChi, newKeyAES);
            ncc.SoDT = AES.EncryptAES(ncc.SoDT, newKeyAES);
            ncc.SoFAX = AES.EncryptAES(ncc.SoFAX, newKeyAES);
            ncc.TrangThai = XOR.EncryptXOR(ncc.TrangThai, keyXOR);
            return nccDAL.insertNhaCC(ncc);
        }
        public bool updateNhaCC(NhaCungCapDTO ncc)
        {
            ncc.MaNCC = AES.EncryptAES(ncc.MaNCC, newKeyAES);
            ncc.TenNCC = AES.EncryptAES(ncc.TenNCC, newKeyAES);
            ncc.DiaChi = AES.EncryptAES(ncc.DiaChi, newKeyAES);
            ncc.SoDT = AES.EncryptAES(ncc.SoDT, newKeyAES);
            ncc.SoFAX = AES.EncryptAES(ncc.SoFAX, newKeyAES);
            ncc.TrangThai = XOR.EncryptXOR(ncc.TrangThai, keyXOR);
            return nccDAL.updateNhaCC(ncc);
        }
        public bool deleteNhaCCC(string maNCC, out bool isLoiKhoaNgoai)
        {
            maNCC = AES.EncryptAES(maNCC, newKeyAES);
            return nccDAL.deleteNhaCC(maNCC, out isLoiKhoaNgoai);
        }
        public bool updateTrangThai(int trangThai, string maNCC)
        {
            maNCC = AES.EncryptAES(maNCC, newKeyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);

            trangThai = (trangThai == 0) ? 1 : 0;
            return nccDAL.updateTrangThai(trangThai, maNCC);
        }



    }
}
