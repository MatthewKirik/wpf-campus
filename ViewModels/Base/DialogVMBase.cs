using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Base
{
    public abstract class DialogVMBase : VMBase
    {
        public DialogVMBase()
          : base()
        {
        }

        private bool? _dialogResult;
        public bool? DialogResultValue
        {
            get { return _dialogResult; }
            set
            {
                if (_dialogResult != value)
                {
                    _dialogResult = value;
                    this.RaisePropertyChanged(nameof(DialogResultValue));
                }
            }
        }
    }
}
