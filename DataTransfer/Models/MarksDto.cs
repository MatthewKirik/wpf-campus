using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Models
{
    public class MarksDto
    {
        public List<LessonDto> Lessons { get; set; }
        public List<StudentDto> Students { get; set; }
        public int[,] Marks { get; set; }
    }
}
