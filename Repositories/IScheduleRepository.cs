using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IScheduleRepository
    {
        Task<ScheduleDto> GetSchedule(int groupId, int subjectId, int year, int semester);
        Task<List<ScheduledLessonDto>> GetScheduledLessons(int groupId, int subjectId, int year, int semester);
        Task AddScheduledLesson(int groupId, int subjectId, ScheduledLessonDto scheduledLesson);
        Task RemoveScheduledLesson(int groupId, int subjectId, ScheduledLessonDto scheduledLesson);
    }
}
