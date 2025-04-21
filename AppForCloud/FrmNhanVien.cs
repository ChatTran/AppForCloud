using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppForCloud
{
    public partial class FrmNhanVien: Form
    {
        string connectionString = "Data Source=CHAT;Initial Catalog=QuanLiNV;Integrated Security=True";
        private string role;

        public FrmNhanVien(string quyenDangNhap)
        {
            InitializeComponent();
            this.role = quyenDangNhap;
        }
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM NhanVien";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvNhanVien.DataSource = dt;
            }
        }

        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        { 
            LoadData();

            if (role == "nhanvien")
            {
                btThem.Visible = false;
                btSua.Visible = false;
                btXoa.Visible = false;
            }
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadPhongBanTheoMaNV(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT TenPB 
                                 FROM PhongBan pb 
                                 JOIN NhanVien nv ON pb.MaPB = nv.MaPB 
                                 WHERE nv.MaNV = @MaNV";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                if (result != null)
                    txtTenPhongBan.Text = result.ToString(); // txtTenPhongBan là TextBox bạn thêm để hiển thị phòng ban
                else
                    txtTenPhongBan.Text = "Không xác định";
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO NhanVien (MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaPB) " +
                               "VALUES (@MaNV, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email, @MaPB)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text.Trim());
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", DateTime.Parse(txtNgaySinh.Text));
                cmd.Parameters.AddWithValue("@GioiTinh", txtGioiTinh.Text.Trim());
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@MaPB", txtMaPB.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Thêm nhân viên thành công!");
                LoadData();
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                txtMaNV.Text = row.Cells["MaNV"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtNgaySinh.Text = Convert.ToDateTime(row.Cells["NgaySinh"].Value).ToString("yyyy-MM-dd");
                txtGioiTinh.Text = row.Cells["GioiTinh"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();

                LoadPhongBanTheoMaNV(txtMaNV.Text.Trim());
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE NhanVien SET HoTen=@HoTen, NgaySinh=@NgaySinh, GioiTinh=@GioiTinh, " +
                               "DiaChi=@DiaChi, SoDienThoai=@SoDienThoai, Email=@Email, MaPB=@MaPB WHERE MaNV=@MaNV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text.Trim());
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", DateTime.Parse(txtNgaySinh.Text));
                cmd.Parameters.AddWithValue("@GioiTinh", txtGioiTinh.Text.Trim());
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@MaPB", txtMaPB.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Cập nhật thông tin thành công!");
                LoadData();
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM NhanVien WHERE MaNV=@MaNV";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Xóa nhân viên thành công!");
                    LoadData();
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btTimKiem_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT TOP 1 pb.TenPB
            FROM NhanVien nv
            JOIN PhongBan pb ON nv.MaPB = pb.MaPB
            WHERE nv.MaNV = @MaNV OR nv.HoTen LIKE @HoTen";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@HoTen", "%" + hoTen + "%");

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                if (result != null)
                {
                    txtTenPhongBan.Text = result.ToString(); // Hiển thị tên phòng ban
                }
                else
                {
                    txtTenPhongBan.Text = "Không tìm thấy";
                }
            }
        }

        private void lkExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                FrmLogin frmLG = new FrmLogin();
                frmLG.Show();
            }
        }

    }
}
