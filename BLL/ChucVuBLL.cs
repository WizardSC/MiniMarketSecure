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
    public class ChucVuBLL
    {
        private ChucVuDAL cvDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public ChucVuBLL()
        {
            cvDAL = new ChucVuDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public DataTable getListChucVu()
        {
            DataTable tempCV = cvDAL.getListChucVu();
            if (tempCV != null && tempCV.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtCV = tempCV.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtCV = tempCV.AsEnumerable()
                    .Select(row => dtCV.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaCV"),
                        AES.DecryptAES(row.Field<string>("MaCV"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenCV"), newKeyAES),                       
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                    }, false))
                    .CopyToDataTable();
                dtCV.DefaultView.Sort = "MaCV ASC";

                return dtCV;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempCV;
            }
            //return cvDAL.getListChucVu();
        }
        public bool insertCV(ChucVuDTO cv)
        {
            cv.MaCV = AES.EncryptAES(cv.MaCV, newKeyAES);
            cv.TenCV = AES.EncryptAES(cv.TenCV, newKeyAES);
            cv.TrangThai = XOR.EncryptXOR(cv.TrangThai, keyXOR);
            return cvDAL.insertChucVu(cv);
        }
        public bool updateCV(ChucVuDTO cv)
        {
            cv.MaCV = AES.EncryptAES(cv.MaCV, newKeyAES);
            cv.TenCV = AES.EncryptAES(cv.TenCV, newKeyAES);
            cv.TrangThai = XOR.EncryptXOR(cv.TrangThai, keyXOR);
            return cvDAL.updateChucVu(cv);
        }
        public bool deleteCV(string maCV, out bool isLoiKhoaNgoai)
        {
            maCV = AES.EncryptAES(maCV, newKeyAES);

            return cvDAL.deleteChucVu(maCV, out isLoiKhoaNgoai);
        }
        
        public bool updateTrangThai(int trangThai, string maCV)
        {
            maCV = AES.EncryptAES(maCV, newKeyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);
            trangThai = (trangThai == 0) ? 1 : 0;
            return cvDAL.updateTrangThai(trangThai, maCV);
        }
    }
}
