using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels;
using ViewModels.Windows;

namespace Views.Windows.Main
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<MainWindowVM>();
        }
    }
}
