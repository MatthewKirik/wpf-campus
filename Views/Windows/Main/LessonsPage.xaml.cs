using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfiguration;
using ViewModels;
using ViewModels.Models;
using ViewModels.Windows;

namespace Views.Windows.Main
{
    /// <summary>
    /// Логика взаимодействия для LessonsPage.xaml
    /// </summary>
    public partial class LessonsPage : Page
    {
        public LessonsPage()
        {
            InitializeComponent();
            this.DataContext = Services.Provider.GetService<MainWindowVM>();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataTemplate t = null;

            if (e.PropertyType == typeof(NotifiedCellValue))
                t = (sender as DataGrid).Resources["NotifiedCellTemplate"] as DataTemplate;
            else if (e.PropertyType == typeof(WrappedString))
                t = (sender as DataGrid).Resources["StringTemplate"] as DataTemplate;
            else if (e.PropertyType == typeof(WrappedInt))
                t = (sender as DataGrid).Resources["IntTemplate"] as DataTemplate;
            e.Column = new DataGridTemplateColumn
            {
                CellTemplate = t,
                Header = e.Column.Header,
                HeaderTemplate = e.Column.HeaderTemplate,
                HeaderStringFormat = e.Column.HeaderStringFormat,
                SortMemberPath = e.PropertyName
            };
        }
    }
}
