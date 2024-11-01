using System;
using Library_DTO;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library_GUI.UserControls;
using Library_BUS;
using Library_DAL;

namespace Library_GUI
{
    public partial class SecondaryWindow : Window, INotifyPropertyChanged
    {
        private readonly IUnitOfWork _unitOfWork;
        
        // Managers
        private readonly BookManager _bookManager;
        private readonly ReaderManager _readerManager;
        private readonly BorrowManager _borrowManager;
        private readonly ReturnManager _returnManager;
        private readonly UserManager _userManager;

        public SecondaryWindow()
        {
            // Initialize UnitOfWork with LibraryContext
            var context = new LibraryContext();
            _unitOfWork = new UnitOfWork(context);

            // Initialize managers with UnitOfWork
            _bookManager = new BookManager(_unitOfWork);
            _readerManager = new ReaderManager(_unitOfWork);
            _borrowManager = new BorrowManager(_unitOfWork);
            _returnManager = new ReturnManager(_unitOfWork);
            _userManager = new UserManager(_unitOfWork);

            InitializeComponent();
        }

        // Constructor for User Dialog
        public SecondaryWindow(User selectedUser) : this()
        {
            CurrentContent = new UserDialog(_userManager, _readerManager, selectedUser);
            (CurrentContent as UserDialog).CloseDialog += OnCloseDialog;
        }

        // Constructor for Book Dialog
        public SecondaryWindow(Book selectedBook) : this()
        {
            CurrentContent = new BookDialog(_bookManager, selectedBook);
            (CurrentContent as BookDialog).CloseDialog += OnCloseDialog;
        }

        // Constructor for Borrow Dialog
        public SecondaryWindow(bool isBorrowDialog) : this()
        {
            if (isBorrowDialog)
            {
                CurrentContent = new BookBorrowDialog(_borrowManager, _readerManager, _bookManager);
                (CurrentContent as BookBorrowDialog).CloseDialog += OnCloseDialog;
            }
        }

        private void OnCloseDialog(object? sender, bool e = false)
        {
            DialogResult = e;
            Close();
        }

        // INotifyPropertyChanged implementation
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _unitOfWork.Dispose();
        }
    }
}
