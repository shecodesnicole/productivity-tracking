using Pd.Gateway.Application.Integration.Tasks.Models;

namespace Pd.Gateway.Application.Integration.Tasks.Requests
{
    public class AddTaskCommand
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public ProdashTaskStatus Status { get; set; } = ProdashTaskStatus.ToDo;
        public int HoursWorked { get; set; } = 0;
    }
}
