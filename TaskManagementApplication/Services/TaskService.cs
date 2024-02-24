using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TaskManagementApplication.Controllers;
using TaskManagementApplication.Data;
using TaskManagementApplication.Models;

namespace TaskManagementApplication.Services
{
    public class TaskService : ITaskService
    {
        #region Private Variables

        private readonly ILogger<TaskService> _logger;
        private readonly TaskContext _dbContext;

        #endregion


        #region constructors

        public TaskService(TaskContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion


        #region Public Methods

        public async Task<bool> UpsertTaskAsync(TaskModel taskModel)
        {
            try
            {
                using (var db = _dbContext)
                {
                    TaskData task = ConvertTaskModelToTaskData(taskModel);

                    if (task.Id > 0)
                    {
                        var taskToUpdate = await db.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == task.Id);
                        if (taskToUpdate != null)
                        {
                            db.Tasks.Update(task);
                        }
                    }
                    else
                    {
                        await db.Tasks.AddAsync(task);
                    }

                    return await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpsertTaskAsync failed. Message={ex.Message}", ex);
            }

            return false;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            using (var db = _dbContext)
            {
                var taskToDelete = await db.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

                if (taskToDelete != null)
                {
                    db.Tasks.Remove(taskToDelete);
                    return await db.SaveChangesAsync() > 0;
                }
            }

            return false;
        }

        public async Task<List<TaskModel>> GetTaskListAsync()
        {
            var tasks = new List<TaskData>();

            using (var db = _dbContext)
            {
                tasks = await db.Tasks.ToListAsync();
            }

            return tasks.Any() ? tasks.Select(taskData => ConvertTaskDataToTaskModel(taskData)).ToList() : new List<TaskModel>();
        }

        #endregion


        #region Private Methods

        //Simple map instead of using say AutoMap
        private TaskModel ConvertTaskDataToTaskModel(TaskData task) => new TaskModel
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status
        };

        private TaskData ConvertTaskModelToTaskData(TaskModel task) => new TaskData
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status
        };

        #endregion
    }
}
