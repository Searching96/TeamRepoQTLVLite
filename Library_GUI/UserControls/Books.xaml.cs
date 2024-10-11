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

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Books : UserControl
    {
        private LibraryContext _context;

        public Books()
        {
            InitializeComponent();
            _context = new();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UsersDataGrid.ItemsSource = _context.Users.ToList();
        }

        private UserRepository _userRepository = new();

        private void btn_AddUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as User;
            if (selectedUser != null && selectedUser.Debt == 0 && selectedUser.BookCount == 0)
            {
                var userDialog = new UserDialog(selectedUser);
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
            var selectedUser = UsersDataGrid.SelectedItem as User;
            if (selectedUser != null && selectedUser.Debt == 0 && selectedUser.BookCount == 0)
            {
                _context.Users.Remove(selectedUser);
                _context.SaveChanges();
                LoadUsers();
            }
            else if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to delete.");
            }
            else if (selectedUser.Debt != 0)
            {
                MessageBox.Show("User owes debt, unable to delete.");
            }
            else
            {
                MessageBox.Show("User owes book, unable to delete.");
            }
        }
    }
}
