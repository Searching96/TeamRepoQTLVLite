using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
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
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : UserControl, INotifyPropertyChanged
    {
        private UserManager _userManager;

        private ObservableCollection<Reader> _allReaders;
        private ObservableCollection<Reader> _currentPageReaders;
        private int _itemsPerPage = 10;
        private int _currentPage = 1;
        public ObservableCollection<Reader> CurrentPageReaders
        {
            get => _currentPageReaders;
            set
            {
                _currentPageReaders = value;
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
                UpdateCurrentPageReaders();
            }
        }

        private void UpdateCurrentPageReaders()
        {
            CurrentPageReaders = new ObservableCollection<Reader>(
                _allReaders.Skip((CurrentPage - 1) * _itemsPerPage)
                           .Take(_itemsPerPage)
                           .ToList());
        }

        public int TotalPages => (int)Math.Ceiling((double)_allReaders.Count / _itemsPerPage);

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

        private List<Reader> _selectedReaders;
        public List<Reader> SelectedReaders
        {
            get => _selectedReaders;
            set
            {
                _selectedReaders = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Users()
        {
            InitializeComponent();
            DataContext = this;
            _userManager = new();
            LoadReaders();
            MultiSelect = Visibility.Visible;
            GeneratePageButtons();
        }

        private void LoadReaders()
        {
            using (var context = new LibraryManagementContext())
            {
                _allReaders = new ObservableCollection<Reader>(
                    context.Readers.Include(r => r.ReaderType)
                                   .Include(r => r.UsernameNavigation)
                                   .ToList());
                UpdateCurrentPageReaders();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is Users viewModel)
            {
                viewModel.UpdateSelectedReaders(UsersDataGrid.SelectedItems);
            }
        }

        public void UpdateSelectedReaders(IList selectedItems)
        {
            SelectedReaders = selectedItems.Cast<Reader>().ToList();
        }

        public System.Windows.Visibility MultiSelect { get; set; }

        private void btn_AddUser_Click(object sender, RoutedEventArgs e)
        {
            var _user = new User();
            var userDialog = new SecondaryWindow(_user);
            if (userDialog.ShowDialog() == true)
            {
                LoadReaders();
            }
        }

        private void btn_UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItems.Count != 1) return;
            var selectedUser = UsersDataGrid.SelectedItem as Reader;
            if (selectedUser != null && selectedUser.CurrentBorrows == 0)
            {
                var userDialog = new SecondaryWindow(selectedUser.UsernameNavigation);
                if (userDialog.ShowDialog() == true)
                {
                    LoadReaders();
                }
            }
            else
            {
                MessageBox.Show("User owes book, unable to edit.");
            }
        }

        private void btn_DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            foreach (var reader in SelectedReaders)
            {
                if (reader != null && reader.CurrentBorrows == 0)
                {
                    _userManager.RemoveUser(reader.Username);
                    LoadReaders();
                }
                else
                {
                    MessageBox.Show("User owes book, unable to delete.");
                }
            }
        }

        private void Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MultiSelect = UsersDataGrid.SelectedItems.Count > 1 ? Visibility.Visible : Visibility.Hidden;
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new LibraryManagementContext())
            {
                var query = context.Readers.AsQueryable();

                if (!string.IsNullOrEmpty(Search))
                {
                    query = query.Where(r => r.Username.Contains(Search) ||
                                             r.FirstName.Contains(Search) ||
                                             r.LastName.Contains(Search) ||
                                             r.UsernameNavigation.Email.Contains(Search));
                }

                _allReaders = new ObservableCollection<Reader>(query.ToList());
                UpdateCurrentPageReaders();
            }
        }

        // Pagination

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
                var pageButton = CreatePageButton(i.ToString(), PageNumber_Click);
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
