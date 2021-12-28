using AutoMapper;
using DataAccess.Entities;
using DataTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementations
{
    public class StudentRepository : Repository, IStudentRepository
    {
        public StudentRepository(CampusContext comuseContext, IMapper mapper)
            : base(comuseContext, mapper)
        { }

        public async Task<StudentDto> AddStudent(int groupId, StudentDto student)
        {
            var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                throw new ArgumentOutOfRangeException();
            int lastIndex = await 
                db.Students
                .Where(x => x.Group.Id == group.Id)
                .MaxAsync(x => x.IndexInGroup);
            var studentEntity = mapper.Map<StudentEntity>(student);
            studentEntity.Group = group;
            studentEntity.IndexInGroup = lastIndex + 1;
            db.Students.Add(studentEntity);
            await db.SaveChangesAsync();
            return mapper.Map<StudentDto>(studentEntity);
        }

        public async Task<StudentDto> GetStudent(int studentId)
        {
            var student = await db.Students.FirstOrDefaultAsync(u => u.Id == studentId);
            if (student == null)
                throw new ArgumentOutOfRangeException();
            return mapper.Map<StudentDto>(student);
        }
        public async Task<StudentDto> GetStudent(string email)
        {
            var student = await db.Students.FirstOrDefaultAsync(u => u.Email == email);
            if (student == null)
                throw new ArgumentOutOfRangeException();
            return mapper.Map<StudentDto>(student);
        }

        public async Task RemoveStudent(int studentId)
        {
            var student = await db.Students.FirstOrDefaultAsync(u => u.Id == studentId);
            if (student == null)
                throw new ArgumentOutOfRangeException();
            db.Students.Remove(student);
            await db.SaveChangesAsync();
        }
    }
}
