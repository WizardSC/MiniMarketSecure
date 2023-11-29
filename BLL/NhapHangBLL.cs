using BLL.Crypto;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NhapHangBLL
    {
        public NhapHangDAL nhDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public NhapHangBLL()
        {
            nhDAL = new NhapHangDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }

        public DataTable getListNhapHang()
        {
            int trangThai = XOR.EncryptXOR(1, keyXOR);
            DataTable tempSP = nhDAL.getListSPToNhapHang(trangThai);
            if (tempSP != null && tempSP.Rows.Count > 0)
            {
                DataTable dtSP = tempSP.Clone();
                dtSP = tempSP.AsEnumerable()
                    .Select(row => dtSP.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaSP"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenSP"), newKeyAES),
                        row.Field<int>("SoLuong"),
                        row.Field<int>("DonGiaNhap"),
                        AES.DecryptAES(row.Field<string>("TenLoai"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenNCC"), newKeyAES),
                        // Thêm các trường cần giải mã khác nếu có
                    }, false))
                    .CopyToDataTable();
                //dtSP.DefaultView.Sort = "MaHD ASC";

                return dtSP;
            }
            else
            {
                return tempSP;
            }
            //return nhDAL.getListSPToNhapHang();
        }

    }
}
