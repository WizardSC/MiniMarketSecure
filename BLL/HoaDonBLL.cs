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
    public class HoaDonBLL
    {
        private HoaDonDAL hdDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public HoaDonBLL()
        {
            hdDAL = new HoaDonDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public DataTable getListHoaDon()
        {
            DataTable tempHD = hdDAL.getListHoaDon();
            if (tempHD != null && tempHD.Rows.Count > 0)
            {
                DataTable dtHD = tempHD.Clone();
                dtHD = tempHD.AsEnumerable()
                    .Select(row => dtHD.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaHD"), newKeyAES),
                        row.Field<DateTime>("NgayLapHD"),
                        row.Field<int>("TongTienTT"),
                        row.Field<int>("DiemSuDung"),
                        row.Field<int>("TongTien"),
                        row.Field<int>("DiemNhanDuoc"),
                        AES.DecryptAES(row.Field<string>("MaKM"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaNV"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaKH"), newKeyAES),


                        // Thêm các trường cần giải mã khác nếu có
                    }, false))
                    .CopyToDataTable();
                dtHD.DefaultView.Sort = "MaHD ASC";

                return dtHD;
            }
            else
            {
                return tempHD;
            }
            //return hdDAL.getListHoaDon();
        }
        public bool insertHoaDon(HoaDonDTO hd)
        {
            hd.MaHD = AES.EncryptAES(hd.MaHD, newKeyAES);
            //hd.TongTienTT = XOR.EncryptXOR(hd.TongTienTT, keyXOR);
            //hd.DiemSuDung = XOR.EncryptXOR(hd.DiemSuDung, keyXOR);
            //hd.TongTien = XOR.EncryptXOR(Convert.ToInt32(hd.TongTien), keyXOR);
            //hd.DiemNhanDuoc = XOR.EncryptXOR(hd.DiemNhanDuoc, keyXOR);
            hd.MaKM = AES.EncryptAES(hd.MaKM, newKeyAES);
            hd.MaNV = AES.EncryptAES(hd.MaNV, newKeyAES);
            hd.MaKH = AES.EncryptAES(hd.MaKH, newKeyAES);
            return hdDAL.insertHoaDon(hd);
        }
        public bool updateHoaDon(HoaDonDTO hd)
        {
            hd.MaHD = AES.EncryptAES(hd.MaHD, newKeyAES);
            //hd.TongTienTT = XOR.EncryptXOR(hd.TongTienTT, keyXOR);
            //hd.DiemSuDung = XOR.EncryptXOR(hd.DiemSuDung, keyXOR);
            //hd.TongTien = XOR.EncryptXOR(Convert.ToInt32(hd.TongTien), keyXOR);
            //hd.DiemNhanDuoc = XOR.EncryptXOR(hd.DiemNhanDuoc, keyXOR);
            hd.MaKM = AES.EncryptAES(hd.MaKM, newKeyAES);
            hd.MaNV = AES.EncryptAES(hd.MaNV, newKeyAES);
            hd.MaKH = AES.EncryptAES(hd.MaKH, newKeyAES);
            return hdDAL.updateHoaDon(hd);
        }
        public DataTable getListXemHoaDon()
        {
            DataTable tempHD = hdDAL.getListXemHoaDon();
            if (tempHD != null && tempHD.Rows.Count > 0)
            {
                DataTable dtHD = tempHD.Clone();
                dtHD = tempHD.AsEnumerable()
                    .Select(row => dtHD.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaHD"), newKeyAES),
                        row.Field<DateTime>("NgayLapHD"),                
                        AES.DecryptAES(row.Field<string>("Ten"), newKeyAES),
                        row.Field<int>("DiemSuDung"),
                        row.Field<int>("TongTien"),
                        row.Field<int>("DiemNhanDuoc"),
                        AES.DecryptAES(row.Field<string>("Ten1"), newKeyAES),

                        // Thêm các trường cần giải mã khác nếu có
                    }, false))
                    .CopyToDataTable();
                dtHD.DefaultView.Sort = "MaHD ASC";

                return dtHD;
            }
            else
            {
                return tempHD;
            }
            return hdDAL.getListXemHoaDon();
        }
    }
}
