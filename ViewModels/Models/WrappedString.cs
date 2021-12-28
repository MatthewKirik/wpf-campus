using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Models
{
    public class WrappedString
    {
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
            }
        }
    }
}
