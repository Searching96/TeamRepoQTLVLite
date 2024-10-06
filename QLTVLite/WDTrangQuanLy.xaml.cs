using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLTVLite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WDTrangQuanLy : Window
    {
        private int userRole; // Biến để lưu phân quyền người dùng

        public WDTrangQuanLy(int role)
        {
            InitializeComponent();
            userRole = role; // Nhận phân quyền từ cửa sổ đăng nhập
            SetupUI();
        }

        private void SetupUI()
        {
            // Kiểm tra phân quyền và ẩn/hiện button
            if (userRole == 1)
            {
                btnQuanLySach.Visibility = Visibility.Visible;
                btnThongKe.Visibility = Visibility.Visible;
            }
            else if (userRole == 2)
            {
                btnQuanLySach.Visibility = Visibility.Visible;
                btnThongKe.Visibility = Visibility.Collapsed;
            }
            //else
            //{
            //    // Nếu không phải phân quyền 1 hoặc 2, ẩn tất cả
            //    btnQuanLySach.Visibility = Visibility.Collapsed;
            //    btnThongKe.Visibility = Visibility.Collapsed;
            //}
        }

        private void NavigateToUCQuanLySach(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCQuanLySach();
        }

        private void NavigateToUCThongKe(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCThongKe();
        }


    }
}