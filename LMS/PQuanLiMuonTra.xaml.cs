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
    /// Interaction logic for PQuanLiMuonTra.xaml
    /// </summary>
    public partial class PQuanLiMuonTra : Page
    {
        public PQuanLiMuonTra()
        {
            InitializeComponent();
            LoadPMData();
        }
        string tensach;
        string tentacgia;
        List<int> selectedAuthorNumber = new List<int>();
        public void LoadPMData()
        {
            try
            {

                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog = Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();

                    string query = "SELECT PMUON.id, pmid, pmRedate, pmBodate, SACH.bName, DOCGIA.rName from PMUON " +"JOIN SACH on SACH.id = PMUON.bookbid "
                        + "JOIN DOCGIA on DOCGIA.id = PMUON.readerbid";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<CPhieuMuon> pms = new List<CPhieuMuon>();

                    while (reader.Read())
                    {
                        CPhieuMuon pm = new CPhieuMuon
                        {

                            Number = reader.GetInt32(0),  // Giả sử số thứ tự nằm ở cột 0
                            PMCode = reader.GetString(1),   // Tên sách ở cột 1
                            BorrowDate = reader.GetString(2), // Tên tác giả ở cột 2
                            ReturnDate = reader.GetString(3), // Nhà xuất bản ở cột 3
                            BookName = reader.GetString(4),// Kiểm tra null cho PublishDate// Lấy giá trị kiểu DateTime từ SQ
                            AuthorName = reader.GetString(5), // Giá bán ở cột 5
                  
                            BgColor = "#FF5733", // Màu nền tuỳ chọn (bạn có thể thay đổi theo nhu cầu)

                        };

                        pms.Add(pm);
                    }


                    PhieuMuonDataGrid.ItemsSource = pms;
                    reader.Close();
                   

                    // Gán giá trị cho TextBlock
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void LoadBookData()
        {
            try
            {

                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog = Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();

                    string query = "SELECT SACH.id, bName, bPubl, bPDate, bPrice, bQuan FROM SACH " +
                                    "WHERE bName = @bookName";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@bookName", tensach);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Book> books = new List<Book>();

                    while (reader.Read())
                    {
                        Book book = new Book
                        {

                            Number = reader.GetInt32(0),  // Giả sử số thứ tự nằm ở cột 0
                            Name = reader.GetString(1),   // Tên sách ở cột 1
                             // Tên tác giả ở cột 2
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
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void LoadAuthorData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog = Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();

                    // Xây dựng câu truy vấn động với IN và danh sách các tham số id
                    string query = "SELECT TACGIA.id, aName FROM TACGIA WHERE TACGIA.id IN (";

                    // Tạo các tham số cho câu truy vấn, ví dụ: @id0, @id1, @id2
                    for (int i = 0; i < selectedAuthorNumber.Count; i++)
                    {
                        query += "@id" + i;
                        if (i < selectedAuthorNumber.Count - 1)
                        {
                            query += ", ";  // Thêm dấu phẩy giữa các tham số
                        }
                    }
                    query += ")";  // Đóng ngoặc cho câu truy vấn IN

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Gán giá trị từ danh sách selectedAuthorNumber cho các tham số động
                    for (int i = 0; i < selectedAuthorNumber.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@id" + i, selectedAuthorNumber[i]);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<CLAuthor> authors = new List<CLAuthor>();

                        while (reader.Read())
                        {
                            CLAuthor author = new CLAuthor
                            {
                                Number = reader.GetInt32(0),  // Giả sử số thứ tự nằm ở cột 0
                                Authorname = reader.GetString(1),   // Tên tác giả ở cột 1
                                BgColor = "#FF5733", // Màu nền tuỳ chọn
                            };

                            authors.Add(author);
                        }

                        authorDataGrid.ItemsSource = authors;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu tác giả: " + ex.Message);
            }
        }
        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            LoadPMData();
        }

        private void Button_Click_AddPM(object sender, RoutedEventArgs e)
        {

        }
       
        private void PhieuMuonDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using(SqlConnection conn = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
            {
                
                CPhieuMuon selectedPM = (CPhieuMuon)PhieuMuonDataGrid.SelectedItem;
                tensach = selectedPM.BookName;
                tentacgia = selectedPM.AuthorName;
    
                // Lấy giá trị của cột Number (giả sử cột đầu tiên là N
                conn.Open();
                string query = "SELECT authorid FROM SACH_TACGIA " +
                               "JOIN SACH on SACH.id = SACH_TACGIA.bookid " + 
                               "WHERE bName = @bookName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@bookName", tensach);

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
            LoadBookData();
            LoadAuthorData();
        }
    }
}
