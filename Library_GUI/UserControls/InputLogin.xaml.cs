using Library_DAL;
using Library_DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using Library_GUI;
using Library_BUS;

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
            LoadSettings();
            _unitOfWork = new(_libraryContext);
            _userManager = new(_unitOfWork);
        }
        public event EventHandler<string> SwitchControlRequested;
        public event EventHandler<User> LoginSucceeded;
        private UserManager _userManager;
        private LibraryContext _libraryContext = new();
        private UnitOfWork _unitOfWork;

        public bool Remember { get; set; }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            //login code
            if (_userManager.ValidateUser(txbUsername.Text, txbPassword.Password))
            {
                MessageBox.Show("Login succesfull", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                if (Remember)
                    SaveSettings();
                else ClearSettings();
                User user = _userManager.GetByUsername(txbUsername.Text);
                LoginSucceeded?.Invoke(this, user);
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LoadSettings()
        {
            txbUsername.Text = Library_GUI.Settings.Default.Username;
            txbPassword.Password = Library_GUI.Settings.Default.Password;
            Remember = Library_GUI.Settings.Default.RememberMe;
        }

        private void SaveSettings()
        {
            Library_GUI.Settings.Default.Username = txbUsername.Text;
            Library_GUI.Settings.Default.Password = txbPassword.Password;
            Library_GUI.Settings.Default.RememberMe = Remember;
            Library_GUI.Settings.Default.Save();
        }

        private void ClearSettings()
        {
            Library_GUI.Settings.Default.Username = "";
            Library_GUI.Settings.Default.Password = "";
            Library_GUI.Settings.Default.RememberMe = false;
            Library_GUI.Settings.Default.Save();
        }

        private void btn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            SwitchControlRequested?.Invoke(this, "InputSignUp");
        }
    }
}
