using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Library_DAL;
using Library_DTO;
using Library_GUI.UserControls;

namespace Library_GUI
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ReaderRepository _readerRepository = new();
        private AdminRepository _adminRepository = new();

        public MainWindow(User user)
        {
            InitializeComponent();

            var _reader = _readerRepository.GetByUsername(user.Username);
            if (_reader == null)
            {
                _user = _adminRepository.GetByUsername(user.Username);
                DisplayName = (_user as Admin).LastName;
                CurrentContent = new Dashboard(_user as Admin);
                CurrentButton = btn_Dashboard;
            }
            else
            {
                _user = _reader;
                DisplayName = (_user as Reader).LastName;
                CurrentContent = new Dashboard(_user as Reader);
                CurrentButton = btn_Dashboard;
            }
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

        public object _user { get;private set; }

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
            if (_user is Admin)
                CurrentContent = new Dashboard(_user as Admin);
            else CurrentContent = new Dashboard(_user as Reader);
            (CurrentButton as Button).Style = this.FindResource("menuButton") as Style;
            (sender as Button).Style = this.FindResource("menuButtonActive") as Style;
            CurrentButton = sender;
        }
    }
}