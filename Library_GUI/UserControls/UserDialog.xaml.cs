using Library_BUS;
using Library_DAL;
using Library_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : UserControl
    {
        private readonly UserManager _userManager;
        private readonly ReaderManager _readerManager;
        public User User { get; private set; }
        public EventHandler<bool> CloseDialog;

        public UserDialog(UserManager userManager, ReaderManager readerManager, User? user = null)
        {
            InitializeComponent();
            _userManager = userManager;
            _readerManager = readerManager;
            User = user ?? new User();

            if (user != null)
            {
                PopulateFields(user);
            }
        }

        private void PopulateFields(User user)
        {
            txbUsername.Text = user.Username;
            txbPassword.Text = user.Password;
            txbEmail.Text = user.Email;
            // Add other fields as needed
        }

        private void UserSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    User.Username = txbUsername.Text;
                    User.Password = txbPassword.Text;
                    User.Email = txbEmail.Text;
                    User.TypeOfUser = "Reader"; // Default type for new users

                    if (User.Username != null && _userManager.GetByUsername(User.Username) != null)
                    {
                        _userManager.UpdateUser(User.Username,User.Password,User.TypeOfUser, User.Email);
                    }
                    else
                    {
                        _userManager.AddUser(User.Username, User.Password, User.TypeOfUser, User.Email);
                        
                        // Create associated reader
                        var reader = new Reader 
                        { 
                            Username = User.Username,
                            // Add other reader properties as needed
                        };
                        _readerManager.AddReader(reader.Username,reader.FirstName, reader.LastName, reader.ReaderTypeId,reader.StartDate);
                    }

                    MessageBox.Show("User saved successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CloseDialog?.Invoke(this, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving user: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txbUsername.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txbPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txbEmail.Text))
            {
                MessageBox.Show("Please enter an email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void UserCancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog?.Invoke(this, false);
        }
    }
}
