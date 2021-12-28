using ServiceConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModels.Base;

namespace Views.Windows.Utils
{
    public class WindowCreator : IWindowCreator
    {
        public Window CreateWindow(string name, params ValueTuple<string, object>[] ps)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            var type = types.FirstOrDefault(x => x.Name == name);
            var window = Services.Provider.GetService(type) as Window;
            if (window.DataContext is VMBase)
            {
                var context = window.DataContext as VMBase;
                var parameters = new Dictionary<string, object>();
                foreach (var param in ps)
                {
                    parameters.Add(param.Item1, param.Item2);
                }
                context.ParentParameters = parameters;
            }
            return window;
        }
    }
}
