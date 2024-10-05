using QLTVLite.Models;
using System.Windows;

namespace QLTVLite
{
    /// <summary>
    /// Interaction logic for WDAddBook.xaml
    /// </summary>
    public partial class WDAddBook : Window
    {
        public WDAddBook()
        {
            InitializeComponent();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string tenSach = txtTenSach.Text;
            string tacGia = txtTacGia.Text;
            string theLoai = txtTheLoai.Text;
            int namXuatBan;
            if (!int.TryParse(txtNamXuatBan.Text, out namXuatBan))
            {
                MessageBox.Show("Năm xuất bản phải là số hợp lệ.");
                return;

            }

            using (var context = new AppDbContext())
            {
                var newBook = new Sach()
                {
                    TenSach = tenSach,
                    TacGia = tacGia,
                    TheLoai = theLoai,
                    NamXuatBan = namXuatBan
                };

                context.SACH.Add(newBook);
                context.SaveChanges();
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
