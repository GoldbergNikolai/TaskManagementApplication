using TaskManagementApplication.Models;

namespace TaskManagementApplication.Services
{
    public interface ITaskService
    {
        //public Task<List<TaskModel>> GetTaskList(string name, string description, DateTime dueDate, TaskStatus status);
        public Task<List<TaskModel>> GetTaskListAsync();
        public Task<bool> UpsertTaskAsync(TaskModel taskModel);
        public Task<bool> DeleteTaskAsync(int id);
    }
}
