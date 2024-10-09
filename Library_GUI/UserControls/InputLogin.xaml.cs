using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
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

namespace Library_GUI
{
    /// <summary>
    /// Interaction logic for InputLogin.xaml
    /// </summary>
    public partial class InputLogin: UserControl
    {
        public InputLogin()
        {
            InitializeComponent();

        }
        public event EventHandler<string> SwitchControlRequested;
        public event EventHandler<User> LoginSucceeded;
        private UserRepository context;
        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            //login code
            User user = new User { Username = txbUsername.Text, Password = txbPassword.Password };
            context = new();
            if (context.CheckExist(user))
            {
                MessageBox.Show("Login succesfull", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginSucceeded?.Invoke(this, user);
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            SwitchControlRequested?.Invoke(this, "InputSignUp");
        }
    }
}
