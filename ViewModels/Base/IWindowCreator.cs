using System;
using System.Windows;

namespace ViewModels.Base
{
    public interface IWindowCreator
    {
        Window CreateWindow(string name, params ValueTuple<string, object>[] ps);
    }
}
