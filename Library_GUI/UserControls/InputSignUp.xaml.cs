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

namespace Library_GUI
{
    /// <summary>
    /// Interaction logic for InputSignUp.xaml
    /// </summary>
    public partial class InputSignUp : UserControl
    {
        private LibraryContext _context;
        public event EventHandler<string> SwitchControlRequested;
        public InputSignUp()
        {
            InitializeComponent();
            //_context = new LibraryContext();
            //LoadUsers();
        }
        private void btn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            //Sign up code
            
        }
        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            SwitchControlRequested?.Invoke(this, "InputLogin");
        }
    }
}
