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
using Library_BUS;
using System.Windows.Media;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Books : UserControl, INotifyPropertyChanged
    {
        private BookManager _bookManager;

        private ObservableCollection<Book> _allBooks;
        private ObservableCollection<Book> _currentPageBooks;
        private int _itemsPerPage = 10;
        private int _currentPage = 1;
        public ObservableCollection<Book> CurrentPageBooks
        {
            get => _currentPageBooks;
            set
            {
                _currentPageBooks = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                UpdateCurrentPageBooks();
            }
        }

        private void UpdateCurrentPageBooks()
        {
            CurrentPageBooks = new ObservableCollection<Book>(
                _allBooks.Skip((CurrentPage - 1) * _itemsPerPage)
                           .Take(_itemsPerPage)
                           .ToList());
        }

        public int TotalPages => (int)Math.Ceiling((double)_allBooks.Count / _itemsPerPage);

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
            _bookManager = new();
            LoadBooks();
            MultiSelect = Visibility.Visible;
            GeneratePageButtons();
        }

        private void LoadBooks()
        {
            using (var context = new LibraryManagementContext())
            {
                _allBooks = new ObservableCollection<Book>(
                    context.Books.ToList());
                UpdateCurrentPageBooks();
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
                _bookManager.UpdateBook(_book.BookId, _book.Title);
                LoadBooks();
            }
        }

        private void btn_UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItems.Count != 1) return;
            var selectedBook = BooksDataGrid.SelectedItem as Book;


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
                else if (Book.BookId != 0)
                {
                    MessageBox.Show("Book is borrowed, unable to delete.");
                    continue;
                }
                else
                {
                    _bookManager.RemoveBook(Book.BookId);
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

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new LibraryManagementContext())
            {
                var query = context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(Search))
                {
                    query = query.Where(r =>
                    r.Title.Contains(Search));
                    _allBooks = new ObservableCollection<Book>(query.ToList());
                }
                else
                {
                    _allBooks = new ObservableCollection<Book>(
                        context.Books.ToList());
                }
            }
        }

        //Pagination

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                GeneratePageButtons();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                GeneratePageButtons();
            }
        }

        private void PageNumber_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Content.ToString(), out int pageNumber))
            {
                CurrentPage = pageNumber;
                GeneratePageButtons();
            }
        }

        private void GeneratePageButtons()
        {
            PaginationPanel.Children.Clear();

            int startPage = Math.Max(1, CurrentPage - 2);
            int endPage = Math.Min(TotalPages, startPage + 4);

            if (startPage >= endPage)
            {
                PaginationBorder.Visibility = Visibility.Collapsed;
                return;
            }

            PaginationPanel.Children.Add(CreatePageButton("<<", PreviousPage_Click));

            if (startPage > 1)
            {
                PaginationPanel.Children.Add(CreatePageButton("1", PageNumber_Click));
                if (startPage > 2)
                {
                    PaginationPanel.Children.Add(new TextBlock { Text = "...", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) });
                }
            }

            for (int i = startPage; i <= endPage; i++)
            {
                Button pageButton = CreatePageButton(i.ToString(), PageNumber_Click);
                if (i == CurrentPage)
                {
                    pageButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7950F2"));
                    pageButton.Foreground = Brushes.White;
                }
                PaginationPanel.Children.Add(pageButton);
            }

            if (endPage < TotalPages)
            {
                if (endPage < TotalPages - 1)
                {
                    PaginationPanel.Children.Add(new TextBlock { Text = "...", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) });
                }
                PaginationPanel.Children.Add(CreatePageButton(TotalPages.ToString(), PageNumber_Click));
            }

            PaginationPanel.Children.Add(CreatePageButton(">>", NextPage_Click));
        }

        private Button CreatePageButton(string content, RoutedEventHandler clickHandler)
        {
            var button = new Button
            {
                Content = content,
                Style = (Style)FindResource("pagingButton")
            };
            button.Click += clickHandler;
            return button;
        }
    }
}
