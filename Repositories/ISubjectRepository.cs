using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ISubjectRepository
    {
        Task<SubjectDto> GetSubject(int id);
        Task<List<SubjectDto>> GetSubjects();
        Task<SubjectDto> AddSubject(SubjectDto subject);
        Task RemoveSubject(int subjectId);
    }
}
