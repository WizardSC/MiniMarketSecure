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
    public class CTPhieuNhapBLL
    {
        private CTPhieuNhapDAL ctpnDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;
        public CTPhieuNhapBLL()
        {
            ctpnDAL = new CTPhieuNhapDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public bool insertCTPN(CTPhieuNhapDTO ctpn)
        {
            ctpn.MaPN = AES.EncryptAES(ctpn.MaPN, newKeyAES);
            ctpn.MaSP = AES.EncryptAES(ctpn.MaSP, newKeyAES);
            ctpn.TenSP = AES.EncryptAES(ctpn.TenSP, newKeyAES);

            return ctpnDAL.insertCTPhieuNhap(ctpn);
        }

        public DataTable getListPhieuNhapbyMaPN(string maPN)
        {
            maPN = AES.EncryptAES(maPN, newKeyAES);
            DataTable tempPN = ctpnDAL.getListPhieuNhapbyMaPN(maPN);
            if (tempPN != null && tempPN.Rows.Count > 0)
            {
                DataTable dtPN = tempPN.Clone();
                dtPN = tempPN.AsEnumerable()
                    .Select(row => dtPN.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaSP"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenSP"), newKeyAES),
                        row.Field<int>("SoLuong"),
                        row.Field<int>("DonGiaNhap"),
                        row.Field<int>("ThanhTien"),
                    }, false))
                    .CopyToDataTable();
                dtPN.DefaultView.Sort = "MaSP ASC";
                return dtPN;
            }
            else
            {
                return tempPN;
            }
            //return ctpnDAL.getListPhieuNhapbyMaPN(maPN);
        }

        public DataTable getListChiTietPhieuNhap(string maPN)
        {
            maPN = AES.EncryptAES(maPN, newKeyAES);
            DataTable tempPN = ctpnDAL.getListChiTietPhieuNhap(maPN);
            if (tempPN != null && tempPN.Rows.Count > 0)
            {
                DataTable dtPN = tempPN.Clone();
                dtPN = tempPN.AsEnumerable()
                    .Select(row => dtPN.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaPN"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaSP"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenSP"), newKeyAES),
                        row.Field<int>("SoLuong"),
                        row.Field<int>("DonGiaNhap"),
                        row.Field<int>("ThanhTien"),
                    }, false))
                    .CopyToDataTable();
                dtPN.DefaultView.Sort = "MaPN ASC";
                return dtPN;
            }
            else
            {
                return tempPN;
            }
            //return ctpnDAL.getListChiTietPhieuNhap(maPN);
        }
    }
}
