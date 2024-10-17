using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;

namespace LMS
{
    /// <summary>
    /// Interaction logic for ViewBook.xaml
    /// </summary>
    public partial class ViewBook : Page
    {
        bool insearchmode = false;
        public ViewBook()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                
                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog = Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();
                    
                    string query = "SELECT SACH.id, bName, bPubl, bPDate, bPrice, bQuan, STRING_AGG(aName, ', ') AS Authors FROM SACH " +
                                   "JOIN SACH_TACGIA ON SACH_TACGIA.bookid = SACH.id " +
                                   "JOIN TACGIA ON TACGIA.id = SACH_TACGIA.authorid " +
                                   "GROUP BY SACH.id, bName, bPubl, bPDate, bPrice, bQuan";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    List<Book> books = new List<Book>();
                    
                    while (reader.Read())
                    {
                        Book book = new Book
                        {
                            
                            Number = reader.GetInt32(0),  // Giả sử số thứ tự nằm ở cột 0
                            Name = reader.GetString(1),   // Tên sách ở cột 1
                            Author = reader.GetString(6), // Tên tác giả ở cột 2
                            Publisher = reader.GetString(2), // Nhà xuất bản ở cột 3
                            PublishDate = reader.GetString(3),// Kiểm tra null cho PublishDate// Lấy giá trị kiểu DateTime từ SQ
                            Price = reader.GetInt64(4), // Giá bán ở cột 5
                            Quantity = reader.GetInt64(5), // Số lượng ở cột 6
                            BgColor = "#FF5733", // Màu nền tuỳ chọn (bạn có thể thay đổi theo nhu cầu)
                            
                        };

                        books.Add(book);
                    }
                    
                    
                    bookDataGrid.ItemsSource = books;
                    reader.Close();
                    string totalQuery = "SELECT SUM(bQuan) FROM SACH";
                    SqlCommand totalCmd = new SqlCommand(totalQuery, conn);
                    var totalBooks = totalCmd.ExecuteScalar();

                    // Gán giá trị cho TextBlock
                    bookCountTextBlock.Text = "Tổng số lượng sách: " + totalBooks.ToString();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private void Button_Click_AddBook(object sender, RoutedEventArgs e)
        {

            AddBook ab = new AddBook(this);
            ab.Show();
            
        }
        
        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        
        private int selectedBookNumber = -1;
        private List<int> selectedAuthorNumber = new List<int>();
        public void bookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book selectedBook = (Book)bookDataGrid.SelectedItem;

            // Lấy giá trị của cột Number (giả sử cột đầu tiên là Number)
            
            selectedBookNumber = selectedBook.Number;
            using (SqlConnection conn = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
            {
                conn.Open();
                string query = "SELECT authorid FROM SACH_TACGIA WHERE bookid = @bookNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@bookNumber", selectedBookNumber);

                // Sử dụng SqlDataReader để đọc nhiều giá trị authorid
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    selectedAuthorNumber.Clear(); // Xóa danh sách trước khi thêm mới

                    while (reader.Read())
                    {
                        // Lấy giá trị của authorid và thêm vào danh sách
                        selectedAuthorNumber.Add(reader.GetInt32(0));
                    }
                }

                conn.Close();
            }
        }
        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (selectedBookNumber != -1)
            {
                // Xác nhận trước khi xóa (không bắt buộc)
                if (MessageBox.Show("Bạn có chắc muốn xóa sách này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // Thực hiện xóa dòng khỏi cơ sở dữ liệu
                    DeleteBookFromDatabase(selectedBookNumber);

                    // Cập nhật lại DataGrid sau khi xóa
                    if(!insearchmode) LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách để xóa.");
            }
        }
        private void DeleteBookFromDatabase(int bookNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    string query = "DELETE FROM SACH WHERE id = @Number " + "DELETE FROM SACH_TACGIA WHERE bookid = @Number";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Number", bookNumber);
                    
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa: " + ex.Message);
            }
        }
        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {

            UpdateBook ub = new UpdateBook(this, selectedBookNumber, selectedAuthorNumber);
            ub.Show();
        }
        int timtheo = 0;
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilter.SelectedItem != null)
            {
                // Lấy ComboBoxItem đã chọn
                ComboBoxItem selectedItem = (ComboBoxItem)cbFilter.SelectedItem;

                // Lấy giá trị Tag của ComboBoxItem
                
                int selectedValue = int.Parse(selectedItem.Tag.ToString());
                {
                    // Thực hiện hành động tùy theo giá trị chọn
                    switch (selectedValue)
                    {
                        case 1:
                            
                            // Chọn "Tên sách"
                            timtheo = 1; break;
                            
                        case 2:
                            // Chọn "Tác giả"
                            timtheo = 2; break;
                        case 3:
                            // Chọn "Thể loại"
                            timtheo = 3; break;
                        default:
                            break;
                    }
                }
            }
            
        }
        
        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxFilter.Text != "")
            {
                insearchmode = true;
                using (SqlConnection conn = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    string query;
                    
                    if (timtheo == 1)
                    {
                        query = "select SACH.id, bName, bPubl, bPDate, bPrice, bQuan, aName from SACH " + "join SACH_TACGIA on SACH_TACGIA.bookid = SACH.id " + "join TACGIA on TACGIA.id = SACH_TACGIA.authorid " + "where bName LIKE @text";
                    }
                    else if(timtheo == 2)
                    {
                        query = "select SACH.id, bName, bPubl, bPDate, bPrice, bQuan, aName from SACH " + "join SACH_TACGIA on SACH_TACGIA.bookid = SACH.id " + "join TACGIA on TACGIA.id = SACH_TACGIA.authorid " + "where aName LIKE @text";
                    }
                    else query = "select SACH.id, bName, bPubl, bPDate, bPrice, bQuan, aName from SACH " + "join SACH_TACGIA on SACH_TACGIA.bookid = SACH.id " + "join TACGIA on TACGIA.id = SACH_TACGIA.authorid " + "where bPubl LIKE @text";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@text", "%" + textBoxFilter.Text + "%");
                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Book> books = new List<Book>();
                    while (reader.Read())
                    {


                        Book book = new Book
                        {
                            Number = reader.GetInt32(0),  // Giả sử số thứ tự nằm ở cột 0
                            Name = reader.GetString(1),   // Tên sách ở cột 1
                            Author = reader.GetString(6), // Tên tác giả ở cột 2
                            Publisher = reader.GetString(2), // Nhà xuất bản ở cột 3
                            PublishDate = reader.GetString(3),// Kiểm tra null cho PublishDate// Lấy giá trị kiểu DateTime từ SQ
                            Price = reader.GetInt64(4), // Giá bán ở cột 5
                            Quantity = reader.GetInt64(5), // Số lượng ở cột 6
                            BgColor = "#FF5733", // Màu nền tuỳ chọn (bạn có thể thay đổi theo nhu cầu)

                        };

                        books.Add(book);
                    }
                    bookDataGrid.ItemsSource = books;
                    reader.Close();
                }
            }
            else
            {
                LoadData();
                insearchmode = false;
            }
                
            
        }

        
    }
}
