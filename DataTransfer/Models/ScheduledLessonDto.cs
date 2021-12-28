using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Models
{
    public class ScheduledLessonDto
    {
        public int Id { get; set; }

        public int LessonIndex { get; set; }

        public int DayOfWeek { get; set; }

        public int WeekIndex { get; set; }

        public int Year { get; set; }

        public int Semester { get; set; }
    }
}
