using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Models
{
    public enum ProdashTaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3
    }

    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProdashTaskStatus  Status { get; set; }             
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }             
        public DateTime? CompletedAt { get; set; }
        public int HoursWorked { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
