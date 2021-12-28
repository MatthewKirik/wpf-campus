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
    public class GroupRepository : Repository, IGroupRepository
    {
        public GroupRepository(CampusContext comuseContext, IMapper mapper)
            : base(comuseContext, mapper)
        { }

        public async Task<GroupDto> AddGroup(string faculty, GroupDto group)
        {
            var groupEntity = mapper.Map<GroupEntity>(group);
            groupEntity.Faculty = faculty;
            db.Groups.Add(groupEntity);
            await db.SaveChangesAsync();
            return mapper.Map<GroupDto>(groupEntity);
        }

        public async Task<List<string>> GetFaculties()
        {
            var faculties = await db.Groups
                .Select(g => g.Faculty)
                .Distinct()
                .ToListAsync();
            return faculties;
        }

        public async Task<GroupDto> GetGroup(int id)
        {
            var group = await db.Groups
                .Include(x => x.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
                throw new ArgumentOutOfRangeException();
            return mapper.Map<GroupDto>(group);
        }

        public async Task<List<GroupDto>> GetGroups(string faculty = null)
        {
            var query = db.Groups
                .AsNoTracking()
                .AsQueryable();
            if (faculty != null)
            {
                query = query.Where(g => g.Faculty == faculty);
            }
            var groups = await query.ToListAsync();
            return mapper.Map<List<GroupDto>>(groups);
        }

        public async Task RemoveGroup(int groupId)
        {
            var group = await db.Groups
                .Include(x => x.Students)
                .FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
                throw new ArgumentOutOfRangeException();
            db.Groups.Remove(group);
            await db.SaveChangesAsync();
        }
    }
}
