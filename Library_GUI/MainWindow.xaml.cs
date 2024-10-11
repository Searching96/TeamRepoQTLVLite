using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Library_DTO;
using Library_GUI.UserControls;

namespace Library_GUI
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow(User user)
        {
            InitializeComponent();
            DisplayName = user.DisplayName;
            _user = user;
            CurrentContent = new Dashboard(user);
            CurrentButton = btn_Dashboard;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private object _currentContent;
        public object CurrentContent
        {
            get => _currentContent;
            set
            {
                _currentContent = value;
                OnPropertyChanged();
            }
        }

        public object CurrentButton { get; set; }

        public User _user { get;private set; }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            UserWindow LoginContent = new();
            LoginContent.Show();
            this.Close();
        }

        private void btn_Messages_Click(object sender, RoutedEventArgs e)
        {
            //Extra
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            //Not planned
        }

        private void btn_Users_Click(object sender, RoutedEventArgs e)
        {
            CurrentContent = new Users();
            (CurrentButton as Button).Style = this.FindResource("menuButton") as Style;
            (sender as Button).Style = this.FindResource("menuButtonActive") as Style;
            CurrentButton = sender;
        }

        private void btn_Revenue_Click(object sender, RoutedEventArgs e)
        {
            //Extra
        }

        private void btn_Statistics_Click(object sender, RoutedEventArgs e)
        {
            //Extra
        }

        private void btn_Borrows_Click(object sender, RoutedEventArgs e)
        {
            //Deadline 16/11
        }

        private void btn_Books_Click(object sender, RoutedEventArgs e)
        {
            //Deadline 16/11
        }

        private void btn_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            CurrentContent = new Dashboard(_user);
            (CurrentButton as Button).Style = this.FindResource("menuButton") as Style;
            (sender as Button).Style = this.FindResource("menuButtonActive") as Style;
            CurrentButton = sender;
        }
    }
}