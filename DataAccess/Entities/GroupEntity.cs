using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class GroupEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Faculty { get; set; }

        public List<StudentEntity> Students { get; set; } = new List<StudentEntity>();
        public List<ScheduledLesson> ScheduledLessons { get; set; } = new List<ScheduledLesson>();
        public List<LessonEntity> Lessons { get; set; } = new List<LessonEntity>();
    }
}
