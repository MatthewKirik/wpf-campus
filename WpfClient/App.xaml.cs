using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Views;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            ServiceConfigurator.ConfigureServices();
        }


        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceConfigurator.Provider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
