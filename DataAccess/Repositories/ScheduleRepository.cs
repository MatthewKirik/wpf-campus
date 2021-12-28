using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Implementations;
using DataTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ScheduleRepository : Repository, IScheduleRepository
    {
        public ScheduleRepository(CampusContext comuseContext, IMapper mapper) : base(comuseContext, mapper)
        {
        }

        public async Task AddScheduledLesson(int groupId, int subjectId, ScheduledLessonDto scheduledLesson)
        {
            var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                throw new ArgumentOutOfRangeException();
            var subject = await db.Subjects
                .FirstOrDefaultAsync(s => s.Id == subjectId);
            if (subject == null)
                throw new ArgumentOutOfRangeException();
            var scheduledLessonEntity = mapper.Map<ScheduledLesson>(scheduledLesson);
            scheduledLessonEntity.Subject = subject;
            scheduledLessonEntity.Group = group;
            db.ScheduledLesson.Add(scheduledLessonEntity);
            await db.SaveChangesAsync();
        }

        public async Task RemoveScheduledLesson(int groupId, int subjectId, ScheduledLessonDto scheduledLesson)
        {
            var lesson = await db.ScheduledLesson
                .FirstOrDefaultAsync(x =>
                    x.Group.Id == groupId
                    && x.Subject.Id == subjectId
                    && x.LessonIndex == scheduledLesson.LessonIndex
                    && x.Semester == scheduledLesson.Semester
                    && x.WeekIndex == scheduledLesson.WeekIndex
                    && x.Year == scheduledLesson.Year
                    && x.DayOfWeek == scheduledLesson.DayOfWeek);
            if (lesson == null) return;
            db.ScheduledLesson.Remove(lesson);
            await db.SaveChangesAsync();
        }

        private string[] times =
        {
            "",
            "08:30",
            "10:25",
            "12:20",
            "14:15",
            "16:10",
            "18:30",
            "",
            "",
            "",
            "",
            "",
            ""
        };

        public async Task<ScheduleDto> GetSchedule(int groupId, int subjectId, int year, int semester)
        {
            var scheduledLessons = await db.ScheduledLesson
                .Where(x => 
                    x.Group.Id == groupId 
                    && x.Subject.Id == subjectId
                    && x.Year == year
                    && x.Semester == semester)
                .ToListAsync();
            var scheduleDto = new ScheduleDto
            {
                GroupId = groupId,
                SubjectId = subjectId,
                Semester = semester,
                Year = year
            };
            var week1Lessons = scheduledLessons
                .Where(x => x.WeekIndex == 1)
                .GroupBy(x => x.LessonIndex)
                .OrderBy(x => x.Key)
                .ToList();
            var week2Lessons = scheduledLessons
                .Where(x => x.WeekIndex == 2)
                .GroupBy(x => x.LessonIndex)
                .OrderBy(x => x.Key)
                .ToList();
            var week1 = new List<LessonScheduleDto>();
            var week2 = new List<LessonScheduleDto>();
            for (int i = 1; i <= 6; i++)
            {
                var l1 = new List<IsScheduledValue>(8);
                var l2 = new List<IsScheduledValue>(8);
                for (int j = 0; j < 8; j++)
                {
                    l1.Add(new IsScheduledValue()
                    {
                        DayOfWeek = j,
                        Value = false
                    });
                    l2.Add(new IsScheduledValue()
                    {
                        DayOfWeek = j,
                        Value = false
                    });
                }
                var lessonWeek1 = new LessonScheduleDto()
                {
                    LessonIndex = i,
                    WeekIndex = 1,
                    IsScheduled = l1,
                    Time = times[i]
                };
                var lessonWeek2 = new LessonScheduleDto()
                {
                    LessonIndex = i,
                    WeekIndex = 2,
                    IsScheduled = l2,
                    Time = times[i]
                };
                week1.Add(lessonWeek1);
                week2.Add(lessonWeek2);
            }
            scheduleDto.Week1 = week1;
            scheduleDto.Week2 = week2;
            foreach (var group in week1Lessons)
            {
                int lessonIndex = group.Key;
                foreach (var lesson in group)
                {
                    scheduleDto.Week1[lessonIndex - 1].IsScheduled[lesson.DayOfWeek].Value = true;
                }
            }
            foreach (var group in week2Lessons)
            {
                int lessonIndex = group.Key;
                foreach (var lesson in group)
                {
                    scheduleDto.Week2[lessonIndex - 1].IsScheduled[lesson.DayOfWeek].Value = true;
                }
            }
            return scheduleDto;
        }

        public async Task<List<ScheduledLessonDto>> GetScheduledLessons(int groupId, int subjectId, int year, int semester)
        {
            var scheduledLessons = await db.ScheduledLesson
                .Where(x =>
                    x.Group.Id == groupId
                    && x.Subject.Id == subjectId
                    && x.Year == year
                    && x.Semester == semester)
                .ToListAsync();
            return mapper.Map<List<ScheduledLessonDto>>(scheduledLessons);
        }
    }
}
