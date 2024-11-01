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
using Library_BUS;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for BookDialog.xaml
    /// </summary>
    public partial class BookDialog : UserControl
    {
        private readonly BookManager _bookManager;
        public Book Book { get; private set; }
        public EventHandler<bool> CloseDialog;

        public BookDialog(BookManager bookManager, Book selectedBook)
        {
            InitializeComponent();
            _bookManager = bookManager;
            Book = selectedBook ?? new Book();
            
            if (selectedBook != null)
            {
                txbTitle.Text = selectedBook.Title;
            }
        }

        private void BookSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    Book.Title = txbTitle.Text;
                    Book.BorrowId = null;  // New books are always available

                    if (Book.BookId != 0)
                    {
                        _bookManager.UpdateBook(Book.BookId, Book.Title);
                    }
                    else
                    {
                        _bookManager.AddBook(Book.Title);
                        Book.BorrowId = null;  // New books are always available
                    }

                    MessageBox.Show("Book saved successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseDialog?.Invoke(this, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving book: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txbTitle.Text))
            {
                MessageBox.Show("Please enter a title for the book.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void BookCancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog?.Invoke(this, false);
        }
    }
}
