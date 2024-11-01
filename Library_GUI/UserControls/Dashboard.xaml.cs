using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Library_DAL;
using Library_BUS;

namespace Library_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Dashboard : UserControl, INotifyPropertyChanged
    {
        public Dashboard(Admin admin)
        {
            InitializeComponent();
            _user = admin;
            Time = GetTime();
            _unitOfWork = new(_context);
            _bookManager = new(_unitOfWork);
            _userManager = new(_unitOfWork);
            _borrowManager = new(_unitOfWork);
            DisplayName = admin.FirstName;
            BookCount = _bookManager.Count().ToString();
            UserCount = _userManager.Count().ToString();
            BorrowCount = _borrowManager.Count().ToString();
        }

        public Dashboard(Reader reader)
        {
            InitializeComponent();
            _user = reader;
            Time = GetTime();
            _unitOfWork = new(_context);
            _bookManager = new(_unitOfWork);
            _userManager = new(_unitOfWork);
            _borrowManager = new(_unitOfWork);
            DisplayName = reader.FirstName;
            BookCount = _bookManager.Count().ToString();
            UserCount = _userManager.Count().ToString();
            BorrowCount = _borrowManager.Count().ToString();
        }

        private string _bookCount;
        public string BookCount
        {
            get => _bookCount;
            set
            {
                _bookCount = value;
                OnPropertyChanged();
            }
        }

        private string _userCount;
        public string UserCount
        {
            get => _userCount;
            set
            {
                _userCount = value;
                OnPropertyChanged();
            }
        }

        private string _borrowCount;
        public string BorrowCount
        {
            get => _borrowCount;
            set
            {
                _borrowCount = value;
                OnPropertyChanged();
            }
        }

        private LibraryContext _context = new();
        private UnitOfWork _unitOfWork;

        private BookManager _bookManager;
        private UserManager _userManager;
        private BorrowManager _borrowManager;

        public event PropertyChangedEventHandler? PropertyChanged;

        public object _user { get; private set; }

        private string _time;
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public static string GetTime()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 5 && hour < 11)
                return "morning";
            else if (hour >= 11 && hour < 17)
                return "afternoon";
            else if (hour >= 17 && hour < 21)
                return "evening";
            else return "night";
        }

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


    }


}
