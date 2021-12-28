using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class StudentEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Image { get; set; }

        public int IndexInGroup { get; set; }

        public GroupEntity Group { get; set; }

        public List<MarkEntity> Marks { get; set; } = new List<MarkEntity>();
    }
}
