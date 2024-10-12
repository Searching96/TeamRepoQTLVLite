using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
namespace LMS
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void KeyDown_ESC(object sender, KeyEventArgs e)
        {
            // Kiểm tra nếu phím Esc được nhấn
            if (e.Key == Key.Escape)
            {
                this.Close(); // Đóng cửa sổ
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
            {
                SqlConnection sqlCon = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True");
                try
                {
                    
                    sqlCon.Open();
                    String query1 = "SELECT ROLE FROM NGUOIDUNG WHERE Username = @Username AND Password = @Password";
                    
                    SqlCommand sqlCmd = new SqlCommand(query1, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);
                    object roleObj = sqlCmd.ExecuteScalar();
                    sqlCon.Close();
                    if (roleObj != null)
                    {
                        string role = roleObj.ToString(); // Lấy vai trò từ kết quả truy vấn
                        sqlCon.Open();
                        String query2 = "UPDATE NGUOIDUNG SET Status = @status WHERE Username = @Username AND Password = @Password" ;

                        sqlCmd = new SqlCommand(query2, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);
                        sqlCmd.Parameters.AddWithValue("@status", 1);
                        sqlCmd.ExecuteNonQuery();
                    // Kiểm tra role để mở cửa sổ tương ứng
                         if (role == "Admin")
                        {
                            // Mở cửa sổ dành cho Admin
                            MainWindow mw = new MainWindow(txtUsername.Text, txtPassword.Password);
                            mw.Show();
                            
                        }
                        else if (role == "User")
                        {
                            // Mở cửa sổ dành cho User
                            UserWindow uw = new UserWindow(txtUsername.Text, txtPassword.Password);
                            uw.Show();
                        }

                        // Đóng màn hình login
                        this.Close();
                    }
                    else
                    {
                        // Nếu không tìm thấy tài khoản, hiện thông báo lỗi
                        MessageBox.Show("Username or Password is incorrect.");
                        txtUsername.Text = "";
                        txtPassword.Password = "";
                        txtUsername.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                { sqlCon.Close(); }
            }
        }
    
}
