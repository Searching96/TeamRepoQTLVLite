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
        public User User { get; private set; }

        private UserManager context = new();

        public EventHandler<bool> CloseDialog;

        public UserDialog(User? user = null)
        {
            InitializeComponent();
            User = user ?? new User();
        }

        private void UserSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbUsername.Text) || string.IsNullOrWhiteSpace(txbPassword.Text) || string.IsNullOrWhiteSpace(txbDisplayName.Text))
            {
                MessageBox.Show("Please fill in all fields.");
            }
            else
            {
                if (User.UserId == 0) context.AddUser(txbUsername.Text, txbPassword.Text, txbDisplayName.Text);
                else context.UpdateUser(User.UserId, txbUsername.Text, txbPassword.Text, txbDisplayName.Text);
            }
            CloseDialog?.Invoke(this, true);
        }

        private void UserCancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog?.Invoke(this, false);
        }
    }
}
