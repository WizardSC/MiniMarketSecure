using BLL.Crypto;
using DAL;
using DTO;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL
{
    public class TaiKhoanBLL
    {
        private TaiKhoanDAL tkDAL;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;
        private readonly Random random = new Random();
        public TaiKhoanBLL()
        {
            tkDAL = new TaiKhoanDAL();

            keyAES = "MUAXUANDENABCDEF";
            string keyString = "MUAXUANDENABCDEF";
           
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }
        public DataTable getListTaiKhoan()
        {
            DataTable tempTK = tkDAL.getListTaiKhoan();
            if (tempTK != null && tempTK.Rows.Count > 0)
            {
                DataTable dtTK = tempTK.Clone();
                dtTK = tempTK.AsEnumerable()
                    .Select(row => dtTK.LoadDataRow(new object[]
                    {
                        AES.DecryptAES(row.Field<string>("MaTK"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaNV"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenDangNhap"), newKeyAES),
                        row.Field<string>("MatKhau"),
                        AES.DecryptAES(row.Field<string>("Quyen"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                        row.Field<DateTime>("NgayLap"),
                    }, false))
                    .CopyToDataTable();
                dtTK.DefaultView.Sort = "MaTK ASC";
                return dtTK;
            }
            else
            {
                return tempTK;
            }
            //return tkDAL.getListTaiKhoan();
        }
        public bool comparePassword(string inputPassword, string hashedPassword)
        {
            return BCryptFunction.VerifyPassword(inputPassword, hashedPassword);
        }
        public bool insertTaiKhoan(TaiKhoanDTO tk)
        {
            tk.MaTK = AES.EncryptAES(tk.MaTK, newKeyAES);
            tk.MaNV = AES.EncryptAES(tk.MaNV, newKeyAES);
            tk.TenDangNhap = AES.EncryptAES(tk.TenDangNhap, newKeyAES);
            tk.MatKhau = BCryptFunction.HashPassword(tk.MatKhau);
            tk.Quyen = AES.EncryptAES(tk.Quyen, newKeyAES);
            tk.TrangThai = XOR.EncryptXOR(tk.TrangThai, keyXOR);
            Console.WriteLine(tk.MatKhau);

            //return true;
            return tkDAL.insertTaiKhoan(tk);
        }
        

        public bool updateTTCN(string maNV, string tenDangNhap, string matKhau)
        {
            maNV = AES.EncryptAES(maNV, newKeyAES);
            tenDangNhap = AES.EncryptAES(tenDangNhap, newKeyAES);
            matKhau = BCryptFunction.HashPassword(matKhau);
            return tkDAL.updateThongTinCaNhan(maNV, tenDangNhap, matKhau); 
        }

        public bool updateTrangThai(string maNV, int trangThai)
        {
            maNV = AES.EncryptAES(maNV, newKeyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);
            return tkDAL.updateTrangThai(maNV, trangThai);
        }
        //public string getLastMaTK()
        //{
        //    string lastMaTK = tkDAL.getLastMaTK();
        //    if (!string.IsNullOrEmpty(lastMaTK))
        //    {
        //        string tempLastNumber = lastMaTK.Substring(2);
        //        if (int.TryParse(tempLastNumber, out int lastNumber))
        //        {
        //            lastNumber++;
        //            string nextNumber = lastNumber.ToString("D3");
        //            string nextMaTK = "TK" + nextNumber;
        //            return nextMaTK;
        //        }
        //    }
        //    return null;
        //}
        
        public string GenerateRandomPassword()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-=_+[]{}|;:'\",.<>/?";

            // Sử dụng StringBuilder để hiệu quả khi cần thay đổi chuỗi nhiều lần
            StringBuilder password = new StringBuilder();

            // Thêm ít nhất một ký tự đặc biệt
            password.Append(GetRandomChar(specialChars));

            // Thêm ít nhất một số
            password.Append(GetRandomChar(digits));

            // Thêm các ký tự chữ còn lại
            for (int i = 0; i < 10; i++)
            {
                password.Append(GetRandomChar(chars));
            }

            // Trộn ngẫu nhiên các ký tự trong password
            string result = new string(password.ToString().ToCharArray().OrderBy(x => random.Next()).ToArray());

            return result;
        }

        private char GetRandomChar(string charSet)
        {
            int index = random.Next(charSet.Length);
            char selectedChar = charSet[index];

            // Loại bỏ ký tự đã chọn từ tập hợp
            charSet = charSet.Remove(index, 1);

            return selectedChar;
        }
    }
}
