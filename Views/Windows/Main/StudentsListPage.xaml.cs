using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels.Windows;

namespace Views.Windows.Main
{
    /// <summary>
    /// Логика взаимодействия для StudentsListPage.xaml
    /// </summary>
    public partial class StudentsListPage : Page
    {
        public StudentsListPage()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<MainWindowVM>();
        }
    }
}
