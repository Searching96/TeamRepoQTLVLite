using Library_DAL;
using Library_DTO;
using Library_BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Library_GUI.UserControls
{
    public partial class BookBorrowDialog : UserControl
    {
        private readonly BorrowManager _borrowManager;
        private readonly ReaderManager _readerManager;
        private readonly BookManager _bookManager;
        public EventHandler<bool> CloseDialog;

        public BookBorrowDialog(BorrowManager borrowManager, ReaderManager readerManager, BookManager bookManager)
        {
            InitializeComponent();

            _borrowManager = borrowManager;
            _readerManager = readerManager;
            _bookManager = bookManager;
            DataContext = this;

            // Set default dates
            dpBorrowDate.SelectedDate = DateTime.Now;
            dpDueDate.SelectedDate = DateTime.Now.AddDays(14);

            LoadBooks();
            LoadReaders();
        }

        private void LoadBooks()
        {
            try
            {
                var books = _bookManager.GetAllBooks()
                    .Where(b => b.BorrowId == 0) // Only show available books
                    .ToList();

                if (books.Any())
                {
                    cmbBooks.ItemsSource = books;
                    cmbBooks.DisplayMemberPath = "Title";
                    cmbBooks.SelectedValuePath = "BookId";
                }
                else
                {
                    MessageBox.Show("No available books found.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReaders()
        {
            try
            {
                var readers = _readerManager.GetAllReaders();
                if (readers.Any())
                {
                    cmbReaders.ItemsSource = readers;
                    cmbReaders.DisplayMemberPath = "Username";
                    cmbReaders.SelectedValuePath = "Username";
                }
                else
                {
                    MessageBox.Show("No readers found.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading readers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    var selectedReader = (Reader)cmbReaders.SelectedItem;
                    var selectedBook = (Book)cmbBooks.SelectedItem;
                    var borrowDate = dpBorrowDate.SelectedDate ?? DateTime.Now;
                    var dueDate = dpDueDate.SelectedDate ?? DateTime.Now.AddDays(14);

                    if (!_readerManager.CanBorrow(selectedReader))
                    {
                        MessageBox.Show("This reader cannot borrow more books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Create the book-due date tuple
                    var bookList = new List<Tuple<Book, DateTime>>
                    {
                        Tuple.Create(selectedBook, dueDate)
                    };

                    // Process the borrow
                    _borrowManager.ProcessBorrow(selectedReader, bookList, borrowDate);

                    MessageBox.Show("Book borrowed successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CloseDialog?.Invoke(this, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing borrow: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            if (cmbBooks.SelectedItem == null)
            {
                MessageBox.Show("Please select a book.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbReaders.SelectedItem == null)
            {
                MessageBox.Show("Please select a reader.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!dpBorrowDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a borrow date.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!dpDueDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a due date.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dpDueDate.SelectedDate < dpBorrowDate.SelectedDate)
            {
                MessageBox.Show("Due date cannot be earlier than borrow date.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog?.Invoke(this, false);
        }
    }
}
