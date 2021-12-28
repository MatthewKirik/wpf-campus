using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CampusContext : DbContext
    {
        //public string DbPath { get; }

        //public CampusContext(DbContextOptions<CampusContext> options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=./campus.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<LessonEntity> Lessons { get; set; }
        public DbSet<MarkEntity> Marks { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<ScheduledLesson> ScheduledLesson { get; set; }
    }
}

