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
        private LibraryContext _context;

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

        private List<Borrow> _selectedBookBorrows;
        public List<Borrow> SelectedBookBorrows
        {
            get => _selectedBookBorrows;
            set
            {
                _selectedBookBorrows = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BookBorrows()
        {
            InitializeComponent();
            DataContext = this;
            _context = new();
            LoadBookBorrows();
            MultiSelect = Visibility.Visible;
            GeneratePageButtons();
        }

        private void LoadBookBorrows()
        {
            using (var context = new LibraryManagementContext())
            {
                _allBookBorrows = new ObservableCollection<Borrow>(
                    context.Borrows.ToList());
                UpdateCurrentPageBookBorrows();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is BookBorrows viewModel)
            {
                viewModel.UpdateSelectedBookBorrows(BookBorrowsDataGrid.SelectedItems);
            }
        }

        public void UpdateSelectedBookBorrows(IList selectedItems)
        {
            SelectedBookBorrows = selectedItems.Cast<Borrow>().ToList();
        }

        private BorrowRepository _userRepository = new();

        public System.Windows.Visibility MultiSelect { get; set; }

        private void btn_AddBorrow_Click(object sender, RoutedEventArgs e)
        {
            var _borrow = new Borrow();
            var borrowDialog = new SecondaryWindow(_borrow);
            if (borrowDialog.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadBookBorrows();
            }
        }

        private void btn_UpdateBorrow_Click(object sender, RoutedEventArgs e)
        {
            if (BookBorrowsDataGrid.SelectedItems.Count != 1) return;
            var selectedBorrow = BookBorrowsDataGrid.SelectedItem as Borrow;
            if (selectedBorrow != null /*&& selectedBorrow.Debt == 0*/)
            {
                var bookDialog = new SecondaryWindow(selectedBorrow);
                if (bookDialog.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadBookBorrows();
                }
            }
            else if (selectedBorrow == null)
            {
                MessageBox.Show("Please select a borrow to edit.");
            }
            //else if (selectedBorrow.Debt != 0)
            //{
            //    MessageBox.Show("Borrow owes debt, unable to edit.");
            //}
        }

        private void btn_DeleteBorrow_Click(object sender, RoutedEventArgs e)
        {
            foreach (var Borrow in SelectedBookBorrows)
            {
                if (Borrow != null /*&& selectedBorrow.Debt == 0 */)
                {
                    _context.Borrows.Remove(Borrow);
                    _context.SaveChanges();
                    LoadBookBorrows();
                }
                else if (Borrow == null)
                {
                    MessageBox.Show("Please select a user to delete.");
                    continue;
                }
                //else if (selectedBorrow.Debt != 0)
                //{
                //    MessageBox.Show("Borrow owes debt, unable to delete.");
                //    continue;
                //}
                else
                {
                    MessageBox.Show("Borrow owes book, unable to delete.");
                    continue;
                }
            }
        }

        private void BookBorrows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookBorrowsDataGrid.SelectedItems.Count > 1)
                MultiSelect = Visibility.Visible;
            else
                MultiSelect = Visibility.Hidden;
        }


        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new LibraryManagementContext())
            {
                var query = context.Borrows.AsQueryable();

                if (!string.IsNullOrEmpty(Search))
                {
                    query = query.Where(r =>
                        r.Username.Contains(Search));
                }

                _allBookBorrows = new ObservableCollection<Borrow>(
                    query.ToList());

                CurrentPage = 1;
                UpdateCurrentPageBookBorrows();
                GeneratePageButtons();
            }
        }

        /* Pagination */

        private ObservableCollection<Borrow> _allBookBorrows;
        private ObservableCollection<Borrow> _currentPageBookBorrows;
        private int _itemsPerPage = 10;
        private int _currentPage = 1;

        public ObservableCollection<Borrow> CurrentPageBookBorrows
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

        private void UpdateCurrentPageBookBorrows()
        {
            CurrentPageBookBorrows = new ObservableCollection<Borrow>(
                _allBookBorrows.Skip((CurrentPage - 1) * _itemsPerPage)
                           .Take(_itemsPerPage)
                           .ToList());
        }

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
