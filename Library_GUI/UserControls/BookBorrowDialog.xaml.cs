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
        private List<Book> selectedBooks = new List<Book>();

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
                    .Where(b => b.BorrowId == null)
                    .ToList();

                if (books.Any())
                {
                    lstBooks.ItemsSource = books;
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
                    var borrowDate = dpBorrowDate.SelectedDate ?? DateTime.Now;
                    var dueDate = dpDueDate.SelectedDate ?? DateTime.Now.AddDays(14);

                    if (!_readerManager.CanBorrow(selectedReader))
                    {
                        MessageBox.Show("This reader cannot borrow more books.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Create the book-due date tuple list
                    var bookList = selectedBooks.Select(book => 
                        Tuple.Create(book, dueDate)).ToList();

                    // Process the borrow
                    _borrowManager.ProcessBorrow(selectedReader, bookList, borrowDate);

                    MessageBox.Show("Books borrowed successfully!", "Success",
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

        private void lstBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Book book in e.AddedItems)
            {
                selectedBooks.Add(book);
            }
            foreach (Book book in e.RemovedItems)
            {
                selectedBooks.Remove(book);
            }
        }

        private bool ValidateInputs()
        {
            if (!selectedBooks.Any())
            {
                MessageBox.Show("Please select at least one book.", "Validation Error",
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
