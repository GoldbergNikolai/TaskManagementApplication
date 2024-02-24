using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApplication.Models;
using TaskManagementApplication.Services;

namespace TaskManagementApplication.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            List<TaskModel> taskModels = await _taskService.GetTaskListAsync();
            return View(taskModels);
        }

        [HttpPost]
        public async Task<JsonResult> UpsertTask(TaskModel taskModel)
        {
            bool result = false;

            try
            {
                if (!string.IsNullOrWhiteSpace(taskModel.Name)
                    && (taskModel.DueDate != DateTime.MinValue && taskModel.DueDate != DateTime.MaxValue)
                    && !string.IsNullOrWhiteSpace(taskModel.Description))
                {
                    result = await _taskService.UpsertTaskAsync(taskModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpsertTask failed. " +
                    $"Task Id={taskModel?.Id}, " +
                    $"Task Name={taskModel?.Name}, " +
                    $"Due Date={taskModel?.DueDate} " +
                    $"Description={taskModel?.Description} " +
                    $"Status={taskModel?.Status} " +
                    $",Message={ex.Message}", ex);
            }

            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTask(int id)
        {
            bool result = false;

            try
            {
                if (id > 0)
                {
                    result = await _taskService.DeleteTaskAsync(id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteTask failed. Task Id={id}, Message={ex.Message}", ex);
            }

            return new JsonResult(result);
        }
    }
}
