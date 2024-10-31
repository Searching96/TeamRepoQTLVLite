using System;
using Library_DTO;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library_GUI.UserControls;
using Library_BUS;
using Library_DAL;

namespace Library_GUI
{
    /// <summary>
    /// Interaction logic for SecondaryWindow.xaml
    /// </summary>
    public partial class SecondaryWindow : Window, INotifyPropertyChanged
    {
        private readonly BookRepository _bookRepository;
        private readonly ReaderRepository _readerRepository;
        private readonly BorrowRepository _borrowRepository;
        private readonly BorrowDetailRepository _borrowDetailRepository;
        private readonly UserRepository _userRepository;
        public SecondaryWindow(User selectedUser)
        {
            InitializeComponent();
            CurrentContent = new UserDialog(selectedUser);
            (CurrentContent as UserDialog).CloseDialog += OnCloseDialog;
        }

        public SecondaryWindow(Book selectedBook)
        {
            InitializeComponent();
            CurrentContent = new BookDialog(selectedBook);
            (CurrentContent as BookDialog).CloseDialog += OnCloseDialog;
        }

        public SecondaryWindow(Borrow selectedBorrow)
        {
            InitializeComponent();
            _borrowRepository = new();
            _borrowDetailRepository = new();
            _readerRepository = new();
            _bookRepository = new();
            CurrentContent = new BookBorrowDialog(_borrowRepository, _borrowDetailRepository, _readerRepository, _bookRepository , selectedBorrow);
            (CurrentContent as BookDialog).CloseDialog += OnCloseDialog;
        }

        private void OnCloseDialog(object? sender,bool e = false)
        {
            DialogResult = e;
            this.Close();
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
