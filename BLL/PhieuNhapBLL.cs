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
    public class PhieuNhapBLL
    {
        private PhieuNhapDAL pnDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public PhieuNhapBLL()
        {
            pnDAL = new PhieuNhapDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }

        public bool insertPhieuNhap(PhieuNhapDTO pn)
        {
            pn.MaPN = AES.EncryptAES(pn.MaPN, newKeyAES);
            pn.MaNV = AES.EncryptAES(pn.MaNV, newKeyAES);
            pn.MaNCC = AES.EncryptAES(pn.MaNCC, newKeyAES);

            return pnDAL.insertPhieuNhap(pn);
        }
        public DataTable getListDsPhieuNhap()
        {
            DataTable tempPN = pnDAL.getListPhieuNhap();
            if (tempPN != null && tempPN.Rows.Count > 0)
            {
                DataTable dtPN = tempPN.Clone();
                dtPN = tempPN.AsEnumerable()
                    .Select(row => dtPN.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaPN"), newKeyAES),
                        row.Field<DateTime>("NgayLap"),
                        
                        row.Field<int>("TongTien"),
                        AES.DecryptAES(row.Field<string>("Ten"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenNCC"), newKeyAES),
                        
                    }, false))
                    .CopyToDataTable();
                dtPN.DefaultView.Sort = "MaPN ASC";
                return dtPN;
            }
            else
            {
                return tempPN;
            }
            //return pnDAL.getListPhieuNhap();
        }
        //public string getLastMaPN()
        //{
        //    string lastMaPN = pnDAL.getLastMaPN();
        //    if (!string.IsNullOrEmpty(lastMaPN))
        //    {
        //        string tempLastNumber = lastMaPN.Substring(2);
        //        if(int.TryParse(tempLastNumber, out int lastNumber))
        //        {
        //            lastNumber++;
        //            string nextNumber = lastNumber.ToString("D3");
        //            string nextMaPN = "#PN" + nextNumber;
        //            return nextMaPN;
        //        }
        //    }
        //    return null;
        //}
    }
}
