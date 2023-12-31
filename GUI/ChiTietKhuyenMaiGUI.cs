﻿using BLL;
using DTO;
using GUI.MyCustom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace GUI
{
    public partial class ChiTietKhuyenMaiGUI : Form
    {
        DataTable data = new DataTable();
        DataTable dataListMaKmNoDK = new DataTable();
        DataTable datagetListSanPham = new DataTable();
        CTKhuyenMaiBLL CTKhuyenMaiBLL = new CTKhuyenMaiBLL();
        KhuyenMaiBLL KmBLL = new KhuyenMaiBLL();
        SanPhamBLL SpBLL = new SanPhamBLL();
        private string cbxItemsMacDinh;
        private string currentSearch;
        private string textSearchCondition = ""; // Biến để lưu trữ điều kiện từ textbox tìm kiếm

        public ChiTietKhuyenMaiGUI()
        {
            InitializeComponent();
          
            loadDataToCBX(cbxTimKiem);
            dataListMaKmNoDK = KmBLL.getListMaKmNoDK();
            datagetListSanPham = SpBLL.getListSanPham();
            data = CTKhuyenMaiBLL.getListDsCTKm();
            loadsItemTenKM();
            loadsItemTenSp();
            hideError();


        }
        //load tenkm
        public void loadsItemTenKM()
        {
            comboBoxTenKM.DataSource = dataListMaKmNoDK;
            comboBoxTenKM.DisplayMember = "TenKM";
            comboBoxTenKM.SelectedIndex = -1;

        }
        //load tensp
        public void loadsItemTenSp()
        {
            comboBoxTenSP.DataSource = datagetListSanPham;
            comboBoxTenSP.DisplayMember = "TenSP";
            comboBoxTenSP.SelectedIndex = -1;

        }
        //load combobox timkiem
        private void loadDataToCBX(RJComboBox cbx)
        {
            cbx.Items.Add("Mã KM");
            cbx.Items.Add("Mã SP");
            cbx.Items.Add("Tên KM");
            cbx.Items.Add("Tên SP");
            cbx.Items.Add("Phần Trăm KM");
            cbxTimKiem.SelectedIndex = 0;
        }

        private string returnDieuKien(string text)
        {
            return text;
        }
        private string GetTextSearchCondition(string searchText)
        {
            switch (cbxItemsMacDinh)
            {
                case "Mã KM":
                    return returnDieuKien($"MaKM like '%{searchText}%'");
                case "Mã SP":
                    return returnDieuKien($"MaSP like '%{searchText}%'");
                case "Tên KM":
                    return returnDieuKien($"TenKM like '%{searchText}%'");
                case "Tên SP":
                    return returnDieuKien($"TenSP like '%{searchText}%'");
                case "Phần Trăm KM":
                    int phanTramKM;
                    if (int.TryParse(searchText, out phanTramKM))
                    {
                        return returnDieuKien($"PhanTramKM = {phanTramKM}");
                    }
                    else
                    {
                        return "";
                    }
                default:
                    return "";
            }
        }
        private string CombineConditions(string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                return $"({condition})";
            }
            else
            {
                return "";
            }
        }

        private void applySearchs(string text)
        {
            currentSearch = text;
            Console.WriteLine(currentSearch);
            DataView dvCTKhuyenMai = data.DefaultView;
            dvCTKhuyenMai.RowFilter = currentSearch;
            dgvChiTietKM.DataSource = dvCTKhuyenMai.ToTable();
        }

        
        public void init()
        {
            dgvChiTietKM.DataSource = CTKhuyenMaiBLL.getListDsCTKm();

            cbxTrangThai.SelectedIndex=0;
            // Gán sự kiện CellFormatting
            dgvChiTietKM.CellFormatting += dgvChiTietKM_CellFormatting;
        }

        public void clearForm()
        {
            txtPhanTramKM.Texts = " ";
            comboBoxTenKM.SelectedIndex = -1;
            comboBoxTenSP.SelectedIndex = -1;
            cbxTrangThai.SelectedIndex = 0;
            btnThemCTKM.Enabled = true;
            btnUpdateKM.Enabled = false;
            comboBoxTenKM.Enabled = true;
            comboBoxTenSP.Enabled = true;
            hideError();
        }

        private void ChiTietKhuyenMaiGUI_Load(object sender, EventArgs e)
        {
            init();
            btnThemCTKM.Enabled = true;
            btnUpdateKM.Enabled = false;
           
        }

        private void hideError()
        {
            label10.ForeColor = Color.Transparent;
        }

        private void cbxTimKiem_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            cbxItemsMacDinh = cbxTimKiem.SelectedItem.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string textTimKiem = txtTimKiem.Texts;
            textSearchCondition = GetTextSearchCondition(textTimKiem);
            string combinedCondition = CombineConditions(textSearchCondition);
            applySearchs(combinedCondition);
        }

        private void txtTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTimKiem.PerformClick();
                e.Handled = true;
            }
        }


        private string CheckAndSetColor(object control, Label label)
        {
             if (control is System.Windows.Forms.ComboBox comboBox)
            {
                string selectedValue = comboBox.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(selectedValue))
                {
                    label.ForeColor = Color.FromArgb(230, 76, 89);
                }
                else
                {
                    label.ForeColor = Color.Transparent;
                }
                return selectedValue;
            }

            return null; // Nếu kiểu dữ liệu không hợp lệ.
        }

        // check phantramkm nhập int
        private const int MaxPercentage = 99;
        private string CheckAndSetColorPhanTramKM(object control, Label label)
        {
            if (control is RJTextBox textBox)
            {
                string text = textBox.Texts.Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    label.ForeColor = Color.FromArgb(230, 76, 89);
                    label.Text = "* Bạn phải nhập % KM";
                }
                else if (!int.TryParse(text, out int result))
                {
                    label.ForeColor = Color.FromArgb(230, 76, 89);
                    label.Text = "* Bạn phải nhập số nguyên";
                }
                else if (!int.TryParse(text, out int percentage) || percentage > MaxPercentage)
                {
                    label.ForeColor = Color.FromArgb(230, 76, 89);
                    label.Text = "* Bạn phải nhập bé hơn 100";
                }
                else
                {
                    label.ForeColor = Color.Transparent;
                    label.Text = "";
                }
                return text;
            }
            return null; // Nếu kiểu dữ liệu không hợp lệ.
        }
        private bool IsInteger(string text)
        {
            int result;
            return int.TryParse(text, out result);
        }
        private void btnThemCTKM_Click(object sender, EventArgs e)
        {
            string tenKM = comboBoxTenKM.Text;

            string tenSp = comboBoxTenSP.Text;
           
            // Lấy ID khuyen mai từ tên khuyenmai
            string idKm = "KM000"; // Giá trị mặc định nếu không tìm thấy
           
           
            foreach (DataRow row in dataListMaKmNoDK.Rows)
            {
                if (row["TenKM"].ToString() == tenKM)
                {
                    idKm = row["MaKM"].ToString();
                    break; // Thoát vòng lặp khi tìm thấy ID
                }
            }
            string idSp = "SP000";
           
            foreach (DataRow row in datagetListSanPham.Rows)
            {
                if (row["TenSP"].ToString() == tenSp)
                {
                    idSp = row["MaSP"].ToString();
                    break; // Thoát vòng lặp khi tìm thấy ID
                }
            }

            string phantram = CheckAndSetColorPhanTramKM(txtPhanTramKM, label10);
            string CheckTrangThai = cbxTrangThai.Texts.ToString();
            int trangthai = (CheckTrangThai == "Hoạt động") ? 1 : 0;
            if ( string.IsNullOrWhiteSpace(phantram) || !IsInteger(phantram) || string.IsNullOrWhiteSpace(tenKM) || string.IsNullOrWhiteSpace(tenSp) || int.Parse(phantram) > 99)
            {
                return;
            }

            CTKhuyenMaiDTO CTKM_DTO = new CTKhuyenMaiDTO();
            CTKM_DTO.Makm = idKm;
            CTKM_DTO.Masp = idSp;
            CTKM_DTO.PhanTramKm = int.Parse(phantram);
            CTKM_DTO.TrangThai = trangthai;

           
            bool result = CTKhuyenMaiBLL.insertCTKhuyenMai(CTKM_DTO);

            if (result)
            {
                MessageBox.Show("Thêm khuyến mãi cho sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                init();
                clearForm();
            }
            else
            {
                MessageBox.Show("Thêm thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        // đang lỗi update cần check lại database 
        private void btnUpdateKM_Click(object sender, EventArgs e)
        {
            string tenKM = comboBoxTenKM.Text;

            string tenSp = comboBoxTenSP.Text;
            // Lấy ID khuyen mai từ tên khuyenmai
            string idKm = ""; // Giá trị mặc định nếu không tìm thấy

            foreach (DataRow row in dataListMaKmNoDK.Rows)
            {
                if (row["TenKM"].ToString() == tenKM)
                {
                    idKm = row["MaKM"].ToString();
                    break; // Thoát vòng lặp khi tìm thấy ID
                }
            }
            string idSp = "";
            foreach (DataRow row in datagetListSanPham.Rows)
            {
                if (row["TenSP"].ToString() == tenSp)
                {
                    idSp = row["MaSP"].ToString();
                    break; // Thoát vòng lặp khi tìm thấy ID
                }
            }
            string phantram = CheckAndSetColorPhanTramKM(txtPhanTramKM, label10);
            string CheckTrangThai = cbxTrangThai.Texts.ToString();
            int trangthai = (CheckTrangThai == "Hoạt động") ? 1 : 0;
            if (string.IsNullOrWhiteSpace(phantram) || !IsInteger(phantram) || string.IsNullOrWhiteSpace(tenKM) || string.IsNullOrWhiteSpace(tenSp) || int.Parse(phantram) > 99)
            {
                return;
            }
            CTKhuyenMaiDTO CTKM_DTO = new CTKhuyenMaiDTO();
            CTKM_DTO.Makm = idKm;
            CTKM_DTO.Masp = idSp;
            CTKM_DTO.PhanTramKm = int.Parse(phantram);
            CTKM_DTO.TrangThai = trangthai;
            
            try
            {
                CTKhuyenMaiBLL.UpdateCTKhuyenMai(CTKM_DTO);
                MessageBox.Show("Cập nhật khuyến mãi cho sản phẩm thành công!");
                init();
                clearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }

        
       
        }

        private void btnRS_Click(object sender, EventArgs e)
        {
            clearForm();
            cbxTrangThai.SelectedIndex = 0;
            btnThemCTKM.Enabled = true;
            btnUpdateKM.Enabled = false;
        }

        private void dgvChiTietKM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBoxTenKM.Enabled = false;
            comboBoxTenSP.Enabled = false;
            int i = dgvChiTietKM.CurrentRow.Index;
            txtPhanTramKM.Texts = dgvChiTietKM.Rows[i].Cells[4].Value.ToString();
            int TrangThai = int.Parse(dgvChiTietKM.Rows[i].Cells[5].Value.ToString());
            if (TrangThai == 1)
            {
                cbxTrangThai.SelectedIndex = 0;
            }
            else
            {
                cbxTrangThai.SelectedIndex = 1;
            }

            // Lấy giá trị "Ma KM " từ dòng được chọn
            string Tenkm = dgvChiTietKM.Rows[i].Cells[2].Value.ToString();
            // Kiểm tra xem giá trị khuyenmai có tồn tại trong ComboBox không
            int indexKM = comboBoxTenKM.FindStringExact(Tenkm);
            if (indexKM != -1)
            {
                comboBoxTenKM.SelectedIndex = indexKM; // Hiển thị giá trị khuyenmai trong ComboBox

            }

            // Lấy giá trị "Ma SP " từ dòng được chọn
            string Tensp = dgvChiTietKM.Rows[i].Cells[3].Value.ToString();
            // Kiểm tra xem giá trị khuyenmai có tồn tại trong ComboBox không
            int indexSP = comboBoxTenSP.FindStringExact(Tensp);
            if (indexSP != -1)
            {
                comboBoxTenSP.SelectedIndex = indexSP; // Hiển thị giá trị sanpham trong ComboBox

            }
            btnThemCTKM.Enabled = false;
            btnUpdateKM.Enabled = true;
        }

        private void dgvChiTietKM_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5) // Thay 4 bằng chỉ số cột thứ 5 (chú ý: chỉ số cột bắt đầu từ 0).
            {
                if (e.Value != null && e.Value is int)
                {
                    int value = (int)e.Value;
                    if (value == 1)
                    {
                        e.Value = "Hoạt động";
                    }
                    else if (value == 0)
                    {
                        e.Value = "Không hoạt động";
                    }
                    e.FormattingApplied = true;
                }
            }
            if (e.Value != null)
            {
                // Đặt chữ nằm ở giữa cho tất cả các cột
                dgvChiTietKM.Columns[e.ColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void comboBoxTenKM_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAndSetColor(comboBoxTenKM, label13);
        }

        private void txtPhanTramKM__TextChanged(object sender, EventArgs e)
        {
            CheckAndSetColorPhanTramKM(txtPhanTramKM, label10);
        }

        private void comboBoxTenSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAndSetColor(comboBoxTenSP, label4);
        }
    }
    
}
