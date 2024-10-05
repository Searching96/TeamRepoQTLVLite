using QLTVLite.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace QLTVLite
{
    /// <summary>
    /// Interaction logic for UCQuanLySach.xaml
    /// </summary>
    public partial class UCQuanLySach : UserControl
    {
        public ObservableCollection<Sach> Books { get; set; }

        public UCQuanLySach()
        {
            InitializeComponent();
            Books = new ObservableCollection<Sach>();
            BooksDataGrid.ItemsSource = Books; // Gán danh sách sách cho DataGrid
            LoadBook(); // Tải danh sách sách từ cơ sở dữ liệu
        }

        private void LoadBook()
        {
            using (var context = new AppDbContext())
            {
                List<Sach> dsSach = context.SACH.ToList();
                BooksDataGrid.ItemsSource = dsSach;
            }
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            WDAddBook wdAddBook = new WDAddBook();
            if (wdAddBook.ShowDialog() == true)
            {
                LoadBook();
            }
        }
        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Sach selectedBook)
            {
                WDEditBook wdEditBook = new WDEditBook(selectedBook);

                if (wdEditBook.ShowDialog() == true)
                {
                    LoadBook();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sách để sửa");
            }
        }
        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có sách nào được chọn không
            if (BooksDataGrid.SelectedItem is Sach selectedBook)
            {
                // Hiển thị thông báo xác nhận trước khi xóa
                MessageBoxResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sách: {selectedBook.TenSach}?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                // Nếu người dùng chọn "Yes", thực hiện xóa
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext()) // Thay YourDbContext bằng tên context của bạn
                    {
                        // Tìm sách trong cơ sở dữ liệu bằng mã sách
                        var bookToDelete = context.SACH.Find(selectedBook.MaSach);
                        if (bookToDelete != null)
                        {
                            // Xóa sách và lưu thay đổi
                            context.SACH.Remove(bookToDelete);
                            context.SaveChanges();

                            // Tải lại danh sách sách để cập nhật giao diện
                            LoadBook();
                        }
                    }
                }
            }
            else
            {
                // Nếu không có sách nào được chọn, hiển thị thông báo lỗi
                MessageBox.Show("Vui lòng chọn một sách để xóa.");
            }
        }

        private void SearchBook_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim();
            string selectedProperty = ((ComboBoxItem)SearchPropertyComboBox.SelectedItem)?.Content.ToString();

            using (var context = new AppDbContext()) // Thay YourDbContext bằng tên context của bạn
            {
                IEnumerable<Sach> results = Enumerable.Empty<Sach>();

                if (selectedProperty == "Tên Sách")
                {
                    results = context.SACH
                        .Where(b => b.TenSach.Contains(searchTerm))
                        .ToList();
                }
                else if (selectedProperty == "Tác Giả")
                {
                    results = context.SACH
                        .Where(b => b.TacGia.Contains(searchTerm))
                        .ToList();
                }
                else if (selectedProperty == "Thể Loại")
                {
                    results = context.SACH
                        .Where(b => b.TheLoai.Contains(searchTerm))
                        .ToList();
                }

                BooksDataGrid.ItemsSource = results;
            }
        }
    }
}
