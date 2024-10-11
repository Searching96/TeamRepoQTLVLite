using Microsoft.EntityFrameworkCore;
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
        private List<TacGia> selectedAuthors;

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
                var dsSach = context.SACH
                    .Select(s => new
                    {
                        s.MaSach,
                        s.TenSach,
                        s.TheLoai,
                        s.NamXuatBan,
                        DSTacGia = string.Join(", ", context.SACH_TACGIA
                            .Where(stg => stg.IDSach == s.ID)
                            .Select(stg => stg.TacGia.TenTacGia)
                            .ToList())
                    })
                    .ToList();

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
            var selectedItem = BooksDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var maSach = (string)selectedItem.GetType().GetProperty("MaSach").GetValue(selectedItem, null);

                using (var context = new AppDbContext())
                {
                    // Tìm cuốn sách cần cập nhật
                    var bookToUpdate = context.SACH.FirstOrDefault(s => s.MaSach == maSach);

                    if (bookToUpdate != null)
                    {
                        // Cập nhật các thuộc tính sách
                        bookToUpdate.TenSach = (string)selectedItem.GetType().GetProperty("TenSach").GetValue(selectedItem, null);
                        bookToUpdate.TheLoai = (string)selectedItem.GetType().GetProperty("TheLoai").GetValue(selectedItem, null);
                        bookToUpdate.NamXuatBan = (int)selectedItem.GetType().GetProperty("NamXuatBan").GetValue(selectedItem, null);

                        // Nếu selectedAuthors không null, xử lý cập nhật tác giả
                        if (selectedAuthors != null)
                        {
                            // Xóa các liên kết SACH_TACGIA hiện tại
                            var currentAuthors = context.SACH_TACGIA.Where(stg => stg.IDSach == bookToUpdate.ID).ToList();
                            context.SACH_TACGIA.RemoveRange(currentAuthors);

                            // Thêm các liên kết SACH_TACGIA mới dựa trên selectedAuthors
                            foreach (var author in selectedAuthors)
                            {
                                context.SACH_TACGIA.Add(new Sach_TacGia
                                {
                                    IDSach = bookToUpdate.ID,
                                    IDTacGia = author.ID
                                });
                            }
                        }

                        // Lưu các thay đổi vào CSDL
                        context.SaveChanges();

                        // Đặt selectedAuthors về null sau khi cập nhật xong
                        selectedAuthors = null;

                        // Hiển thị thông báo và tải lại dữ liệu cho DataGrid
                        MessageBox.Show("Thông tin sách đã được cập nhật thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadBook();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy cuốn sách để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách để chỉnh sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có sách nào được chọn không
            var selectedItem = BooksDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                // Cho nay can "if (BooksDataGrid.SelectedItem is Sach selectedBook)" ko???
                // Lấy MaSach của sách được chọn
                var maSach = (string)selectedItem.GetType().GetProperty("MaSach").GetValue(selectedItem, null);

                // Hiển thị thông báo xác nhận trước khi xóa
                MessageBoxResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sách có mã: {maSach}?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                // Nếu người dùng chọn "Yes", thực hiện xóa
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        // Tìm sách trong cơ sở dữ liệu bằng mã sách
                        var bookToDelete = context.SACH.FirstOrDefault(s => s.MaSach == maSach);
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

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lấy đối tượng đã chọn từ DataGrid
            var selectedItem = BooksDataGrid.SelectedItem;

            // Kiểm tra xem selectedItem có dữ liệu không
            if (selectedItem != null)
            {
                // Sử dụng dynamic để không cần ép kiểu cụ thể
                dynamic selectedBook = selectedItem;

                // Hiển thị thông tin sách
                txtMaSach.Text = selectedBook.MaSach;
                txtTenSach.Text = selectedBook.TenSach;
                txtTheLoai.Text = selectedBook.TheLoai;
                txtNamXuatBan.Text = selectedBook.NamXuatBan.ToString();

                // Hiển thị nguyên chuỗi DSTacGia
                txtTenTacGia.Text = !string.IsNullOrWhiteSpace(selectedBook.DSTacGia)
                    ? selectedBook.DSTacGia
                    : "Không có tên tác giả";
            }
            else
            {
                // Nếu không có sách nào được chọn, xóa các TextBox
                txtMaSach.Text = "";
                txtTenSach.Text = "";
                txtTenTacGia.Text = "";
                txtNamXuatBan.Text = "";
                txtTheLoai.Text = "";
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
                else if (selectedProperty == "Thể Loại")
                {
                    results = context.SACH
                        .Where(b => b.TheLoai.Contains(searchTerm))
                        .ToList();
                }

                BooksDataGrid.ItemsSource = results;
            }
        }

        private void btnEditAuthors_Click(object sender, RoutedEventArgs e)
        {
            var currentAuthors = txtTenTacGia.Text
                .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            using (var context = new AppDbContext())
            {
                var allAuthors = context.TACGIA.ToList();

                var wdSelectAuthor = new WDSelectAuthor(
                    allAuthors.Where(a => currentAuthors.Contains(a.TenTacGia)).ToList()
                );

                if (wdSelectAuthor.ShowDialog() == true)
                {
                    selectedAuthors = wdSelectAuthor.SelectedAuthors; // Cập nhật biến toàn cục
                    txtTenTacGia.Text = string.Join(", ", selectedAuthors.Select(a => a.TenTacGia));
                }
            }
        }
    }
}
