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
using System.Windows.Shapes;
using Library_BUS;
using Library_DAL;
using Library_DTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library_GUI
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// ??
    /// </summary>
    public partial class UserWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public UserManager Instance;
        public UserWindow()
        {
            InitializeComponent();
            DataContext = this;
            Display.Text = "Login";
            CurrentControl = new InputLogin();
            (CurrentControl as InputLogin).SwitchControlRequested += OnSwitchControlRequested;
            (CurrentControl as InputLogin).LoginSucceeded += OnLoginSucceeded;
        }

        private void OnLoginSucceeded(object? sender, User user)
        {
            MainWindow MainContent = new MainWindow(user);
            MainContent.Show();
            this.Close();
        }

        private object _currentControl;
        public object CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;
                OnPropertyChanged();
            }
        }
        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSwitchControlRequested(object sender, string e)
        {
            switch (e)
            {
                case "InputLogin":
                    CurrentControl = new InputLogin();
                    (CurrentControl as InputLogin).SwitchControlRequested += OnSwitchControlRequested;
                    break;
                case "InputSignUp":
                    CurrentControl = new InputSignUp();
                    (CurrentControl as InputSignUp).SwitchControlRequested += OnSwitchControlRequested;
                    break;
            }
        }

        private void btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            icon_Close.Opacity = 0.7;
        }

        private void btn_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            icon_Close.Opacity = 0.3;
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
