using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class SubjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ScheduledLesson> ScheduledLessons { get; set; } = new List<ScheduledLesson>();
        public List<LessonEntity> Lessons { get; set; } = new List<LessonEntity>();
    }
}
