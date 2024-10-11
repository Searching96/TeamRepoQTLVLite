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
    /// Interaction logic for WDSelectAuthor.xaml
    /// </summary>
    public partial class WDSelectAuthor : Window
    {
        public List<TacGia> SelectedAuthors { get; private set; }
        private List<TacGia> allAuthors = new List<TacGia>(); // Lưu tất cả các tác giả
        private List<TacGia> filteredAuthors = new List<TacGia>();

        public WDSelectAuthor(List<TacGia> currentSelectedAuthors)
        {
            InitializeComponent();

            // Khởi tạo danh sách các tác giả đã chọn
            SelectedAuthors = new List<TacGia>(currentSelectedAuthors);

            // Tải tất cả tác giả từ CSDL một lần và hiển thị
            LoadAllAuthors();

            // Hiển thị danh sách tác giả đã chọn
            UpdateSelectedAuthorsDisplay();
        }

        private void LoadAllAuthors()
        {
            using (var context = new AppDbContext())
            {
                // Lấy tất cả các tác giả từ DB và lưu vào allAuthors
                allAuthors = context.TACGIA.ToList();
            }

            // Hiển thị toàn bộ danh sách tác giả trong ListBox
            lstAuthors.ItemsSource = allAuthors;
        }

        private void UpdateSelectedAuthorsDisplay()
        {
            // Đánh dấu các tác giả đã chọn trong ListBox
            foreach (var author in allAuthors)
            {
                if (SelectedAuthors.Any(selected => selected.MaTacGia == author.MaTacGia))
                {
                    lstAuthors.SelectedItems.Add(author); // Thêm vào danh sách đã chọn
                }
            }
        }

        private void AuthorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Xử lý các tác giả được chọn hoặc bỏ chọn
            foreach (TacGia author in e.AddedItems)
            {
                if (!SelectedAuthors.Contains(author))
                {
                    SelectedAuthors.Add(author); // Thêm tác giả vào danh sách đã chọn
                }
            }

            foreach (TacGia author in e.RemovedItems)
            {
                if (SelectedAuthors.Contains(author))
                {
                    SelectedAuthors.Remove(author); // Bỏ tác giả ra khỏi danh sách đã chọn
                }
            }
        }

        private void ConfirmSelection_Click(object sender, RoutedEventArgs e)
        {
            // Lưu danh sách các tác giả đã chọn
            SelectedAuthors = lstAuthors.SelectedItems.Cast<TacGia>().ToList();

            this.DialogResult = true; // Đóng cửa sổ và xác nhận lựa chọn
            this.Close();
        }

        private void txtSearchAuthor_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lấy từ khóa tìm kiếm
            string searchText = txtSearchAuthor.Text.ToLower();

            // Lọc danh sách tác giả từ danh sách đã tải sẵn (allAuthors)
            filteredAuthors = allAuthors
                .Where(a => a.TenTacGia.ToLower().Contains(searchText))
                .ToList();

            // Cập nhật danh sách hiển thị trong ListBox
            lstAuthors.ItemsSource = filteredAuthors;
        }
    }

}
