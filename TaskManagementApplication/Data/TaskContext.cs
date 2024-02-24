using Microsoft.EntityFrameworkCore;
using System;

namespace TaskManagementApplication.Data
{
    public class TaskContext : DbContext
    {
        public DbSet<TaskData> Tasks { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }
    }

    public class TaskData
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime DueDate { get; set; }
        public Enums.Enums.TaskStatus Status { get; set; }
    }
}
