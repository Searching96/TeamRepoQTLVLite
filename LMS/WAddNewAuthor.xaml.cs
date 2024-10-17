using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace LMS
{
    /// <summary>
    /// Interaction logic for WAddAuthor.xaml
    /// </summary>
    public partial class WAddNewAuthor : Window
    {
        WAddAuthor WAA;
        public WAddNewAuthor(WAddAuthor waa)
        {
            InitializeComponent();
            WAA = waa;
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

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            if (AuthorName.Text != "")
            {
                String aname = AuthorName.Text;
                
                using (SqlConnection sqlCon = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO TACGIA (aName) " +
                                   "VALUES (@Author)";
                                   
                    int bookID = 0;

                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        // Thêm tham số vào câu lệnh SQL để tránh SQL Injection
                        sqlCmd.Parameters.AddWithValue("@Author", aname);
                        sqlCmd.ExecuteNonQuery();
                    }
                    sqlCon.Close();
                    
                }

                // Hiển thị thông báo thành công
                MessageBox.Show("Data Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                WAA.LoadData();
                this.Close();
               
            }
            else
            {
                MessageBox.Show("Empty Field NOT Allowed", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
