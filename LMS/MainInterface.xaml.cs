using LoginApp.Data;
using System.Windows;
using System.Windows.Controls;

namespace LoginApp
{
    public partial class MainInterface : Window
    {
        private readonly AppDbContext _context;
        public MainInterface()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dashboard");
        }

        private void ReaderManagement_Click(object sender, RoutedEventArgs e)
        {
            ReaderManagementPage readerManagementPage = new ReaderManagementPage();
            readerManagementPage.Show();
        }

        private void BookManagement_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("BookManagement");
        }

        private void LoanManagement_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("LoanManagement");
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Report");
        }
    }
}