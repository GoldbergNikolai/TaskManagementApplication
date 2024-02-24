namespace TaskManagementApplication.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Enums.Enums.TaskStatus Status { get; set; }
    }
}
