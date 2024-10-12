using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    string query = "SELECT * FROM SACH WHERE bName LIKE @Nametext";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nametext", "%" + bookneedtofind + "%");


                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Book> books = new List<Book>();
                    while (reader.Read())
                    {
                        bool tinhtrang = reader.GetBoolean(8);

                        Book book = new Book
                        {
                            Number = reader.GetInt32(0),  // Giả sử số thứ tự nằm ở cột 0
                            Name = reader.GetString(2),   // Tên sách ở cột 1
                            Author = reader.GetString(3), // Tên tác giả ở cột 2
                            Publisher = reader.GetString(4), // Nhà xuất bản ở cột 3
                            PublishDate = reader.GetString(5),// Kiểm tra null cho PublishDate// Lấy giá trị kiểu DateTime từ SQ
                            Price = reader.GetInt64(6),// Giá bán ở cột 5
                            // Số lượng ở cột 7
                            BgColor = "#FF5733", // Màu nền tuỳ chọn (bạn có thể thay đổi theo nhu cầu)

                        };
                        if (tinhtrang) book.Status = "Còn sách";
                        else book.Status = "Hết sách";

                        books.Add(book);
                    }
                    bookDataGrid.ItemsSource = books;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }
        private void Button_Click_Borrow(object sender, RoutedEventArgs e)
        {


        }
    }
}
