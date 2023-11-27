using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Crypto;
using DAL;
using DTO;
namespace BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL khDAL;
        private List<KhachHangDTO> listKhacHang = null;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string oldKeyAES;
        private string keyVigenere;

        public KhachHangBLL()
        {
            khDAL = new KhachHangDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));

        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }



        public DataTable getListKhachHang()
        {
            DataTable tempKH = khDAL.getListKhachHang();
            if (tempKH != null && tempKH.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNCC = tempKH.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtNCC = tempKH.AsEnumerable()
                    .Select(row => dtNCC.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaNCC"),

                        AES.DecryptAES(row.Field<string>("MaKH"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ho"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ten"), newKeyAES),
                        row.Field<DateTime>("NgaySinh"),
                        AES.DecryptAES(row.Field<string>("GioiTinh"), newKeyAES),

                        AES.DecryptAES(row.Field<string>("SoDT"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("DiaChi"), newKeyAES),

                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                        XOR.DecryptXOR(row.Field<int>("DiemTichLuy"),keyXOR),

                        row.Field<string>("IMG"),


                    }, false))
                    .CopyToDataTable();
                dtNCC.DefaultView.Sort = "MaKH ASC";

                return dtNCC;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempKH;
            }
            //return khDAL.getListKhachHang();
        }

        public DataTable getMiniListKhachHang()
        {
            int trangThai = XOR.EncryptXOR(1, keyXOR);

            DataTable tempKH = khDAL.getMiniListKhachHang(trangThai);
            if (tempKH != null && tempKH.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtNCC = tempKH.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtNCC = tempKH.AsEnumerable()
                    .Select(row => dtNCC.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaNCC"),

                        AES.DecryptAES(row.Field<string>("MaKH"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ho"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("Ten"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("DiemTichLuy"),keyXOR),
                    }, false))
                    .CopyToDataTable();
                dtNCC.DefaultView.Sort = "MaKH ASC";

                return dtNCC;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempKH;
            }
            //return khDAL.getMiniListKhachHang();
        }
        public bool insertKhachhang(KhachHangDTO kh)
        {
            kh.MaKH = AES.EncryptAES(kh.MaKH, newKeyAES);
            kh.Ho = AES.EncryptAES(kh.Ho, newKeyAES);
            kh.Ten = AES.EncryptAES(kh.Ten, newKeyAES);
            //kh.NgaySinh = AES.EncryptAES(kh.NgaySinh.ToStr, newKeyAES);
            kh.GioiTinh = AES.EncryptAES(kh.GioiTinh, newKeyAES);
            kh.DiaChi = AES.EncryptAES(kh.DiaChi, newKeyAES);
            kh.SoDT = AES.EncryptAES(kh.SoDT, newKeyAES);
            kh.TrangThai = XOR.EncryptXOR(kh.TrangThai, keyXOR);
            kh.DiemTichLuy = XOR.EncryptXOR(kh.DiemTichLuy, keyXOR);


            return khDAL.insertKhachHang(kh);

        }

        public bool updateKhachHang(KhachHangDTO kh)
        {
            kh.MaKH = AES.EncryptAES(kh.MaKH, newKeyAES);
            kh.Ho = AES.EncryptAES(kh.Ho, newKeyAES);
            kh.Ten = AES.EncryptAES(kh.Ten, newKeyAES);
            //kh.NgaySinh = AES.EncryptAES(kh.NgaySinh.ToStr, newKeyAES);
            kh.GioiTinh = AES.EncryptAES(kh.GioiTinh, newKeyAES);
            kh.DiaChi = AES.EncryptAES(kh.DiaChi, newKeyAES);
            kh.SoDT = AES.EncryptAES(kh.SoDT, newKeyAES);
            kh.TrangThai = XOR.EncryptXOR(kh.TrangThai, keyXOR);
            kh.DiemTichLuy = XOR.EncryptXOR(kh.DiemTichLuy, keyXOR);

            return khDAL.updateKhachHang(kh);
        }

        public bool updateDiemTL(string maKH, int diemTL)
        {
            maKH = AES.EncryptAES(maKH, newKeyAES);
            diemTL = XOR.EncryptXOR(diemTL, keyXOR);

            return khDAL.updateDiemTichLuy(maKH, diemTL);
        }
        public bool deleteKhachHang(string maKH, out bool isLoiKhoaNgoai)
        {
            maKH = AES.EncryptAES(maKH, newKeyAES);
            return khDAL.deleteKhachHang(maKH, out isLoiKhoaNgoai);
        }
    }
}
