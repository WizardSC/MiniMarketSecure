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
                        XOR.DecryptXOR(row.Field<int>("SoLuong"), keyXOR),
                        XOR.DecryptXOR(row.Field<int>("DonGiaBanDau"), keyXOR),
                        XOR.DecryptXOR(row.Field<int>("DonGiaDaGiam"), keyXOR),
                        XOR.DecryptXOR(row.Field<int>("ThanhTien"), keyXOR),
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
            cthd.MaHD = AES.DecryptAES(cthd.MaHD, newKeyAES);
            cthd.MaSP = AES.DecryptAES(cthd.MaSP, newKeyAES);

            cthd.TenSP = AES.DecryptAES(cthd.TenSP, newKeyAES);
            cthd.SoLuong = XOR.DecryptXOR(cthd.SoLuong, keyXOR);
            cthd.DonGiaBanDau = XOR.DecryptXOR(cthd.DonGiaBanDau, keyXOR);
            cthd.DonGiaDaGiam = XOR.DecryptXOR(cthd.DonGiaDaGiam, keyXOR);
            cthd.PhanTramKM = XOR.DecryptXOR(cthd.PhanTramKM, keyXOR);
            cthd.ThanhTien = XOR.DecryptXOR(cthd.ThanhTien, keyXOR);
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
