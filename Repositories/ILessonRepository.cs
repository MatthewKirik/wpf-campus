using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ILessonRepository
    {
        Task<List<LessonDto>> GetLessons(int groupId, int subjectId, int year, int semester);
        Task<MarksDto> GetMarks(int groupId, int subjectId, int year, int semester);
        Task AddLesson(int groupId, int subjectId, LessonDto newLesson);
        Task RemoveLesson(int lessonId);
        Task AddMark(int lessonId, int studentId, int mark);
        Task RemoveMark(int lessonId, int studentId);
    }
}
