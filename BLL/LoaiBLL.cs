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
        public LoaiBLL()
        {
            loaiDAL = new LoaiDAL();
            publicKeyPem = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCdkdunv/XlyPKPdLQ5MIAc3spph/4r8FQM2K8oKDGRtiZ5Ro+2HLk81xXVhaHNh7ThgOENKcTDrils1LKOpkRaU+FSg9wGkEiO5Dkt0oF3H2vF+KKz0hf2Q+s0o92k749k03DSMJp/UnfTODqjjqlb3SkVT3Xpz1LBpf7nxF4SyQIDAQAB";
            privateKeyPem = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAJ2R26e/9eXI8o90tDkwgBzeymmH/ivwVAzYrygoMZG2JnlGj7YcuTzXFdWFoc2HtOGA4Q0pxMOuKWzUso6mRFpT4VKD3AaQSI7kOS3SgXcfa8X4orPSF/ZD6zSj3aTvj2TTcNIwmn9Sd9M4OqOOqVvdKRVPdenPUsGl/ufEXhLJAgMBAAECgYBvEOfmt21pmho3ukhq41/6eaPtfRlQ+WcVYwsb4DEIh39ZDr6v3FFJrEZMqpQhnp2GMmTv9VgnojS67LYrzNDySyWywKcLb5iqRhXLUFIcW4VBobB3RTF14TP3Z8/9JwSPIya0LPfyKekGfSmiPvn2AK+NP/plHboPP8Pj7NZvAQJBANKkEHX1MY/A4nwmAQBwZxzMb67OAhvpqLZO+stvsMTQHVVlhdyktZaEwZAz7ZqzE45VLM8qR+K591rchqB3jCECQQC/gCiFiZ6mnABQsD3zDHQfoUPGF6ezG1EL2253IC5wLO66AxP1mE3OLNhD5GCj6SHOVCZOGbe5hI1NX1Q9mHGpAkA9b+LpNMHm5uBoS4FhSmeYy7wLZUTSNV2jBvg5W/NYBEd5/+mCSHw6UbBPSgYUaATdL9So+/EJnSobU1Tk+tlhAkAP7A7yc2vOfIAGNXEhKTLqglUJPmRCHw5RBxCbNQDHcOT0cFcTf3NQpifhFTb3yeV2q0Z4DelZsmyxNzDg4jDpAkEAigr8ikcyBBrDBoZ4ZX6i40N2a5YmGXIehbjOgx2IH5NnGsBPS5hdMOF3hM/G8f77UdqICoMFvKVWtcsX4F7gLg==";
            publicKeyRSA = RSACrypto.ConvertPemToPublicKey(publicKeyPem);
            privateKeyRSA = RSACrypto.ConvertPemToPrivateKey(privateKeyPem);
            keyXOR = 33;
        }
        public DataTable getListLoai()
        {
            return loaiDAL.getListLoai();
        }

      
        public string getMaxMaLoai()
        {
            return loaiDAL.getMaxMaLoai();
        }
        public DataTable getListLoaiMini()
        {
            return loaiDAL.getListLoaiMini();
        }
        public bool insert_LoaiSP(LoaiDTO LSP)
        {
            return loaiDAL.insertLoaiSP(LSP);
        }
        public bool delete_LoaiSP(string MaNSX, out bool isLoiKhoaNgoai)
        {
            return loaiDAL.delete_LoaiSP(MaNSX, out isLoiKhoaNgoai);
        }
        public bool update_LoaiSP(int Trangthai, string MaLoai)
        {
            Trangthai = (Trangthai == 0) ? 1 : 0;
           return loaiDAL.update_LoaiSP(Trangthai, MaLoai);
        }
        public bool update_LoaiSP(LoaiDTO loaiDTO)
        {
            return loaiDAL.update_LoaiSP(loaiDTO);
        }
    }
}
