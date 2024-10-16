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

        private UserRepository _userRepository = new();

        private ReaderRepository _readerContext = new();

        public EventHandler<bool> CloseDialog;

        public UserDialog(User? user = null)
        {
            InitializeComponent();
            User = user ?? new User();
        }

        private void UserSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbUsername.Text) || string.IsNullOrWhiteSpace(txbPassword.Text))
            {
                MessageBox.Show("Please fill in all fields.");
            }
            else
            {
                User.Username = txbUsername.Text;
                User.Password = txbPassword.Text;
                User.Email = txbDisplayName.Text;
                User.TypeOfUser = "Reader";
                if (_userRepository.GetByUsername(txbUsername.Text) != null)
                {
                    _userRepository.Update(User);
                }
                else
                {
                    _userRepository.Add(User);
                    Reader newReader = new Reader {Username = User.Username};
                    _readerContext.Add(newReader);
                }
            }
            CloseDialog?.Invoke(this, true);
        }

        private void UserCancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog?.Invoke(this, false);
        }
    }
}
