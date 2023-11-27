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
        string publicKeyPem;
        string privateKeyPem;
        RsaKeyParameters publicKeyRSA;
        RsaPrivateCrtKeyParameters privateKeyRSA;
        private int keyXOR;
        private string keyAES;


        public NhaSanXuatBLL()
        {
            nsxDAL = new NhaSanXuatDAL();
            publicKeyPem = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCdkdunv/XlyPKPdLQ5MIAc3spph/4r8FQM2K8oKDGRtiZ5Ro+2HLk81xXVhaHNh7ThgOENKcTDrils1LKOpkRaU+FSg9wGkEiO5Dkt0oF3H2vF+KKz0hf2Q+s0o92k749k03DSMJp/UnfTODqjjqlb3SkVT3Xpz1LBpf7nxF4SyQIDAQAB";
            privateKeyPem = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAJ2R26e/9eXI8o90tDkwgBzeymmH/ivwVAzYrygoMZG2JnlGj7YcuTzXFdWFoc2HtOGA4Q0pxMOuKWzUso6mRFpT4VKD3AaQSI7kOS3SgXcfa8X4orPSF/ZD6zSj3aTvj2TTcNIwmn9Sd9M4OqOOqVvdKRVPdenPUsGl/ufEXhLJAgMBAAECgYBvEOfmt21pmho3ukhq41/6eaPtfRlQ+WcVYwsb4DEIh39ZDr6v3FFJrEZMqpQhnp2GMmTv9VgnojS67LYrzNDySyWywKcLb5iqRhXLUFIcW4VBobB3RTF14TP3Z8/9JwSPIya0LPfyKekGfSmiPvn2AK+NP/plHboPP8Pj7NZvAQJBANKkEHX1MY/A4nwmAQBwZxzMb67OAhvpqLZO+stvsMTQHVVlhdyktZaEwZAz7ZqzE45VLM8qR+K591rchqB3jCECQQC/gCiFiZ6mnABQsD3zDHQfoUPGF6ezG1EL2253IC5wLO66AxP1mE3OLNhD5GCj6SHOVCZOGbe5hI1NX1Q9mHGpAkA9b+LpNMHm5uBoS4FhSmeYy7wLZUTSNV2jBvg5W/NYBEd5/+mCSHw6UbBPSgYUaATdL9So+/EJnSobU1Tk+tlhAkAP7A7yc2vOfIAGNXEhKTLqglUJPmRCHw5RBxCbNQDHcOT0cFcTf3NQpifhFTb3yeV2q0Z4DelZsmyxNzDg4jDpAkEAigr8ikcyBBrDBoZ4ZX6i40N2a5YmGXIehbjOgx2IH5NnGsBPS5hdMOF3hM/G8f77UdqICoMFvKVWtcsX4F7gLg==";
            publicKeyRSA = RSACrypto.ConvertPemToPublicKey(publicKeyPem);
            privateKeyRSA = RSACrypto.ConvertPemToPrivateKey(privateKeyPem);
            keyAES = "0123456789ABCDEF";
            keyXOR = 33;

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

                        AES.DecryptAES(row.Field<string>("MaNSX"),keyAES),
                        //row.Field<string>("TenNSX"),
                        //row.Field<string>("DiaChi"),
                        //row.Field<string>("SoDT"),
                        //Encoding.UTF8.GetString(RSACrypto.RSADecrypt(RSACrypto.FromHexString(row.Field<string>("MaNSX")), privateKeyRSA)),
                        Encoding.UTF8.GetString(RSACrypto.RSADecrypt(RSACrypto.FromHexString(row.Field<string>("TenNSX")), privateKeyRSA)),
                        Encoding.UTF8.GetString(RSACrypto.RSADecrypt(RSACrypto.FromHexString(row.Field<string>("DiaChi")), privateKeyRSA)),
                        Encoding.UTF8.GetString(RSACrypto.RSADecrypt(RSACrypto.FromHexString(row.Field<string>("SoDT")), privateKeyRSA)),
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
            return nsxDAL.getListNhaSanXuat();
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
                        AES.DecryptAES(row.Field<string>("MaNSX"),keyAES),
                        Encoding.UTF8.GetString(RSACrypto.RSADecrypt(RSACrypto.FromHexString(row.Field<string>("TenNSX")), privateKeyRSA)),
                    }, false))
                    .CopyToDataTable();
                dtNhaSanXuat.DefaultView.Sort = "MaNSX ASC";

                return dtNhaSanXuat;
            }
            else
            {
                return tempTable;
            }
            //return nsxDAL.getListNhaSanXuatMini();
        }
        public bool insertNhaSanXuat(NhaSanXuatDTO nsx)
        {
            nsx.MaNSX = AES.EncryptAES(nsx.MaNSX, keyAES);
            nsx.TenNSX = BitConverter.ToString(
                RSACrypto.RSAEncrypt(
                    Encoding.UTF8.GetBytes(nsx.TenNSX),
                    publicKeyRSA
                )
            ).Replace("-", "");
            nsx.DiaChi = BitConverter.ToString(
                RSACrypto.RSAEncrypt(
                    Encoding.UTF8.GetBytes(nsx.DiaChi),
                    publicKeyRSA
                )
            ).Replace("-", "");
            nsx.SoDT = BitConverter.ToString(
                RSACrypto.RSAEncrypt(
                    Encoding.UTF8.GetBytes(nsx.SoDT),
                    publicKeyRSA
                )
            ).Replace("-", "");
            nsx.TrangThaiNSX = XOR.EncryptXOR(nsx.TrangThaiNSX, keyXOR);
            return nsxDAL.insertNhaSanXuat(nsx);
        }

        public bool update_nhasanxuat(NhaSanXuatDTO nsx)
        {
            nsx.MaNSX = AES.EncryptAES(nsx.MaNSX, keyAES);
            nsx.TenNSX = BitConverter.ToString(
                RSACrypto.RSAEncrypt(
                    Encoding.UTF8.GetBytes(nsx.TenNSX),
                    publicKeyRSA
                )
            ).Replace("-", "");
            nsx.DiaChi = BitConverter.ToString(
                RSACrypto.RSAEncrypt(
                    Encoding.UTF8.GetBytes(nsx.DiaChi),
                    publicKeyRSA
                )
            ).Replace("-", "");
            nsx.SoDT = BitConverter.ToString(
                RSACrypto.RSAEncrypt(
                    Encoding.UTF8.GetBytes(nsx.SoDT),
                    publicKeyRSA
                )
            ).Replace("-", "");
            nsx.TrangThaiNSX = XOR.EncryptXOR(nsx.TrangThaiNSX, keyXOR);

            return nsxDAL.update_nhasanxuat(nsx);
        }
        //public bool delete_nhasanxuat(NhaSanXuatDTO nsx)maSP, out isLoiKhoaNgoai //string maSP, out bool isLoiKhoaNgoai
        public bool delete_nhasanxuat(string MaNSX, out bool isLoiKhoaNgoai)
        {
            MaNSX = AES.EncryptAES(MaNSX, keyAES);
            return nsxDAL.delete_nhasanxuat(MaNSX, out isLoiKhoaNgoai);
        }
        public bool update_nhasanxuat(int trangThai, string MaNSX)
        {
            MaNSX = AES.EncryptAES(MaNSX, keyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);
            trangThai = (trangThai == 0) ? 1 : 0;
            return nsxDAL.update_nhasanxuat(trangThai, MaNSX);
        }
    }
}
