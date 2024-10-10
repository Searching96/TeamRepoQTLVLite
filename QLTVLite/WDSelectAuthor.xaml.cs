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

            // Khởi tạo danh sách tác giả hiện có và các tác giả đã chọn
            LoadAuthors();
            SelectedAuthors = new List<TacGia>(currentSelectedAuthors);
        }

        private void LoadAuthors()
        {
            using (var context = new AppDbContext())
            {
                var authors = context.TACGIA.ToList(); // Lấy danh sách tác giả từ DB
                lstAuthors.ItemsSource = authors; // Giả sử lstAuthors là ListBox trong XAML
            }
        }

        private void AuthorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Xử lý sự kiện chọn tác giả
            var selectedAuthor = (TacGia)((ListBox)sender).SelectedItem;

            if (selectedAuthor != null)
            {
                if (SelectedAuthors.Contains(selectedAuthor))
                {
                    SelectedAuthors.Remove(selectedAuthor);
                }
                else
                {
                    SelectedAuthors.Add(selectedAuthor);
                }
            }
        }

        private void ConfirmSelection_Click(object sender, RoutedEventArgs e)
        {
            // Update selected authors
            SelectedAuthors = lstAuthors.SelectedItems.Cast<TacGia>().ToList();
            this.DialogResult = true; // Close window and confirm selection
            this.Close();
        }
    }
}
