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

        string publicKeyPem;
        string privateKeyPem;
        RsaKeyParameters publicKeyRSA;
        RsaPrivateCrtKeyParameters privateKeyRSA;
        private int keyXOR;
        private string keyAES;
        public LoaiBLL()
        {
            loaiDAL = new LoaiDAL();
            publicKeyPem = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCdkdunv/XlyPKPdLQ5MIAc3spph/4r8FQM2K8oKDGRtiZ5Ro+2HLk81xXVhaHNh7ThgOENKcTDrils1LKOpkRaU+FSg9wGkEiO5Dkt0oF3H2vF+KKz0hf2Q+s0o92k749k03DSMJp/UnfTODqjjqlb3SkVT3Xpz1LBpf7nxF4SyQIDAQAB";
            privateKeyPem = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAJ2R26e/9eXI8o90tDkwgBzeymmH/ivwVAzYrygoMZG2JnlGj7YcuTzXFdWFoc2HtOGA4Q0pxMOuKWzUso6mRFpT4VKD3AaQSI7kOS3SgXcfa8X4orPSF/ZD6zSj3aTvj2TTcNIwmn9Sd9M4OqOOqVvdKRVPdenPUsGl/ufEXhLJAgMBAAECgYBvEOfmt21pmho3ukhq41/6eaPtfRlQ+WcVYwsb4DEIh39ZDr6v3FFJrEZMqpQhnp2GMmTv9VgnojS67LYrzNDySyWywKcLb5iqRhXLUFIcW4VBobB3RTF14TP3Z8/9JwSPIya0LPfyKekGfSmiPvn2AK+NP/plHboPP8Pj7NZvAQJBANKkEHX1MY/A4nwmAQBwZxzMb67OAhvpqLZO+stvsMTQHVVlhdyktZaEwZAz7ZqzE45VLM8qR+K591rchqB3jCECQQC/gCiFiZ6mnABQsD3zDHQfoUPGF6ezG1EL2253IC5wLO66AxP1mE3OLNhD5GCj6SHOVCZOGbe5hI1NX1Q9mHGpAkA9b+LpNMHm5uBoS4FhSmeYy7wLZUTSNV2jBvg5W/NYBEd5/+mCSHw6UbBPSgYUaATdL9So+/EJnSobU1Tk+tlhAkAP7A7yc2vOfIAGNXEhKTLqglUJPmRCHw5RBxCbNQDHcOT0cFcTf3NQpifhFTb3yeV2q0Z4DelZsmyxNzDg4jDpAkEAigr8ikcyBBrDBoZ4ZX6i40N2a5YmGXIehbjOgx2IH5NnGsBPS5hdMOF3hM/G8f77UdqICoMFvKVWtcsX4F7gLg==";
            publicKeyRSA = RSACrypto.ConvertPemToPublicKey(publicKeyPem);
            privateKeyRSA = RSACrypto.ConvertPemToPrivateKey(privateKeyPem);
            keyAES = "0123456789ABCDEF";
            keyXOR = 33;
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
                        AES.DecryptAES(row.Field<string>("MaLoai"), keyAES),
                        Encoding.UTF8.GetString(RSACrypto.RSADecrypt(RSACrypto.FromHexString(row.Field<string>("TenLoai")), privateKeyRSA)),
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
                        AES.DecryptAES(row.Field<string>("MaLoai"),keyAES),
                        Encoding.UTF8.GetString(RSACrypto.RSADecrypt(RSACrypto.FromHexString(row.Field<string>("TenLoai")), privateKeyRSA)),
                    }, false))
                    .CopyToDataTable();
                dtNhaSanXuat.DefaultView.Sort = "MaLoai ASC";

                return dtNhaSanXuat;
            }
            else
            {
                return tempTable;
            }
            //return loaiDAL.getListLoaiMini();
        }
        public bool insert_LoaiSP(LoaiDTO loai)
        {
            loai.MaLoai = AES.EncryptAES(loai.MaLoai, keyAES);

            loai.TenLoai = BitConverter.ToString(
               RSACrypto.RSAEncrypt(
                   Encoding.UTF8.GetBytes(loai.TenLoai),
                   publicKeyRSA
               )
           ).Replace("-", "");
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
            loai.MaLoai = AES.EncryptAES(loai.MaLoai, keyAES);
            loai.TenLoai = BitConverter.ToString(
                RSACrypto.RSAEncrypt(
                    Encoding.UTF8.GetBytes(loai.TenLoai),
                    publicKeyRSA
                )
            ).Replace("-", "");
            loai.TrangThaiLoai = XOR.EncryptXOR(loai.TrangThaiLoai, keyXOR);
            return loaiDAL.update_LoaiSP(loai);
        }
    }
}
