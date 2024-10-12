using System.Data.SqlClient;
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

namespace LMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void KeyDown_ESC(object sender, KeyEventArgs e)
        {
            // Kiểm tra nếu phím Esc được nhấn
            if (e.Key == Key.Escape)
            {
                this.Close(); // Đóng cửa sổ
            }
        }
        private string username;
        private string password;
        public MainWindow(string un, string pw)
        {
            InitializeComponent();
            username = un;
            password = pw;
            admintxt.Text = username;
        }
        private bool IsMaximize = false;

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }


        private void Button_Click_Book(object sender, RoutedEventArgs e)
        {
            AdminMain.Content = new ViewBook();

        }

        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True");
            try
            {

                sqlCon.Open();
                String query = "UPDATE NGUOIDUNG SET Status = @status WHERE Username = @Username AND Password = @Password";

                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Username", username);
                sqlCmd.Parameters.AddWithValue("@Password", password);
                sqlCmd.Parameters.AddWithValue("@status", 0);
                sqlCmd.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { sqlCon.Close(); }


            LoginScreen lg = new LoginScreen();
            lg.Show();
            this.Close();
        }

        }
    }
