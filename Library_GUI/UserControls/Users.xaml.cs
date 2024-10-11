using Library_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls.Primitives;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Users : UserControl
    {
        private LibraryContext _context;

        public Users()
        {
            InitializeComponent();
            _context = new();
            LoadUsers();
            MultiSelect = Visibility.Hidden;
        }

        private void LoadUsers()
        {
            UsersDataGrid.ItemsSource = _context.Users.ToList();
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
                LoadUsers();
            }
        }

        private void btn_UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItems.Count != 1) return;
            var selectedUser = UsersDataGrid.SelectedItem as User;
            if (selectedUser != null && selectedUser.Debt == 0 && selectedUser.BookCount == 0)
            {
                var userDialog = new SecondaryWindow(selectedUser);
                if (userDialog.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadUsers();
                }
            }
            else if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to edit.");
            }
            else if (selectedUser.Debt != 0)
            {
                MessageBox.Show("User owes debt, unable to edit.");
            }
            else
            {
                MessageBox.Show("User owes book, unable to edit.");
            }
        }

        private void btn_DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < UsersDataGrid.SelectedItems.Count; i++)
            {
                var selectedUser = UsersDataGrid.SelectedItems[i] as User;
                //Check for current user
                if (selectedUser != null && selectedUser.Debt == 0 && selectedUser.BookCount == 0)
                {
                    _context.Users.Remove(selectedUser);
                    _context.SaveChanges();
                    LoadUsers();
                }
                else if (selectedUser == null)
                {
                    MessageBox.Show("Please select a user to delete.");
                    continue;
                }
                else if (selectedUser.Debt != 0)
                {
                    MessageBox.Show("User owes debt, unable to delete.");
                    continue;
                }
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
    }
}
