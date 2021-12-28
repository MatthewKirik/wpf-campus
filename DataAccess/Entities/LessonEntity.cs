using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class LessonEntity
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }

        public GroupEntity Group { get; set; }

        public SubjectEntity Subject { get; set; }

        public List<MarkEntity> Marks { get; set; } = new List<MarkEntity>();
    }
}
