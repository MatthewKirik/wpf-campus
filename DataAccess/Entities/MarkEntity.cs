using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class MarkEntity
    {
        public int Id { get; set; }

        public int Mark { get; set; }

        public LessonEntity Lesson { get; set; }

        public StudentEntity Student { get; set; }
    }
}
