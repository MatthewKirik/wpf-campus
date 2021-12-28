using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Models
{
    public class GroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Faculty { get; set; }

        public List<StudentDto> Students { get; set; }
    }
}
