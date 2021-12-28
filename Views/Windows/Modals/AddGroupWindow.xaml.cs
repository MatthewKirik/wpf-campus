using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels.Windows;

namespace Views.Windows.Modals
{
    /// <summary>
    /// Логика взаимодействия для AddGroupWindow.xaml
    /// </summary>
    public partial class AddGroupWindow : MetroWindow
    {
        public AddGroupWindow()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<AddGroupWindowVM>();
        }
    }
}
