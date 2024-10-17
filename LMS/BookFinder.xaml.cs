using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
using System.Xml.Linq;

namespace LMS
{
    /// <summary>
    /// Interaction logic for BookFinder.xaml
    /// </summary>
    public partial class BookFinder : Page
    {
        string bookneedtofind;
        public BookFinder(string b)
        {
            InitializeComponent();
            bookneedtofind = b;
            LoadData();
        }
        int selectedBookNumber = -1;
        public void bookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book selectedBook = (Book)bookDataGrid.SelectedItem;

            // Lấy giá trị của cột Number (giả sử cột đầu tiên là Number)
            selectedBookNumber = selectedBook.Number;
        }
        public void LoadData()
        {
            MessageBox.Show(bookneedtofind);
            try
            {

                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog = Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();

                    string query = "SELECT SACH.id, bName, bPubl, bPDate, bPrice, bQuan, STRING_AGG(aName, ', ') AS Authors FROM SACH " +
                                   "JOIN SACH_TACGIA ON SACH_TACGIA.bookid = SACH.id " +
                                   "JOIN TACGIA ON TACGIA.id = SACH_TACGIA.authorid " +
                                   "WHERE bName = @bookfinded " +
                                   "GROUP BY SACH.id, bName, bPubl, bPDate, bPrice, bQuan";
                    

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@bookfinded", bookneedtofind);

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
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void Button_Click_Borrow(object sender, RoutedEventArgs e)
        {


        }
    }
}
