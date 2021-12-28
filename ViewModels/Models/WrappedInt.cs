using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Models
{
    public class WrappedInt
    {
        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
            }
        }
    }
}
