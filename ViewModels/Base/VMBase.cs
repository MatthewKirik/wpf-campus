using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Base
{
    public abstract class VMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, object> _ParentParameters;
        public Dictionary<string, object> ParentParameters 
        {
            get => _ParentParameters;
            set
            {
                _ParentParameters = value;
                OnParentParametersSet();
            }
        }

        protected virtual void OnParentParametersSet() { }

        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
