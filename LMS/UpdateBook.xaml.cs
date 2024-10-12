using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for UpdateBook.xaml
    /// </summary>
    public partial class UpdateBook : Window
    {
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private ViewBook viewbookpage;
        int bookNumber;
        int authorNumber;
        public UpdateBook(ViewBook vb, int bn, int an)
        {
            InitializeComponent();
            viewbookpage = vb;
            bookNumber = bn;
            authorNumber = an;
            LoadCurrentBook();
            
        }

        public void LoadCurrentBook()
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source = BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV; Integrated Security = True; TrustServerCertificate=True"))
                {
                    conn.Open();
                    string query = "select SACH.id, bName, bPubl, bPDate, bPrice, bQuan, aName from SACH " + "join SACH_TACGIA on SACH_TACGIA.bookid = SACH.id " + "join TACGIA on TACGIA.id = SACH_TACGIA.authorid" + " where SACH.id = @Number";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Number", bookNumber);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Kiểm tra nếu có dòng dữ liệu
                        {
                            // Đọc và hiển thị các giá trị
                            BookName.Text = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            BookAuthor.Text = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                            BookPublication.Text = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            BookpDate.Text =  reader.GetString(3);
                            BookPrice.Text = reader.IsDBNull(4) ? string.Empty : reader.GetInt64(4).ToString();
                            BookQuantity.Text = reader.IsDBNull(5) ? string.Empty : reader.GetInt64(5).ToString();
                        }
                        else
                        {
                            // Thông báo nếu không tìm thấy dữ liệu
                            System.Windows.MessageBox.Show("Không tìm thấy sách với mã số này.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }
        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            
                String bname = BookName.Text;
                String bauthor = BookAuthor.Text;
                String publication = BookPublication.Text;
                String pDate = BookpDate.Text;
                Int64 price = Int64.Parse(BookPrice.Text);
                Int64 quan = Int64.Parse(BookQuantity.Text);

                using (SqlConnection sqlCon = new SqlConnection("Data Source=BjnB0\\SQLEXPRESS;Initial Catalog=Demo_QLTV;Integrated Security=True;TrustServerCertificate=True"))
                {
                    try
                    {
                        sqlCon.Open();
                    string query = "UPDATE SACH SET bName = @bname, bPubl = @bpubl, bPDate = @bpdate, bPrice = @bprice, bQuan = @bquan " +  "WHERE id = @bookNumber " +
                    "UPDATE TACGIA SET aName = @bauthor where id = @authorNumber";

                        using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                        {
                            // Thêm tham số vào câu lệnh SQL để tránh SQL Injection
                            sqlCmd.Parameters.AddWithValue("@bname", bname);
                            sqlCmd.Parameters.AddWithValue("@bauthor", bauthor);
                            sqlCmd.Parameters.AddWithValue("@bpubl", publication);
                            sqlCmd.Parameters.AddWithValue("@bpDate", pDate);
                            sqlCmd.Parameters.AddWithValue("@bprice", price);
                            sqlCmd.Parameters.AddWithValue("@bquan", quan);
                            sqlCmd.Parameters.AddWithValue("@bookNumber", bookNumber);
                            sqlCmd.Parameters.AddWithValue("@authorNumber", authorNumber);
                        // Thêm tham số @bookNumber

                        // Thực thi câu lệnh SQL
                        sqlCmd.ExecuteNonQuery();
                        }

                        // Hiển thị thông báo thành công
                        MessageBox.Show("Data Updated Successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        viewbookpage.LoadData();
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị lỗi nếu có
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
