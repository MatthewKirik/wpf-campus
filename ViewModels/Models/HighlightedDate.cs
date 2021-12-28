using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Models
{
    public class HighlightedDate
    {
        public HighlightedDate(DateTime date, string description)
        {
            Date = date;
            Description = description;
        }

        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
