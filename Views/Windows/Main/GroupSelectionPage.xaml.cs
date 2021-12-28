using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels;
using ViewModels.Windows;

namespace Views.Windows.Main
{
    /// <summary>
    /// Логика взаимодействия для GroupSelectionPage.xaml
    /// </summary>
    public partial class GroupSelectionPage : Page
    {
        public GroupSelectionPage()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<MainWindowVM>();
        }
    }
}
