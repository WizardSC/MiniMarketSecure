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
    public class CTHoaDonBLL
    {
        private CTHoaDonDAL cthdDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public CTHoaDonBLL()
        {
            cthdDAL = new CTHoaDonDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public DataTable getListCTHD()
        {
            return cthdDAL.getListCTHD();
        }
        public DataTable getListCTHDbyMaHD(string maHD)
        {
            return cthdDAL.getListCTHDByMaHD(maHD);
        }
        public bool insertCTHoaDon(CTHoaDonDTO cthd)
        {
            return cthdDAL.insertCTHoaDon(cthd);
        }

        public bool updateCTHoaDon(CTHoaDonDTO cthd)
        {
            return cthdDAL.updateChiTietHoaDon(cthd);
        }
    }
}
