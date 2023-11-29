using BLL.Crypto;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PhanQuyenBLL
    {
        private PhanQuyenDAL pqDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;
        public PhanQuyenBLL()
        {
            pqDAL = new PhanQuyenDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public PhanQuyenDTO getPhanQuyen(string tenPQ)
        {
            tenPQ = AES.EncryptAES(tenPQ, newKeyAES);
            PhanQuyenDTO pq = pqDAL.getPhanQuyen(tenPQ);

            return pq;
        }

        public bool updatePhanQuyen(PhanQuyenDTO pq)
        {
            Console.WriteLine(pq.TenPQ);
            pq.TenPQ = AES.EncryptAES(pq.TenPQ, newKeyAES);
            Console.WriteLine(pq.IsNhapHang);
            Console.WriteLine(pq.IsBanHang);

            return pqDAL.updatePhanQuyen(pq);
        }

        public bool insertPhanQuyen(string tenPQ)
        {
            tenPQ = AES.EncryptAES(tenPQ, newKeyAES);
            return pqDAL.insertPhanQuyen(tenPQ);
        }
    }
}
