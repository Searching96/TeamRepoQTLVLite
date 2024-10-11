using QLTVLite.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace QLTVLite
{
    public partial class WDTrangDocGia : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Sach> Books { get; set; }
        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage)); // Thông báo thay đổi
                }
            }
        }
        public int BooksPerPage { get; set; }
        public int TotalBooks { get; set; }
        public RelayCommand NextPageCommand { get; }
        public RelayCommand PreviousPageCommand { get; }

        public WDTrangDocGia()
        {
            InitializeComponent();
            Books = new ObservableCollection<Sach>();
            BooksPerPage = 12; // Số lượng sách trên mỗi trang
            CurrentPage = 1; // Trang hiện tại
            DataContext = this; // Đặt DataContext cho Window để sử dụng binding

            // Khởi tạo Commands
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);

            DisplayBooks(); // Gọi hàm để hiển thị sách
        }

        public void DisplayBooks()
        {
            using (var context = new AppDbContext())
            {
                // Lấy danh sách sách từ cơ sở dữ liệu
                var booksFromDb = context.SACH.ToList();
                TotalBooks = booksFromDb.Count; // Lưu tổng số sách

                // Tính số lượng sách cần hiển thị dựa trên trang hiện tại
                var booksToDisplay = booksFromDb
                    .Skip((CurrentPage - 1) * BooksPerPage)
                    .Take(BooksPerPage)
                    .ToList();

                // Clear current books and add from database
                Books.Clear();
                foreach (var book in booksToDisplay)
                {
                    Books.Add(book);
                }
            }
        }

        public void NextPage()
        {
            if (CurrentPage * BooksPerPage < TotalBooks) // Kiểm tra xem có còn trang tiếp theo không
            {
                CurrentPage++;
                DisplayBooks(); // Hiển thị sách cho trang tiếp theo
            }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1) // Kiểm tra xem có còn trang trước không
            {
                CurrentPage--;
                DisplayBooks(); // Hiển thị sách cho trang trước
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}