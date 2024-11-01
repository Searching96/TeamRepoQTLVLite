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
using Library_BUS;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Library_GUI
{
    /// <summary>
    /// Interaction logic for InputSignUp.xaml
    /// </summary>
    public partial class InputSignUp : UserControl
    {
        private LibraryContext _context = new();
        private UnitOfWork _unitOfWork;

        private UserManager _userContext = new();
        private ReaderManager _readerContext = new();
        public event EventHandler<string> SwitchControlRequested;
        public event EventHandler<User> LoginSucceeded;
        public InputSignUp()
        {
            InitializeComponent();
            _unitOfWork = new(_context);
            _userContext = new(_unitOfWork);
            _readerContext = new(_unitOfWork);
        }

        private void btn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            //Sign up code
            if (txbPassword.Password != txbRePassword.Password) return;
            User newUser = new User { };
            if (_userContext.GetByUsername(newUser.Username) == null)
            {
                _userContext.AddUser(txbUsername.Text, txbPassword.Password, txbEmail.Text, "Reader");
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
