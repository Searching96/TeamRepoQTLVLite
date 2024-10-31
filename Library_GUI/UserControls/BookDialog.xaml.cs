using Library_DAL;
using Library_DTO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for BookDialog.xaml
    /// </summary>
    public partial class BookDialog : UserControl
    {
        public Book Book { get; private set; }

        private BookRepository _bookRepository = new();

        public EventHandler<bool> CloseDialog;

        public BookDialog(Book? book = null)
        {
            InitializeComponent();
            DataContext = this;
            Book = book ?? new Book();
        }

        private void BookSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbTitle.Text))
            {
                MessageBox.Show("Please fill in all fields.");
            }
            else
            {
                Book.Title = txbTitle.Text;
                Book.BorrowId = 0;
                if (_bookRepository.GetByTitle(txbTitle.Text) != null)
                {
                    _bookRepository.Update(Book);
                }
                else
                {
                    _bookRepository.Add(Book);
                }
            }
            CloseDialog?.Invoke(this, true);
        }

        private void BookCancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog?.Invoke(this, false);
        }
    }
}
