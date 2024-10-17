using Library_DAL;
using Library_DTO;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Books : UserControl, INotifyPropertyChanged
    {
        private LibraryContext _context;

        private ObservableCollection<Book> _booksList;
        public ObservableCollection<Book> BooksList
        {
            get => _booksList;
            set
            {
                _booksList = value;
                OnPropertyChanged();
            }
        }

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                if (_search != value)
                {
                    _search = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Book> _selectedBooks;
        public List<Book> SelectedBooks
        {
            get => _selectedBooks;
            set
            {
                _selectedBooks = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Books()
        {
            InitializeComponent();
            DataContext = this;
            _context = new();
            LoadBooks();
            MultiSelect = Visibility.Visible;
        }

        private void LoadBooks()
        {
            using (var context = new LibraryManagementContext())
            {
                BooksList = new ObservableCollection<Book>(
                    context.Books.ToList());
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is Books viewModel)
            {
                viewModel.UpdateSelectedBooks(BooksDataGrid.SelectedItems);
            }
        }

        public void UpdateSelectedBooks(IList selectedItems)
        {
            SelectedBooks = selectedItems.Cast<Book>().ToList();
        }

        private BookRepository _bookRepository = new();

        public System.Windows.Visibility MultiSelect { get; set; }

        private void btn_AddBook_Click(object sender, RoutedEventArgs e)
        {
            var _book = new Book();
            var bookDialog = new SecondaryWindow(_book);
            if (bookDialog.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadBooks();
            }
        }

        private void btn_UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItems.Count != 1) return;
            var selectedBook = BooksDataGrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Please select a book to edit.");
            }
            else if ((bool)selectedBook.IsBorrowed)
            {
                MessageBox.Show("Book owes book, unable to edit.");
            }
            else /*&& selectedBook.Debt == 0*/
            {
                var bookDialog = new SecondaryWindow(selectedBook);
                if (bookDialog.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadBooks();
                }
            }

            //else if (selectedBook.Debt != 0)
            //{
            //    MessageBox.Show("Book owes debt, unable to edit.");
            //}

        }

        private void btn_DeleteBook_Click (object sender, RoutedEventArgs e)
        {
            foreach (var Book in SelectedBooks)
            {
                if (Book == null)
                {
                    MessageBox.Show("Please select a book to delete.");
                    continue;
                }
                else if ((bool)Book.IsBorrowed)
                {
                    MessageBox.Show("Book is borrowed, unable to delete.");
                    continue;
                }
                else
                {
                    _context.Books.Remove(Book);
                    _context.SaveChanges();
                    LoadBooks();
                }
                //else if (selectedBook.Debt != 0)
                //{
                //    MessageBox.Show("Book owes debt, unable to delete.");
                //    continue;
                //}

            }
        }

        private void Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BooksDataGrid.SelectedItems.Count > 1)
                MultiSelect = Visibility.Visible;
            else
                MultiSelect = Visibility.Hidden;
        }

        private void btn_Search_MouseEnter(object sender, MouseEventArgs e)
        {
            icon_Search.Opacity = 0.7;
        }

        private void btn_Search_MouseLeave(object sender, MouseEventArgs e)
        {
            icon_Search.Opacity = 0.3;
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new LibraryManagementContext())
            {
                var query = context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(Search))
                {
                    query = query.Where(r =>
                    r.Title.Contains(Search));
                    BooksList = new ObservableCollection<Book>(query.ToList());
                }
                else
                {
                    BooksList = new ObservableCollection<Book>(
                        context.Books.ToList());
                }
            }
        }
    }
}
