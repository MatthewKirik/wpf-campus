using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IStudentRepository
    {
        Task<StudentDto> GetStudent(int id);
        Task<StudentDto> GetStudent(string email);
        Task<StudentDto> AddStudent(int groupId, StudentDto student);
        Task RemoveStudent(int studentId);
    }
}
