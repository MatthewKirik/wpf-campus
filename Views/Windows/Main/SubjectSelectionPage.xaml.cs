using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels.Windows;

namespace Views.Windows.Main
{
    /// <summary>
    /// Логика взаимодействия для SubjectSeletionPage.xaml
    /// </summary>
    public partial class SubjectSelectionPage : Page
    {
        public SubjectSelectionPage()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<MainWindowVM>();
        }
    }
}
