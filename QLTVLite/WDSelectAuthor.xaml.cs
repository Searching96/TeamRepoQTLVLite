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

        public WDSelectAuthor(List<TacGia> currentSelectedAuthors)
        {
            InitializeComponent();

            // Khởi tạo danh sách các tác giả đã chọn
            SelectedAuthors = new List<TacGia>(currentSelectedAuthors);
            string authorsList = string.Join(", ", SelectedAuthors.Select(a => a.TenTacGia)); // Thay đổi theo cách bạn lưu tên tác giả
            //MessageBox.Show($"Các tác giả đã chọn: {authorsList}", "Thông báo");
            // Tải danh sách tác giả và đánh dấu những tác giả đã chọn
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            using (var context = new AppDbContext())
            {
                var authors = context.TACGIA.ToList(); // Lấy danh sách tác giả từ DB
                lstAuthors.ItemsSource = authors; // Giả sử lstAuthors là ListBox trong XAML

                // Đánh dấu các tác giả đã chọn
                foreach (var author in authors)
                {
                    // Kiểm tra nếu MaTacGia của tác giả đã chọn nằm trong danh sách đã chọn
                    if (SelectedAuthors.Any(selected => selected.MaTacGia == author.MaTacGia))
                    {
                        lstAuthors.SelectedItems.Add(author); // Thêm vào danh sách đã chọn
                    }
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
    }

}
