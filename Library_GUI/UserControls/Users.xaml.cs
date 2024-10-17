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

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : UserControl, INotifyPropertyChanged
    {
        private LibraryContext _context;

        private ObservableCollection<Reader> _readers;
        public ObservableCollection<Reader> Readers
        {
            get => _readers;
            set
            {
                _readers = value;
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
            _context = new();
            LoadReaders();
            MultiSelect = Visibility.Visible;
        }

        private void LoadReaders()
        {
            using (var context = new LibraryManagementContext())
            {
                Readers = new ObservableCollection<Reader>(
                    context.Readers.Include(r => r.ReaderType)
                                   .Include(r => r.UsernameNavigation)
                                   .ToList());
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

        private UserRepository _userRepository = new();

        public System.Windows.Visibility MultiSelect { get; set; }

        private void btn_AddUser_Click(object sender, RoutedEventArgs e)
        {
            var _user = new User();
            var userDialog = new SecondaryWindow(_user);
            if (userDialog.ShowDialog() == true)
            {
                _context.SaveChanges();
                LoadReaders();
            }
        }

        private void btn_UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItems.Count != 1) return;
            var selectedUser = UsersDataGrid.SelectedItem as Reader;
            if (selectedUser != null /*&& selectedUser.Debt == 0*/ && selectedUser.CurrentBorrows == 0)
            {
                var userDialog = new SecondaryWindow(selectedUser.UsernameNavigation);
                if (userDialog.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadReaders();
                }
            }
            else if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to edit.");
            }
            //else if (selectedUser.Debt != 0)
            //{
            //    MessageBox.Show("User owes debt, unable to edit.");
            //}
            else
            {
                MessageBox.Show("User owes book, unable to edit.");
            }
        }

        private void btn_DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            foreach (var Reader in SelectedReaders)
            {
                var selectedUser = Reader.UsernameNavigation;
                //Check for current user
                if (selectedUser != null /*&& selectedUser.Debt == 0 */ && Reader.CurrentBorrows == 0)
                {
                    _context.Users.Remove(selectedUser);
                    _context.SaveChanges();
                    LoadReaders();
                }
                else if (selectedUser == null)
                {
                    MessageBox.Show("Please select a user to delete.");
                    continue;
                }
                //else if (selectedUser.Debt != 0)
                //{
                //    MessageBox.Show("User owes debt, unable to delete.");
                //    continue;
                //}
                else
                {
                    MessageBox.Show("User owes book, unable to delete.");
                    continue;
                }
            }
        }

        private void Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItems.Count > 1)
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
                var query = context.Readers.AsQueryable();

                if (!string.IsNullOrEmpty(Search))
                {
                    query = query.Where(r =>
                    r.Username.Contains(Search) ||
                        (r.Username != null && r.Username.Contains(Search)) ||
                        (r.FirstName != null && r.FirstName.Contains(Search)) ||
                        (r.LastName != null && r.LastName.Contains(Search)) ||
                        (r.UsernameNavigation.Email != null && r.UsernameNavigation.Email.Contains(Search)));
                    Readers = new ObservableCollection<Reader>(query.ToList());
                }
                else
                {
                    Readers = new ObservableCollection<Reader>(
                        context.Readers.Include(r => r.ReaderType)
                       .Include(r => r.UsernameNavigation)
                       .ToList());
                }
            }
        }
    }
}
