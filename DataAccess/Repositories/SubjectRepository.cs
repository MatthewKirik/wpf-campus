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
    public class SubjectRepository : Repository, ISubjectRepository
    {
        public SubjectRepository(CampusContext comuseContext, IMapper mapper) 
            : base(comuseContext, mapper)
        {
        }

        public async Task<SubjectDto> AddSubject(SubjectDto subject)
        {
            var subjectEntity = mapper.Map<SubjectEntity>(subject);
            db.Subjects.Add(subjectEntity);
            await db.SaveChangesAsync();
            return mapper.Map<SubjectDto>(subjectEntity);
        }

        public async Task<SubjectDto> GetSubject(int id)
        {
            var subject = await db.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null)
                throw new ArgumentOutOfRangeException();
            return mapper.Map<SubjectDto>(subject);
        }

        public async Task<List<SubjectDto>> GetSubjects()
        {
            var subjects = await db.Subjects
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<SubjectDto>>(subjects);
        }

        public Task RemoveSubject(int subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
