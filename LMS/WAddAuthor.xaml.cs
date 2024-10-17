using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    /// Interaction logic for AddAuthor.xaml
    /// </summary>
    public partial class WAddAuthor : Window
    {

        public IBookAction action;
        public WAddAuthor(IBookAction bookAction)
        {
            InitializeComponent();
            
            action = bookAction;

            ExecuteBookAction();


        }
        
        public void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog = Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();

                    string query = "select id, aName from TACGIA ";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<CLAuthor> authors = new List<CLAuthor>();

                    while (reader.Read())
                    {
                        CLAuthor author = new CLAuthor
                        {

                            Number = reader.GetInt32(0),  // Giả sử số thứ tự nằm ở cột 0
                            Authorname = reader.GetString(1),   // Tên tác giả ở cột 1
                            
                            BgColor = "#FF5733", // Màu nền tuỳ chọn (bạn có thể thay đổi theo nhu cầu)

                        };
                        authors.Add(author);
                    }
                    authorDataGrid.ItemsSource = authors;
                    reader.Close();
                    string totalQuery = "SELECT COUNT(*) FROM TACGIA";
                    SqlCommand totalCmd = new SqlCommand(totalQuery, conn);
                    var totalBooks = totalCmd.ExecuteScalar();

                    // Gán giá trị cho TextBlock
                    bookCountTextBlock.Text = "Tổng số lượng tác giả: " + totalBooks.ToString();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void LoadCurrenData(List<int> authorNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog = Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();

                    string query = "SELECT id, aName FROM TACGIA";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<CLAuthor> authors = new List<CLAuthor>();

                    while (reader.Read())
                    {
                        CLAuthor author = new CLAuthor
                        {
                            Number = reader.GetInt32(0),  // ID tác giả
                            Authorname = reader.GetString(1),   // Tên tác giả
                            BgColor = "#FF5733", // Màu nền tuỳ chọn
                             
                        };

                        authors.Add(author);
                    }

                    // Gán dữ liệu cho DataGrid
                    authorDataGrid.ItemsSource = authors;

                    // Tự động chọn các dòng có trong danh sách authorNumber
                    foreach (var author in authors)
                    {
                        if (authorNumber.Contains(author.Number))
                        {
                            
                            authorDataGrid.SelectedItems.Add(author);
                        }
                    }

                    reader.Close();

                    string totalQuery = "SELECT COUNT(*) FROM TACGIA";
                    SqlCommand totalCmd = new SqlCommand(totalQuery, conn);
                    var totalBooks = totalCmd.ExecuteScalar();

                    // Gán giá trị cho TextBlock
                    bookCountTextBlock.Text = "Tổng số lượng tác giả: " + totalBooks.ToString();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (nếu có)
            }
        }
        List<CLAuthor> selectedAuthor = new List<CLAuthor>();
        
        public void authorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedAuthor.Count > 0)
            {
                selectedAuthor.Clear();
            }
            foreach (var item in authorDataGrid.SelectedItems)
            {  
                // Kiểm tra và chuyển đổi từ item sang CLAuthor
                if (item is CLAuthor authorData)
                {
                    CLAuthor author = new CLAuthor
                    {
                        Number = authorData.Number,
                        
                        Authorname = authorData.Authorname
                    };
                    

                    // Thêm vào danh sách selectedAuthor
                    selectedAuthor.Add(author);
                }
            }
            
        }
        public void ExecuteBookAction()
        {
            // Gọi phương thức chung qua interface
            // Kiểm tra kiểu để gọi phương thức riêng
            if (action is AddBook addBook)
            {
                LoadData(); // Gọi phương thức riêng của AddBook
            }
            else if (action is UpdateBook updateBook)
            {
                LoadCurrenData(updateBook.authorNumber); // Gọi phương thức riêng của UpdateBook
            }
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            action.lAuthors = selectedAuthor;

            // Tạo chuỗi các tên tác giả từ danh sách được chọn
            string s = "";
            foreach (var author in selectedAuthor)
            {
                s += author.Authorname + ", ";
            }

            // Set BookAuthor.Text cho đối tượng action (AddBook hoặc UpdateBook)
            if (action is AddBook addBook)
            {
                addBook.BookAuthor.Text = s.TrimEnd(',', ' ');  // Đặt Text cho BookAuthor trong AddBook
            }
            else if (action is UpdateBook updateBook)
            {
                updateBook.BookAuthor.Text = s.TrimEnd(',', ' ');  // Đặt Text cho BookAuthor trong UpdateBook
            }

            MessageBox.Show("Data Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
        private void btn_addnewauthor_Click(object sender, RoutedEventArgs e)
        {
            WAddNewAuthor wana = new WAddNewAuthor(this);
            wana.Show();
        }
    }
}
