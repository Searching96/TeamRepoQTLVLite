﻿using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Data.SqlClient;
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

namespace LMS
{
    /// <summary>
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window, IBookAction
    {
        private bool IsMaximize = false;
        public List<CLAuthor> lAuthors { get; set; }
        
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

        private ViewBook viewbookpage;
        public AddBook(ViewBook vb)
        {
            InitializeComponent();
            viewbookpage = vb;
        }
        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            if (BookName.Text != "" && BookAuthor.Text != "" && BookpDate.Text != "" && BookPublication.Text != "" && BookPrice.Text != "" && BookQuantity.Text != "")
            {
                String bname = BookName.Text;
                String bauthor = BookAuthor.Text;
                String publication = BookPublication.Text;
                String pDate = BookpDate.Text;
                Int64 price = Int64.Parse(BookPrice.Text);
                Int64 quan = Int64.Parse(BookQuantity.Text);
                using (SqlConnection sqlCon = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
                {
                    sqlCon.Open();
                    


                    string query = "INSERT INTO SACH (bName, bPubl, bPDate, bPrice, bQuan) " +
                                   "VALUES (@bName, @bPubl, @bPDate, @bPrice, @bQuan)" +
                                   "SELECT id FROM SACH WHERE bName = @bName;";
                    int bookID = 0;

                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        // Thêm tham số vào câu lệnh SQL để tránh SQL Injection
                        sqlCmd.Parameters.AddWithValue("@bName", bname);
                        
                        sqlCmd.Parameters.AddWithValue("@bPubl", publication);
                        sqlCmd.Parameters.AddWithValue("@bPDate", pDate);
                        sqlCmd.Parameters.AddWithValue("@bPrice", price);
                        sqlCmd.Parameters.AddWithValue("@bQuan", quan);
                        bookID = (int)sqlCmd.ExecuteScalar();
                        
                    }
                    foreach (var author in lAuthors)
                    {
                        // Thêm mối quan hệ giữa sách và tác giả vào bảng SACH_TACGIA
                        string querytgsach = "INSERT INTO SACH_TACGIA (bookid, authorid) VALUES (@bookID, @authorID)";
                        using (SqlCommand sachtgCmd = new SqlCommand(querytgsach, sqlCon))
                        {
                            sachtgCmd.Parameters.AddWithValue("@bookID", bookID);
                            sachtgCmd.Parameters.AddWithValue("@authorID", author.Number);
                            sachtgCmd.ExecuteNonQuery(); // Thực thi câu lệnh chèn
                        }
                    }
                    
                }

                // Hiển thị thông báo thành công
                MessageBox.Show("Data Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                viewbookpage.LoadData();
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
        
        private void btn_AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            WAddAuthor waa = new WAddAuthor(this);
            waa.Show();
         
        }
    }
}
