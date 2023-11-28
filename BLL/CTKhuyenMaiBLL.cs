using BLL.Crypto;
using DAL;
using DTO;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class CTKhuyenMaiBLL
    {
        private CTKhuyenMaiDAL CtKmDAL;
      

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;



        public CTKhuyenMaiBLL()
        {
            CtKmDAL = new CTKhuyenMaiDAL();
            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }

        public DataTable getListCTKM(string maKM)
        {
            maKM = AES.EncryptAES(maKM, newKeyAES);
            DataTable tempCTKM = CtKmDAL.getListCTKM(maKM);
            if (tempCTKM != null && tempCTKM.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtKM = tempCTKM.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtKM = tempCTKM.AsEnumerable()
                    .Select(row => dtKM.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaKM"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaSP"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("PhanTramKM"),keyXOR),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                    }, false))
                    .CopyToDataTable();
                dtKM.DefaultView.Sort = "MaKM ASC";

                return dtKM;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempCTKM;
            }
            //return CtKmDAL.getListCTKM(maKM);
        }
        public DataTable getListDsCTKm()
        {
            DataTable tempCTKM = CtKmDAL.getListChiTietKhuyenMai();
            if (tempCTKM != null && tempCTKM.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtKM = tempCTKM.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtKM = tempCTKM.AsEnumerable()
                    .Select(row => dtKM.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaKM"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaSP"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenKM"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenSP"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("PhanTramKM"),keyXOR),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                    }, false))
                    .CopyToDataTable();
                dtKM.DefaultView.Sort = "MaKM ASC";

                return dtKM;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempCTKM;
            }
            //return CtKmDAL.getListChiTietKhuyenMai();
        }

        public bool insertCTKhuyenMai(CTKhuyenMaiDTO ctkm)
        {
            ctkm.Makm = AES.EncryptAES(ctkm.Makm, newKeyAES);
            ctkm.Masp = AES.EncryptAES(ctkm.Masp, newKeyAES);
            ctkm.PhanTramKm = XOR.EncryptXOR(ctkm.PhanTramKm, keyXOR);
            ctkm.TrangThai = XOR.EncryptXOR(ctkm.TrangThai, keyXOR);


            return CtKmDAL.insert_CTKhuyenMai(ctkm);
        }
        public bool UpdateCTKhuyenMai(CTKhuyenMaiDTO ctkm)
        {
            ctkm.Makm = AES.EncryptAES(ctkm.Makm, newKeyAES);
            ctkm.Masp = AES.EncryptAES(ctkm.Masp, newKeyAES);
            ctkm.PhanTramKm = XOR.EncryptXOR(ctkm.PhanTramKm, keyXOR);
            ctkm.TrangThai = XOR.EncryptXOR(ctkm.TrangThai, keyXOR);
            return CtKmDAL.Update_CTKhuyenMai(ctkm);
        }
        
    }
}
