﻿using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MiniChonKMGUI : Form
    {
        private KhuyenMaiBLL kmBLL;
        private CTKhuyenMaiBLL ctkmBLL;
        private int trangThaiKM;
        private int tongTienTT;
        private int dieuKienKM;
        private DataTable dtKhuyenMai;
        private DataTable dtCTKM;
        private string maKM1;
        private int phanTramKM1;
        // Khai báo biến để lưu trữ RowIndex
        public List<CTKhuyenMaiDTO> listCTKMinFormMini { get; set; }
        public List<string> MaKMinCTKMList { get; set; }
        public List<string> MaSPinCTKMList { get; set; }
        public List<int> PhantramKMinCTKMList { get; set; }
        public List<int> TrangThaiinCTKMList { get; set; }


        public string MaKM1 { get => maKM1; set => maKM1 = value; }
        public int PhanTramKM1 { get => phanTramKM1; set => phanTramKM1 = value; }

        public MiniChonKMGUI(int tongTienTT)
        {
            InitializeComponent();
            kmBLL = new KhuyenMaiBLL();
            ctkmBLL = new CTKhuyenMaiBLL();
            dtKhuyenMai = kmBLL.getListDsKm();
            dgvKhuyenMai.DataSource = dtKhuyenMai;
            this.tongTienTT = tongTienTT;

            MaKMinCTKMList = new List<string>();
            MaSPinCTKMList = new List<string>();
            PhantramKMinCTKMList = new List<int>();
            TrangThaiinCTKMList = new List<int>();

            listCTKMinFormMini = new List<CTKhuyenMaiDTO>();
        }

        public MiniChonKMGUI() : this(0) // Gọi hàm khởi tạo có tham số với giá trị mặc định 0
        {
            // Mã khởi tạo không có tham số
        }

        private void MiniChonKMGUI_Load(object sender, EventArgs e)
        {


        }



        private void dgvKhuyenMai_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvKhuyenMai.Columns["TrangThai"].Index && e.Value != null)
            {
                int trangThai = Convert.ToInt32(e.Value); // Chuyển đổi giá trị ô thành số nguyên

                // Lấy giá trị cột "NgayKetThuc"
                DateTime ngayKetThuc = Convert.ToDateTime(dgvKhuyenMai.Rows[e.RowIndex].Cells["NgayKetThuc"].Value);
                DateTime ngayBatDau = Convert.ToDateTime(dgvKhuyenMai.Rows[e.RowIndex].Cells["NgayBatDau"].Value);

                // Định dạng giá trị dựa trên giá trị của cột "TrangThai" và ngày kết thúc
                if (trangThai == 1 && ngayKetThuc >= DateTime.Now && ngayBatDau <= DateTime.Now)
                {
                    e.Value = "Hoạt động";
                }
                else
                {
                    
                    e.Value = "Không hoạt động";
                }


                
            }
            
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            listCTKMinFormMini = new List<CTKhuyenMaiDTO>();
            MaKM1 = "";
            this.Close();

        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            int i = dgvKhuyenMai.CurrentRow.Index;
            Console.WriteLine(dgvKhuyenMai.Rows[i].Cells[6].Value.ToString());
            Console.WriteLine(trangThaiKM);
            if (trangThaiKM == 0 || dgvKhuyenMai.Rows[i].Cells[6].Value.ToString() == "Không hoạt động")
            {
                Console.WriteLine("Đâu có đủ đâu");
                MessageBox.Show("Khuyến mãi không khả dụng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tongTienTT < dieuKienKM)
            {
                MessageBox.Show("Chưa đủ điều kiện tham gia khuyến mãi", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MaKM1 = txtMaKM.Texts;
            PhanTramKM1 = phanTramKM1;
            if (dieuKienKM == 0 && phanTramKM1 == 0)
            {
                dtCTKM = ctkmBLL.getListCTKM(MaKM1);
                List<CTKhuyenMaiDTO> listCTKM = dtCTKM.AsEnumerable()
                        .Select(row => new CTKhuyenMaiDTO
                        {
                            Makm = row.Field<string>("MaKm"),
                            Masp = row.Field<string>("MaSp"),
                            PhanTramKm = row.Field<int>("PhanTramKm"),
                            TrangThai = row.Field<int>("TrangThai")
                        })
                        .ToList();
                foreach (var ctkm in listCTKM)
                {
                    MaKMinCTKMList.Add(ctkm.Makm);
                    MaSPinCTKMList.Add(ctkm.Masp);
                    PhantramKMinCTKMList.Add(ctkm.PhanTramKm);
                    TrangThaiinCTKMList.Add(ctkm.TrangThai);


                }
                listCTKMinFormMini = listCTKM;
            }
            else
            {
                listCTKMinFormMini = new List<CTKhuyenMaiDTO>();
            }

            this.Close();

        }

        private void dgvKhuyenMai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvKhuyenMai.CurrentRow.Index;
            txtMaKM.Texts = dgvKhuyenMai.Rows[i].Cells[0].Value.ToString();
            txtTenKM.Texts = dgvKhuyenMai.Rows[i].Cells[1].Value.ToString();
            // Lấy giá trị đã định dạng từ sự kiện CellFormatting

            string formattedTrangThai = dgvKhuyenMai.Rows[i].Cells[6].FormattedValue.ToString();
            if (formattedTrangThai == "Không hoạt động")
            {
                trangThaiKM = 0;
            }
            else
            {
                // Nếu giá trị đã định dạng không là "Không hoạt động", lấy giá trị từ cột "TrangThai" trong dữ liệu
                trangThaiKM = int.Parse(dgvKhuyenMai.Rows[i].Cells[6].Value.ToString());
            }
            dieuKienKM = int.Parse(dgvKhuyenMai.Rows[i].Cells[5].Value.ToString());
            phanTramKM1 = int.Parse(dgvKhuyenMai.Rows[i].Cells[4].Value.ToString());
            
        }

        private void btnKhongApDungKM_Click(object sender, EventArgs e)
        {
            MaKM1 = "Không KM";
            PhanTramKM1 = 0;
            listCTKMinFormMini = new List<CTKhuyenMaiDTO>();

            this.Close();
        }
    }
}
