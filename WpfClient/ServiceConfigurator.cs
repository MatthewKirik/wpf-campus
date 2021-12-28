using AutoMapperConfiguration;
using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using ViewModels;
using ViewModels.Base;
using ViewModels.Windows;
using Views;
using Views.Windows.Modals;
using Views.Windows.Utils;

namespace WpfClient
{
    class ServiceConfigurator
    {
        public static ServiceProvider Provider;

        public static void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Provider = serviceCollection.BuildServiceProvider();
            ServiceConfiguration.Services.Provider = Provider;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(AutoMapperConfigurator.GetMapper());

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();

            services.AddDbContext<CampusContext>();

            services.AddSingleton<MainWindowVM>();
            services.AddSingleton<MainWindow>();

            services.AddTransient<AddGroupWindowVM>();
            services.AddTransient<AddGroupWindow>();

            services.AddTransient<AddStudentWindowVM>();
            services.AddTransient<AddStudentWindow>();

            services.AddTransient<AddSubjectWindowVM>();
            services.AddTransient<AddSubjectWindow>();

            services.AddTransient<AddLessonWindowVM>();
            services.AddTransient<AddLessonWindow>();

            services.AddScoped<IWindowCreator, WindowCreator>();
        }
}
}
