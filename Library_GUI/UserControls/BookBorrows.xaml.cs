using Library_BUS;
using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for BookBorrows.xaml
    /// </summary>
    public partial class BookBorrows : UserControl, INotifyPropertyChanged
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BorrowManager _borrowManager;
        private ObservableCollection<BorrowViewModel> _allBookBorrows;
        private ObservableCollection<BorrowViewModel> _currentPageBookBorrows;
        private readonly int _itemsPerPage = 10;
        private int _currentPage = 1;
        private string _search;

        public event PropertyChangedEventHandler PropertyChanged;

        public BookBorrows(IUnitOfWork unitOfWork, BorrowManager borrowManager)
        {
            InitializeComponent();
            DataContext = this;
            _unitOfWork = unitOfWork;
            _borrowManager = borrowManager;
            LoadBookBorrows();
            GeneratePageButtons();
        }

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

        public ObservableCollection<BorrowViewModel> CurrentPageBookBorrows
        {
            get => _currentPageBookBorrows;
            set
            {
                _currentPageBookBorrows = value;
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
                UpdateCurrentPageBookBorrows();
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)_allBookBorrows.Count / _itemsPerPage);

        private void LoadBookBorrows()
        {
            try
            {
                var borrows = _borrowManager.GetAllBorrows();
                var borrowsWithDetails = borrows.Select(b => new BorrowViewModel
                {
                    BorrowId = b.BorrowId,
                    Username = b.Username,
                    BorrowDate = b.Date,
                    Books = _unitOfWork.BorrowDetails
                        .GetByBorrowId(b.BorrowId)
                        .Select(bd => _unitOfWork.Books.GetById(bd.BookId))
                        .ToList(),
                    DueDate = _unitOfWork.BorrowDetails
                        .GetByBorrowId(b.BorrowId)
                        .FirstOrDefault()?.EndDate ?? DateTime.MinValue
                }).ToList();

                _allBookBorrows = new ObservableCollection<BorrowViewModel>(borrowsWithDetails);
                UpdateCurrentPageBookBorrows();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading borrows: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCurrentPageBookBorrows()
        {
            var filteredBorrows = string.IsNullOrEmpty(Search)
                ? _allBookBorrows
                : new ObservableCollection<BorrowViewModel>(
                    _allBookBorrows.Where(b =>
                        b.Username.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                        b.BookTitles.Contains(Search, StringComparison.OrdinalIgnoreCase)));

            CurrentPageBookBorrows = new ObservableCollection<BorrowViewModel>(
                filteredBorrows
                    .Skip((CurrentPage - 1) * _itemsPerPage)
                    .Take(_itemsPerPage));
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            UpdateCurrentPageBookBorrows();
            GeneratePageButtons();
        }

        private void btn_AddBorrow_Click(object sender, RoutedEventArgs e)
        {
            var borrowDialog = new SecondaryWindow(_unitOfWork, _borrowManager);
            if (borrowDialog.ShowDialog() == true)
            {
                LoadBookBorrows();
                GeneratePageButtons();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                var selectedBorrows = grid.SelectedItems.Cast<BorrowViewModel>().ToList();
                // Handle selection change if needed
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Pagination methods remain the same but use BorrowViewModel instead of Borrow
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

    public class BorrowViewModel : INotifyPropertyChanged
    {
        public int BorrowId { get; set; }
        public string Username { get; set; }
        public DateTime BorrowDate { get; set; }
        private List<Book> _books;
        public List<Book> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
                OnPropertyChanged(nameof(BookTitles));
            }
        }
        public DateTime DueDate { get; set; }
        public string BookTitles => string.Join(", ", Books?.Select(b => b.Title) ?? Array.Empty<string>());
        public string Status => DateTime.Now > DueDate ? "Overdue" : "Active";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
