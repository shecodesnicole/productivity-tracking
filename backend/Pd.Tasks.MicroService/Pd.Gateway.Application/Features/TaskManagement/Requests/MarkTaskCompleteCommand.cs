using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Requests
{
    public class MarkTaskCompleteCommand
    {
        public int Id { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    }
}
