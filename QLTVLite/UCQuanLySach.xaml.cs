using QLTVLite.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        }

    }
}
