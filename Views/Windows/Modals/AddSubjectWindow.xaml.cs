using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels.Windows;

namespace Views.Windows.Modals
{
    /// <summary>
    /// Логика взаимодействия для AddSubjectWindow.xaml
    /// </summary>
    public partial class AddSubjectWindow : MetroWindow
    {
        public AddSubjectWindow()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<AddSubjectWindowVM>();
        }
    }
}
