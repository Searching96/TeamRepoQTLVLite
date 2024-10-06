using QLTVLite.Models;
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

namespace QLTVLite
{
    /// <summary>
    /// Interaction logic for WDLogin.xaml
    /// </summary>
    public partial class WDLogin : Window
    {
        public WDLogin()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string tenNguoiDung = txtTenNguoiDung.Text;
            string matKhau = txtMatKhau.Password;

            using (var context = new AppDbContext())
            {
                //var userCount = context.NGUOIDUNG
                    //.Count(u => u.TenNguoiDung == tenNguoiDung && u.MatKhau == matKhau);

                // Display the user count for debugging
                // MessageBox.Show($"Found {userCount} users with the given credentials.", "User Count", MessageBoxButton.OK, MessageBoxImage.Information);
                //MessageBox.Show($"Trying to log in with Username: '{tenNguoiDung}', Password: '{matKhau}'", "Login Attempt", MessageBoxButton.OK, MessageBoxImage.Information);
                // Authenticate user
                var user = context.NGUOIDUNG
                    .FirstOrDefault(u => u.TenNguoiDung == tenNguoiDung && u.MatKhau == matKhau);
                
                if (user != null)
                {
                    // Set the username in the TextBlock
                    // txtUsername.Text = $"Chào, {user.TenNguoiDung}!";

                    // Navigate based on user permissions
                    if (user.PhanQuyen == 1 || user.PhanQuyen == 2)
                    {
                        // Navigate to MainWindow
                        WDTrangQuanLy mainWindow = new WDTrangQuanLy(user.PhanQuyen);
                        mainWindow.Show();
                        this.Close();
                    }
                    else if (user.PhanQuyen == 3)
                    {
                        // Navigate to WDTrangDocGia
                        WDTrangDocGia docGiaWindow = new WDTrangDocGia();
                        docGiaWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    // Show error message
                    txtError.Text = "Tên người dùng hoặc mật khẩu không đúng.";
                }
            }
        }
    }
}
