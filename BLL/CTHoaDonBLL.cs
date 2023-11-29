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
            maHD = AES.EncryptAES(maHD, newKeyAES);

            DataTable tempCTHD = cthdDAL.getListCTHDByMaHD(maHD);
            if (tempCTHD != null && tempCTHD.Rows.Count > 0)
            {
                DataTable dtCTHD = tempCTHD.Clone();
                dtCTHD = tempCTHD.AsEnumerable()
                    .Select(row => dtCTHD.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaSP"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenSP"), newKeyAES),
                        row.Field<int>("SoLuong"),
                        row.Field<int>("DonGiaBanDau"), 
                        row.Field<int>("DonGiaDaGiam"), 
                        row.Field<int>("ThanhTien"),
                        // Thêm các trường cần giải mã khác nếu có
                    }, false))
                    .CopyToDataTable();
                //dtCTHD.DefaultView.Sort = "MaHD ASC";

                return dtCTHD;
            }
            else
            {
                return tempCTHD;
            }
            //return cthdDAL.getListCTHDByMaHD(maHD);
        }
        public bool insertCTHoaDon(CTHoaDonDTO cthd)
        {
            Console.WriteLine("MaHD :" + cthd.MaHD);

            cthd.MaHD = AES.EncryptAES(cthd.MaHD, newKeyAES);
            cthd.MaSP = AES.EncryptAES(cthd.MaSP, newKeyAES);
            cthd.TenSP = AES.EncryptAES(cthd.TenSP, newKeyAES);
            //cthd.SoLuong = XOR.EncryptXOR(cthd.SoLuong, keyXOR);
            //cthd.DonGiaBanDau = XOR.EncryptXOR(cthd.DonGiaBanDau, keyXOR);
            //cthd.DonGiaDaGiam = XOR.EncryptXOR(cthd.DonGiaDaGiam, keyXOR);
            //cthd.PhanTramKM = XOR.EncryptXOR(cthd.PhanTramKM, keyXOR);
            //cthd.ThanhTien = XOR.EncryptXOR(cthd.ThanhTien, keyXOR);
            Console.WriteLine("MaHD :" + cthd.MaHD); 
            Console.WriteLine("MaSP :" + cthd.MaSP);

            //return true;
            return cthdDAL.insertCTHoaDon(cthd);
        }

        public bool updateCTHoaDon(CTHoaDonDTO cthd)
        {
            cthd.TenSP = AES.DecryptAES(cthd.TenSP, newKeyAES);
            cthd.SoLuong = XOR.DecryptXOR(cthd.SoLuong, keyXOR);
            cthd.DonGiaBanDau = XOR.DecryptXOR(cthd.DonGiaBanDau, keyXOR);
            cthd.DonGiaDaGiam = XOR.DecryptXOR(cthd.DonGiaDaGiam, keyXOR);
            cthd.PhanTramKM = XOR.DecryptXOR(cthd.PhanTramKM, keyXOR);
            cthd.ThanhTien = XOR.DecryptXOR(cthd.ThanhTien, keyXOR);
            return cthdDAL.updateChiTietHoaDon(cthd);
        }
    }
}
