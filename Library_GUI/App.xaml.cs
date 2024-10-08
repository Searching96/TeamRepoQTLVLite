using Library_DAL;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Library_GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new LibraryContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
