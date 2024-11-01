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
    /// Interaction logic for Returns.xaml
    /// </summary>
    public partial class Returns : UserControl, INotifyPropertyChanged
    {
        private LibraryContext _context = new();
        private UnitOfWork _unitOfWork;
        private ObservableCollection<Return> _allReturns;
        private ObservableCollection<Return> _currentPageReturns;
        private int _itemsPerPage = 10;
        private int _currentPage = 1;

        public ObservableCollection<Return> CurrentPageReturns
        {
            get => _currentPageReturns;
            set
            {
                _currentPageReturns = value;
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
                UpdateCurrentPageReturns();
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)_allReturns.Count / _itemsPerPage);

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

        private List<Return> _selectedReturns;
        public List<Return> SelectedReturns
        {
            get => _selectedReturns;
            set
            {
                _selectedReturns = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Returns()
        {
            InitializeComponent();
            DataContext = this;
            _unitOfWork = new(_context);
            _returnManager = new ReturnManager(_unitOfWork);
            LoadReturns();
            MultiSelect = Visibility.Visible;
            GeneratePageButtons();
        }

        private void LoadReturns()
        {
            using (var context = new LibraryManagementContext())
            {
                _allReturns = new ObservableCollection<Return>(
                    context.Returns.ToList());
                UpdateCurrentPageReturns();
            }
        }

        private void UpdateCurrentPageReturns()
        {
            CurrentPageReturns = new ObservableCollection<Return>(
                _allReturns.Skip((CurrentPage - 1) * _itemsPerPage)
                           .Take(_itemsPerPage)
                           .ToList());
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is Returns viewModel)
            {
                viewModel.UpdateSelectedReturns(ReturnsDataGrid.SelectedItems);
            }
        }

        public void UpdateSelectedReturns(IList selectedItems)
        {
            SelectedReturns = selectedItems.Cast<Return>().ToList();
        }

        private ReturnManager _returnManager;

        public System.Windows.Visibility MultiSelect { get; set; }

        private void btn_AddReturn_Click(object sender, RoutedEventArgs e)
        {
            //var _user = new Return();
            //var userDialog = new SecondaryWindow(_user);
            //if (userDialog.ShowDialog() == true)
            //{
            //    _context.SaveChanges();
            //    LoadReturns();
            //}
        }

        private void btn_UpdateReturn_Click(object sender, RoutedEventArgs e)
        {
            if (ReturnsDataGrid.SelectedItems.Count != 1) return;
            var selectedReturn = ReturnsDataGrid.SelectedItem as Return;
            if (selectedReturn != null /*&& selectedReturn.Debt == 0*/)
            {
                //var userDialog = new SecondaryWindow(selectedReturn);
                //if (userDialog.ShowDialog() == true)
                //{
                //    _context.SaveChanges();
                //    LoadReturns();
                //}
            }
            else if (selectedReturn == null)
            {
                MessageBox.Show("Please select a user to edit.");
            }
            //else if (selectedReturn.Debt != 0)
            //{
            //    MessageBox.Show("Return owes debt, unable to edit.");
            //}
            else
            {
                MessageBox.Show("Return owes book, unable to edit.");
            }
        }

        private void btn_DeleteReturn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var Return in SelectedReturns)
            { 
                if (Return != null /*&& selectedReturn.Debt == 0 */)
                {
                    _context.Returns.Remove(Return);
                    _context.SaveChanges();
                    LoadReturns();
                }
                else if (Return == null)
                {
                    MessageBox.Show("Please select a return to delete.");
                    continue;
                }
                //else if (selectedReturn.Debt != 0)
                //{
                //    MessageBox.Show("Return owes debt, unable to delete.");
                //    continue;
                //}
                else
                {
                    MessageBox.Show("Return owes book, unable to delete.");
                    continue;
                }
            }
        }

        private void Returns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReturnsDataGrid.SelectedItems.Count > 1)
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
                var query = context.Returns.AsQueryable();

                if (!string.IsNullOrEmpty(Search))
                {
                    query = query.Where(r =>
                        r.Username.Contains(Search));
                }

                _allReturns = new ObservableCollection<Return>(
                    query.ToList());

                CurrentPage = 1;
                UpdateCurrentPageReturns();
                GeneratePageButtons();
            }
        }

        /* Pagination */

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

            PaginationPanel.Children.Add(CreatePageButton("<<", PreviousPage_Click));

            int startPage = Math.Max(1, CurrentPage - 2);
            int endPage = Math.Min(TotalPages, startPage + 4);

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
