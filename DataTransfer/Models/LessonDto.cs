using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Models
{
    public class LessonDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }
    }
}
