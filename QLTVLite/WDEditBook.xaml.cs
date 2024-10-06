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
    /// Interaction logic for WDEditBook.xaml
    /// </summary>
    public partial class WDEditBook : Window
    {
        private Sach bookToEdit;
        public WDEditBook(Sach book) { }
        //public WDEditBook(Sach book)
        //{
        //    InitializeComponent();
        //    bookToEdit = book;

        //    txtMaSach.Text = bookToEdit.MaSach;
        //    txtTenSach.Text = bookToEdit.TenSach;
        //    txtTacGia.Text = bookToEdit.TacGia;
        //    txtTheLoai.Text = bookToEdit.TheLoai;
        //    txtNamXuatBan.Text = bookToEdit.NamXuatBan.ToString();
        //}

        //private void SaveBook_Click(object sender, RoutedEventArgs e)
        //{
        //    bookToEdit.TenSach = txtTenSach.Text;
        //    bookToEdit.TacGia = txtTacGia.Text;
        //    bookToEdit.TheLoai = txtTheLoai.Text;

        //    int namXuatBan;
        //    if (int.TryParse(txtNamXuatBan.Text, out namXuatBan))
        //    {
        //        bookToEdit.NamXuatBan = namXuatBan;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Năm xuất bản phải là số hợp lệ");
        //        return;
        //    }

        //    using (var context = new AppDbContext())
        //    {
        //        var bookInDb = context.SACH.SingleOrDefault(s => s.MaSach == bookToEdit.MaSach);
        //        if (bookInDb != null)
        //        {
        //            bookInDb.TenSach = bookToEdit.TenSach;
        //            bookInDb.MaSach = bookToEdit.MaSach;
        //            bookInDb.TacGia = bookToEdit.TacGia;
        //            bookInDb.NamXuatBan = bookToEdit.NamXuatBan;

        //            context.SaveChanges();
        //        }
        //    }

        //    this.DialogResult = true;
        //    this.Close();
        //}
    }
}
