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
        public WDAddBook()
        {
            InitializeComponent();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string maSach = txtMaSach.Text;
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
                    MaSach = maSach,
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
