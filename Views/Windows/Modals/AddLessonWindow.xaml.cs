using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels.Windows;


namespace Views.Windows.Modals
{
    /// <summary>
    /// Логика взаимодействия для AddLessonWindow.xaml
    /// </summary>
    public partial class AddLessonWindow : Window
    {
        public AddLessonWindow()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<AddLessonWindowVM>();
        }
    }
}
