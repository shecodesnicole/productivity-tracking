using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Requests
{
    public class UpdateTaskCommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
