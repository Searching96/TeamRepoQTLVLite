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
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookManager _bookManager;
        private ObservableCollection<Book> _allBooks;
        private ObservableCollection<Book> _currentPageBooks;
        private readonly int _itemsPerPage = 10;
        private int _currentPage = 1;
        private string _search;
        private List<Book> _selectedBooks;
        private Visibility _multiSelect;

        public event PropertyChangedEventHandler PropertyChanged;

        public Books(IUnitOfWork unitOfWork, BookManager bookManager)
        {
            InitializeComponent();
            DataContext = this;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _bookManager = bookManager ?? throw new ArgumentNullException(nameof(bookManager));
            
            LoadBooks();
            MultiSelect = Visibility.Hidden;
            GeneratePageButtons();
        }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Book> CurrentPageBooks
        {
            get => _currentPageBooks;
            set
            {
                _currentPageBooks = value;
                OnPropertyChanged();
            }
        }

        public List<Book> SelectedBooks
        {
            get => _selectedBooks;
            set
            {
                _selectedBooks = value;
                OnPropertyChanged();
                MultiSelect = value?.Count > 1 ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public Visibility MultiSelect
        {
            get => _multiSelect;
            set
            {
                _multiSelect = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                UpdateCurrentPageBooks();
                OnPropertyChanged();
            }
        }

        public int TotalPages => (int)Math.Ceiling((_allBooks?.Count ?? 0) / (double)_itemsPerPage);

        private void LoadBooks()
        {
            try
            {
                _allBooks = new ObservableCollection<Book>(_bookManager.GetAllBooks());
                UpdateCurrentPageBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCurrentPageBooks()
        {
            if (_allBooks == null) return;

            var filteredBooks = string.IsNullOrEmpty(Search)
                ? _allBooks
                : new ObservableCollection<Book>(
                    _allBooks.Where(b => 
                        b.Title.Contains(Search, StringComparison.OrdinalIgnoreCase)));

            CurrentPageBooks = new ObservableCollection<Book>(
                filteredBooks
                    .Skip((CurrentPage - 1) * _itemsPerPage)
                    .Take(_itemsPerPage));
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedBooks(BooksDataGrid.SelectedItems);
        }

        private void btn_AddBook_Click(object sender, RoutedEventArgs e)
        {
            var book = new Book();
            var bookDialog = new SecondaryWindow(_unitOfWork, _bookManager, book);
            if (bookDialog.ShowDialog() == true)
            {
                LoadBooks();
                GeneratePageButtons();
            }
        }

        private void btn_UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Book selectedBook)
            {
                var bookDialog = new SecondaryWindow(_unitOfWork, _bookManager, selectedBook);
                if (bookDialog.ShowDialog() == true)
                {
                    LoadBooks();
                    GeneratePageButtons();
                }
            }
        }

        private void btn_DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var bookToDelete = button.DataContext as Book ?? SelectedBooks?.FirstOrDefault();
                if (bookToDelete == null)
                {
                    MessageBox.Show("Please select a book to delete.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (bookToDelete.BorrowId.HasValue)
                {
                    MessageBox.Show("Cannot delete a borrowed book.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _bookManager.RemoveBook(bookToDelete.BookId);
                        LoadBooks();
                        GeneratePageButtons();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting book: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            UpdateCurrentPageBooks();
            GeneratePageButtons();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public void UpdateSelectedBooks(IList selectedItems)
        {
            SelectedBooks = selectedItems.Cast<Book>().ToList();
        }
    }
}
