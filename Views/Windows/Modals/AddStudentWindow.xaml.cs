using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels.Windows;


namespace Views.Windows.Modals
{
    /// <summary>
    /// Логика взаимодействия для AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : MetroWindow
    {
        public AddStudentWindow()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<AddStudentWindowVM>();
        }
    }
}
