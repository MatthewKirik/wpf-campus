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
    public class LessonRepository : Repository, ILessonRepository
    {
        public LessonRepository(CampusContext comuseContext, IMapper mapper) 
            : base(comuseContext, mapper)
        {
        }

        public async Task AddLesson(int groupId, int subjectId, LessonDto newLesson)
        {
            var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                throw new ArgumentOutOfRangeException();
            var subject = await db.Subjects
                .FirstOrDefaultAsync(s => s.Id == subjectId);
            if (subject == null)
                throw new ArgumentOutOfRangeException();
            var lessonEntity = mapper.Map<LessonEntity>(newLesson);
            lessonEntity.Subject = subject;
            lessonEntity.Group = group;
            db.Lessons.Add(lessonEntity);
            await db.SaveChangesAsync();
        }

        public async Task AddMark(int lessonId, int studentId, int mark)
        {
            var existing = await db.Marks
                .FirstOrDefaultAsync(x => x.Lesson.Id == lessonId && x.Student.Id == studentId);
            if (existing != null)
            {
                existing.Mark = mark;
                await db.SaveChangesAsync();
            }
            else
            {
                var lesson = await db.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
                if (lesson == null)
                    throw new ArgumentOutOfRangeException();
                var student = await db.Students.FirstOrDefaultAsync(x => x.Id == studentId);
                if (student == null)
                    throw new ArgumentOutOfRangeException();
                var markEntity = new MarkEntity()
                {
                    Student = student,
                    Lesson = lesson,
                    Mark = mark
                };
                db.Marks.Add(markEntity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<LessonDto>> GetLessons(int groupId, int subjectId, int year, int semester)
        {
            DateTime fromDate, toDate;
            if (semester == 1)
            {
                fromDate = new DateTime(year, 9, 1);
                toDate = new DateTime(year + 1, 1, 1);
            }
            else
            {
                fromDate = new DateTime(year + 1, 1, 1);
                toDate = new DateTime(year + 1, 6, 30);
            }

            var lessons = await db.Lessons.Where(x =>
                x.Group.Id == groupId
                && x.Subject.Id == subjectId
                && x.Date >= fromDate
                && x.Date < toDate)
                .ToListAsync();

            return mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<MarksDto> GetMarks(int groupId, int subjectId, int year, int semester)
        {
            DateTime fromDate, toDate;
            if (semester == 1)
            {
                fromDate = new DateTime(year, 9, 1);
                toDate = new DateTime(year + 1, 1, 1);
            }
            else
            {
                fromDate = new DateTime(year + 1, 1, 1);
                toDate = new DateTime(year + 1, 6, 30);
            }

            var lessons = await db.Lessons.Where(x =>
                x.Group.Id == groupId
                && x.Subject.Id == subjectId
                && x.Date >= fromDate
                && x.Date < toDate)
                .Include(x => x.Marks)
                .ThenInclude(x => x.Student)
                .OrderBy(x => x.Date)
                .ToListAsync();
            var students = await db.Students
                .Where(x => x.Group.Id == groupId)
                .OrderBy(x => x.IndexInGroup)
                .ToListAsync();

            var marks = new MarksDto()
            {
                Students = mapper.Map<List<StudentDto>>(students),
                Lessons = mapper.Map<List<LessonDto>>(lessons),
                Marks = new int[students.Count, lessons.Count]
            };

            for (int j = 0; j < lessons.Count; j++)
            {
                foreach (var mark in lessons[j].Marks)
                {
                    int i = students.FindIndex(x => x.Id == mark.Student.Id);
                    marks.Marks[i, j] = mark.Mark;
                }
            }

            return marks;
        }

        public async Task RemoveLesson(int lessonId)
        {
            var lesson = await db.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson == null)
                throw new ArgumentOutOfRangeException();
            db.Lessons.Remove(lesson);
            await db.SaveChangesAsync();
        }

        public async Task RemoveMark(int lessonId, int studentId)
        {
            var mark = await db.Marks
                .FirstOrDefaultAsync(x => x.Lesson.Id == lessonId && x.Student.Id == studentId);
            if (mark != null)
            {
                db.Marks.Remove(mark);
                await db.SaveChangesAsync();
            }
        }
    }
}
