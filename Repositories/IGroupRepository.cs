using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IGroupRepository
    {
        Task<GroupDto> GetGroup(int id);
        Task<List<GroupDto>> GetGroups(string faculty = null);
        Task<GroupDto> AddGroup(string faculty, GroupDto group);
        Task<List<string>> GetFaculties();
        Task RemoveGroup(int groupId);
    }
}
