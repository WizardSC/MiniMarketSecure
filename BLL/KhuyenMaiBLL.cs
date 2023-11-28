using BLL.Crypto;
using DAL;
using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class KhuyenMaiBLL
    {

        private KhuyenMaiDAL kmDAL;
        private List<KhuyenMaiDTO> listKhuyenMai = null;

        private int keyXOR;
        private string keyAES;
        private string newKeyAES;
        private string keyVigenere;

        public KhuyenMaiBLL()
        {
            kmDAL = new KhuyenMaiDAL();

            keyAES = "MUAXUANDENABCDEF";
            keyXOR = 33;
            keyVigenere = "GIANGSINH";
            newKeyAES = (encryptKeyAES(keyAES));
        }
        public string encryptKeyAES(string keyAES)
        {
            return Vigenere.EncryptVigenere(keyAES, keyVigenere);
        }

        public DataTable getListDsKm()
        {
            DataTable tempKM = kmDAL.getListKhuyenMai();
            if (tempKM != null && tempKM.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtKM = tempKM.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtKM = tempKM.AsEnumerable()
                    .Select(row => dtKM.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaKM"),
                        AES.DecryptAES(row.Field<string>("MaKM"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenKM"), newKeyAES),
                        row.Field<DateTime>("NgayBatDau"),
                        row.Field<DateTime>("NgayKetThuc"),
                        XOR.DecryptXOR(row.Field<int>("PhanTramKM"),keyXOR),
                        AES.DecryptAES(row.Field<string>("DieuKienKM"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),


                    }, false))
                    .CopyToDataTable();
                dtKM.DefaultView.Sort = "MaKM ASC";

                return dtKM;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempKM;
            }

            //return kmDAL.getListKhuyenMai();
        }

        public DataTable getThongTinSPKM(string MaKM)
        {
            MaKM = AES.EncryptAES(MaKM, newKeyAES);
            DataTable tempKM = kmDAL.getThongTinKM(MaKM);
            if (tempKM != null && tempKM.Rows.Count > 0)
            {
                DataTable dtKM = tempKM.Clone();
                dtKM = tempKM.AsEnumerable()
                    .Select(row => dtKM.LoadDataRow(new object[]
                    {               
                        AES.DecryptAES(row.Field<string>("MaKM"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("MaSP"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenSP"), newKeyAES),
                        row.Field<DateTime>("NgayBatDau"),
                        row.Field<DateTime>("NgayKetThuc"),
                        XOR.DecryptXOR(row.Field<int>("PhanTramKM"),keyXOR),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),
                    }, false))
                    .CopyToDataTable();
                dtKM.DefaultView.Sort = "MaKM ASC";

                return dtKM;
            }
            else
            {
                return tempKM;
            }
            //return kmDAL.getThongTinKM(MaKM);
        }
        public DataTable getListMaKmNoDK()
        {
            int phanTramKM = XOR.EncryptXOR(0, keyXOR);
            string dieuKienKM = AES.EncryptAES("0", newKeyAES);
            DataTable tempKM = kmDAL.getMaKmNoDK(dieuKienKM, phanTramKM);
            if (tempKM != null && tempKM.Rows.Count > 0)
            {
                // Tạo DataTable mới với cấu trúc tương tự DataTable gốc
                DataTable dtKM = tempKM.Clone();
                // Sử dụng LINQ để thêm dữ liệu vào DataTable mới
                dtKM = tempKM.AsEnumerable()
                    .Select(row => dtKM.LoadDataRow(new object[]
                    {
                        //row.Field<string>("MaKM"),
                        AES.DecryptAES(row.Field<string>("MaKM"), newKeyAES),
                        AES.DecryptAES(row.Field<string>("TenKM"), newKeyAES),
                        row.Field<DateTime>("NgayBatDau"),
                        row.Field<DateTime>("NgayKetThuc"),
                        XOR.DecryptXOR(row.Field<int>("PhanTramKM"),keyXOR),
                        AES.DecryptAES(row.Field<string>("DieuKienKM"), newKeyAES),
                        XOR.DecryptXOR(row.Field<int>("TrangThai"),keyXOR),


                    }, false))
                    .CopyToDataTable();
                dtKM.DefaultView.Sort = "MaKM ASC";

                return dtKM;
            }
            else
            {
                // Nếu DataTable gốc trống, trả về null hoặc DataTable trống
                return tempKM;
            }

            //return kmDAL.getMaKmNoDK();
        }
        
        public bool insertKhuyenMai(KhuyenMaiDTO km)
        {
            km.Makm = AES.EncryptAES(km.Makm, newKeyAES);
            km.TenKm = AES.EncryptAES(km.TenKm, newKeyAES);
            km.PhanTramKm = XOR.EncryptXOR(km.PhanTramKm, keyXOR);
            km.DieuKiemKm = AES.EncryptAES(km.DieuKiemKm, newKeyAES);
            km.TrangThai = XOR.EncryptXOR(km.TrangThai, keyXOR);
            return kmDAL.insert_KhuyenMai(km);
        }
        public bool UpdateKhuyenMai(KhuyenMaiDTO km)
        {
            km.Makm = AES.EncryptAES(km.Makm, newKeyAES);

            km.TenKm = AES.EncryptAES(km.TenKm, newKeyAES);
            km.PhanTramKm = XOR.EncryptXOR(km.PhanTramKm, keyXOR);
            km.DieuKiemKm = AES.EncryptAES(km.DieuKiemKm, newKeyAES);
            km.TrangThai = XOR.EncryptXOR(km.TrangThai, keyXOR);

            return kmDAL.Update_KhuyenMai(km);
        }
        public bool deleteKhuyenMai(string maKM, out bool isLoiKhoaNgoai)
        {
            maKM = AES.EncryptAES(maKM, newKeyAES);

            return kmDAL.delete_khuyenMai(maKM, out isLoiKhoaNgoai);
        }
        public bool updateTrangThai(int trangThai, string maKM)
        {
            maKM = AES.EncryptAES(maKM, newKeyAES);
            trangThai = XOR.EncryptXOR(trangThai, keyXOR);

            trangThai = (trangThai == 0) ? 1 : 0;
            return kmDAL.update_TrangThai(trangThai, maKM);
        }
    }
}
