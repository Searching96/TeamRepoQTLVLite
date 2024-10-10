using QLTVLite.Models;
using System;
using System.Collections.Generic;
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

namespace QLTVLite
{
    /// <summary>
    /// Interaction logic for WDAddBook.xaml
    /// </summary>
    public partial class WDAddBook : Window
    {
        private List<TacGia> selectedAuthors = new List<TacGia>(); // Khởi tạo danh sách tác giả đã chọn

        public WDAddBook()
        {
            InitializeComponent();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string tenSach = txtTenSach.Text;
            string theLoai = txtTheLoai.Text;
            int namXuatBan;

            if (!int.TryParse(txtNamXuatBan.Text, out namXuatBan))
            {
                MessageBox.Show("Năm xuất bản phải là số hợp lệ.");
                return;
            }

            // Kiểm tra nếu không có tác giả nào được chọn
            if (selectedAuthors.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một tác giả.");
                return;
            }

            using (var context = new AppDbContext())
            {
                // Tạo một sách mới
                var newBook = new Sach()
                {
                    TenSach = tenSach,
                    TheLoai = theLoai,
                    NamXuatBan = namXuatBan
                };

                // Thêm sách vào cơ sở dữ liệu
                context.SACH.Add(newBook);
                context.SaveChanges(); // MaSach sẽ được tự động tính toán

                // Thêm các bản ghi vào bảng Sach_TacGia
                foreach (var author in selectedAuthors)
                {
                    var sachTacGia = new Sach_TacGia
                    {
                        IDSach = newBook.ID, // ID của sách mới
                        IDTacGia = author.ID // ID của tác giả đã chọn
                    };
                    context.SACH_TACGIA.Add(sachTacGia);
                }

                // Lưu các thay đổi cho Sach_TacGia
                context.SaveChanges();
            }

            this.DialogResult = true;
            this.Close();
        }

        private void SelectAuthors_Click(object sender, RoutedEventArgs e)
        {
            // Tạo một cửa sổ chọn tác giả, truyền danh sách tác giả đã chọn
            var wdSelectAuthor = new WDSelectAuthor(selectedAuthors ?? new List<TacGia>());

            if (wdSelectAuthor.ShowDialog() == true)
            {
                selectedAuthors = wdSelectAuthor.SelectedAuthors ?? new List<TacGia>();

                // Cập nhật danh sách tác giả vào TextBox, ngăn cách bằng dấu phẩy
                txtAuthors.Text = string.Join(", ", selectedAuthors.Select(a => a.TenTacGia));
            }
        }
    }
}
