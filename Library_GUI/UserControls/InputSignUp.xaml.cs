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
using Library_DAL;
using Library_DTO;

namespace Library_GUI
{
    /// <summary>
    /// Interaction logic for InputSignUp.xaml
    /// </summary>
    public partial class InputSignUp : UserControl
    {
        private UserRepository _userContext = new();
        private ReaderRepository _readerContext = new();
        public event EventHandler<string> SwitchControlRequested;
        public event EventHandler<User> LoginSucceeded;
        public InputSignUp()
        {
            InitializeComponent();
        }

        private void btn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            //Sign up code
            if (txbPassword.Password != txbRePassword.Password) return;
            User newUser = new User { Username = txbUsername.Text,Password = txbPassword.Password, Email = txbEmail.Text, TypeOfUser = "Reader"};
            if (!_userContext.CheckExist(newUser))
            {
                _userContext.Add(newUser);
                Reader newReader = new Reader { Username = newUser.Username };
                _readerContext.Add(newReader);
                MessageBox.Show("Register succesfull", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginSucceeded?.Invoke(this, newUser);
            }
            else
            {
                MessageBox.Show("User already registered", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            SwitchControlRequested?.Invoke(this, "InputLogin");
        }
    }
}
